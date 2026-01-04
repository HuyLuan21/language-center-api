using examService.Repositories; 
using examService.Repositories.Interfaces;
using examService.Services;    
using examService.Services.Interfaces;

namespace examService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Add services to the container (Đăng ký các Controller)
            builder.Services.AddControllers();

            // 2. Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // --- BẮT ĐẦU: ĐĂNG KÝ DEPENDENCY INJECTION (DI) ---
            // Đây là phần bạn bị thiếu dẫn đến lỗi

            // a. Đăng ký DbContext (Class wrapper ADO.NET của bạn)
            builder.Services.AddScoped<DbContext>();

            // b. Đăng ký Repository (Map Interface -> Implementation)
            builder.Services.AddScoped<IExamRepository, ExamRepository>();

            // c. Đăng ký Service (Map Interface -> Implementation)
            // Lỗi "Unable to resolve service" chính là do thiếu dòng này
            builder.Services.AddScoped<IExamService, ExamService>();

            // --- KẾT THÚC: ĐĂNG KÝ DI ---

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