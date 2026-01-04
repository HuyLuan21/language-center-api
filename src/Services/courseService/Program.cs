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

            var app = builder.Build();

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
