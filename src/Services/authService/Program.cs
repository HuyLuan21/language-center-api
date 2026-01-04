using authService.Repositories;             // <-- THÊM
using authService.Repositories.Interfaces;  // <-- THÊM
using authService.Services;
using authService.Services.Interfaces;      // <-- THÊM

namespace authService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // =============================================================
            // KHU VỰC ĐĂNG KÝ DEPENDENCY INJECTION (DI) - QUAN TRỌNG NHẤT
            // =============================================================

            // 1. Đăng ký DbContext (Kết nối CSDL)
            // Lưu ý: Nếu class DB của mày tên khác (vd: AppDbContext), nhớ sửa lại chữ DbContext ở dưới
            builder.Services.AddScoped<DbContext>();

            // 2. Đăng ký UserRepository
            // Ý nghĩa: Khi ai cần IUserRepository -> Đưa UserRepository
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            // 3. Đăng ký AuthService
            // Ý nghĩa: Khi Controller cần IAuthService -> Đưa AuthService
            builder.Services.AddScoped<IAuthService, AuthService>();

            // =============================================================

            // Add services to the container.
            builder.Services.AddControllers();

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