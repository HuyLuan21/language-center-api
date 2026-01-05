using classService.Repositories;             // <-- THÊM
using classService.Repositories.Interfaces;  // <-- THÊM
using classService.Services;
namespace classService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<DbContext>();  // <-- THÊM
            builder.Services.AddScoped<IClassesRepository, ClassesRepository>(); // <-- THÊM
            builder.Services.AddScoped<ClassServices>(); // <-- THÊM

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
