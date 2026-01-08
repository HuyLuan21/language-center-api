
namespace enrollmentService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            // swagger 
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();



            // DI
            builder.Services.AddSingleton<Repositories.DbContext>();
            builder.Services.AddScoped<Repositories.EnrollmentRepository>();
            builder.Services.AddScoped<Services.EnrollmentService>();

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
