using scheduleService.Repositories;
using scheduleService.Repositories.Interfaces;
using scheduleService.Services;
using scheduleService.Services.Interfaces;

namespace scheduleService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // =============================================
            // DEPENDENCY INJECTION CONFIGURATION
            // =============================================

            // 1. Register DbContext (Database connection)
            builder.Services.AddScoped<DbContext>();

            // 2. Register Repository
            builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();

            // 3. Register Service
            builder.Services.AddScoped<IScheduleService, ScheduleService>();

            // =============================================

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Schedule Service API",
                    Version = "v1",
                    Description = "API for managing class schedules in Language Center"
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Schedule Service API v1");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
