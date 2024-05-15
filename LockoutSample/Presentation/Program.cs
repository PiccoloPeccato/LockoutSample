using LockoutSample.Domain.Repositories;
using LockoutSample.Infrastructure.Data;
using LockoutSample.Infrastructure.Repositories;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LockoutSample.Presentation
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder.Services, builder.Configuration, builder.Environment);

            builder.Services.AddControllers();

            var app = builder.Build();

            app.InitializeDatabase();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapControllers();

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services,
            ConfigurationManager config, IWebHostEnvironment environment)
        {
            services.AddControllers();

            ConfigureDatabase(services, config, environment);

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }

        private static void ConfigureDatabase(IServiceCollection services,
            ConfigurationManager config, IWebHostEnvironment environment)
        {
            string connectionString = config.GetConnectionString("LockoutConnection") ??
                throw new Exception("No LockoutConnection string in configuration.ConnectionStrings.");

            services.AddDbContext<LockoutDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
                options.EnableSensitiveDataLogging(environment.IsDevelopment());
            });

            var optionsBuilder = new DbContextOptionsBuilder<LockoutDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            services.AddScoped<DatabaseHelper>();

            services.AddScoped<ILockCodeRepository, LockCodeRepository>();
        }

        private static void InitializeDatabase(this IHost host)
        {
            using IServiceScope scope = host.Services.CreateScope();

            var helper = scope.ServiceProvider.GetRequiredService<DatabaseHelper>();

            helper.SetupDatabase();
        }
    }
}
