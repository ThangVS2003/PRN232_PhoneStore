using Microsoft.EntityFrameworkCore;
using PhoneStore.Repositories;
using PhoneStore.Repositories.IRepositories;
using PhoneStore.Repositories.Repositories;
using PhoneStore.Services.IServices;
using PhoneStore.Services.Services;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using PhoneStore.BusinessObjects.Models;
using Microsoft.OpenApi.Models;

namespace PhoneStoreAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<Prn232PhoneContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn") ?? throw new InvalidOperationException("Connection string 'MyCnn' not found.")));
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Đăng ký Repositories
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IBrandRepository, BrandRepository>();
            // Đăng ký Services
            builder.Services.AddScoped<IBrandService, BrandService>();
            builder.Services.AddScoped<IProductService, ProductService>();
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
