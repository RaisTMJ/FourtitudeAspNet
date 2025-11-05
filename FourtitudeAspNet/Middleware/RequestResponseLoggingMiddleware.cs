using log4net;
using System.Text;

namespace FourtitudeAspNet.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly ILog log = LogManager.GetLogger(typeof(RequestResponseLoggingMiddleware));

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log request
            var requestBody = await ReadRequestBodyAsync(context.Request);
            var maskedRequestBody = MaskPartnerPassword(requestBody);
            log.Info($"Request: {context.Request.Method} {context.Request.Path} - Body: {maskedRequestBody}");

            // Capture response
            var originalResponseBodyStream = context.Response.Body;
            using var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            await _next(context);

            // Log response
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
            log.Info($"Response: {context.Response.StatusCode} - Body: {responseBody}");

            // Copy back to original stream
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(originalResponseBodyStream);
        }

        private async Task<string> ReadRequestBodyAsync(HttpRequest request)
        {
            request.EnableBuffering();
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            request.Body.Position = 0;
            return body;
        }

        private string MaskPartnerPassword(string body)
        {
            var pattern = "\"partnerpassword\"\\s*:\\s*\"[^\"]*\"";
            var replacement = "\"partnerpassword\":\"********\"";
            return System.Text.RegularExpressions.Regex.Replace(body, pattern, replacement, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }
    }
}