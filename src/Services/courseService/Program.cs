using courseService.Repositories;             // <-- THÊM
using courseService.Repositories.Interfaces;  // <-- THÊM
using courseService.Services;

namespace courseService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<DbContext>();  // <-- THÊM
            builder.Services.AddScoped<ICourseRepository, CourseRepository>(); // <-- THÊM
            builder.Services.AddScoped<CourseService>(); // <-- THÊM
       
            // Swagger
            builder.Services.AddControllers();


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            string[] allowedOrigins = new string[]
            {
                "http://localhost:5173",
                "http://127.0.0.1:5500",
            };

            // Thêm CORS service
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(corsBuilder =>
                {
                    corsBuilder
                        .WithOrigins(allowedOrigins)
                        .WithMethods("GET", "POST", "PUT", "PATCH", "DELETE")
                        .WithHeaders("Content-Type", "Authorization");
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.UseCors();

            app.Run();

        }
    }
}
