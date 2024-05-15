using LockoutSample.Application.Interfaces;
using LockoutSample.Application.Services;
using LockoutSample.Domain.Repositories;
using LockoutSample.Infrastructure.Data;
using LockoutSample.Infrastructure.Repositories;
using LockoutSample.Presentation.Handlers;
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

            app.UseExceptionHandler();

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

            services.AddScoped<ILockoutService, LockoutService>();

            services.AddProblemDetails();
            services.AddExceptionHandler<ExceptionToProblemDetailsHandler>();
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
