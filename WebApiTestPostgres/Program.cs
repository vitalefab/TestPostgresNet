
using Microsoft.EntityFrameworkCore;
using WebApiTestPostgres.Models;
using WebApiTestPostgres.Repositories;
using WebApiTestPostgres.Services;

namespace WebApiTestPostgres
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configurazione Entity Framework Core
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Configurazione Dapper
            builder.Services.AddSingleton<DapperContext>();
            builder.Services.AddScoped<PersonRepository>();

            // Servizio che decide se usare EF Core o Dapper
            builder.Services.AddScoped<PersonService>();

            var app = builder.Build();

            try
            {
                // Esegue la migrazione automatica all'avvio
                using (var scope = app.Services.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    dbContext.Database.Migrate();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
