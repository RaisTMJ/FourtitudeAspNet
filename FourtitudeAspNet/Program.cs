using FourtitudeAspNet.Interface;
using FourtitudeAspNet.Services;
using FourtitudeAspNet.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace FourtitudeAspNet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // get Conectionstring default connection
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnections");



            // Add Entity Framework with SQLite
            builder.Services.AddDbContext<FourtitudeDbContext>(options =>
                options.UseSqlite(connectionString));
      
            // Register custom services
            builder.Services.AddScoped<IPartnerService, PartnerService>();
            builder.Services.AddScoped<ISignatureService, SignatureService>();
            builder.Services.AddScoped<ITransactionValidationService, TransactionValidationService>();
            builder.Services.AddScoped<IDiscountService, DiscountService>();
 
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Ensure database is created and migrated
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FourtitudeDbContext>();
                try
                {
                    context.Database.Migrate();
                }
                catch
                {
                    // Fallback to EnsureCreated if migrations fail
                    context.Database.EnsureCreated();
                }
            }

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
