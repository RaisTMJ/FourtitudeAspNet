using FourtitudeAspNet.Interface;
using FourtitudeAspNet.Services;

namespace FourtitudeAspNet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
      
            // Register custom services
            builder.Services.AddScoped<IPartnerService, PartnerService>();
            builder.Services.AddScoped<ISignatureService, SignatureService>();
            builder.Services.AddScoped<ITransactionValidationService, TransactionValidationService>();
 
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
