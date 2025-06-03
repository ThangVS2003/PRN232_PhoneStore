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
using Microsoft.OData.Edm;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace PhoneStoreAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<Prn232PhoneContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")
                ?? throw new InvalidOperationException("Connection string 'MyCnn' not found.")));

            // Cấu hình JSON sử dụng Newtonsoft.Json và xử lý vòng lặp tham chiếu
            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver(); // Giữ nguyên tên property
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore; // Bỏ qua giá trị null
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; // Bỏ qua vòng lặp tham chiếu
                })
                .AddOData(opt => opt.AddRouteComponents("odata", GetEdmModel())
                    .Select().Expand().Filter().OrderBy().Count().SetMaxTop(100));

            // Add Swagger with JWT support
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PhoneStoreAPI", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            // Đăng ký Repositories
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IBrandRepository, BrandRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            // Đăng ký Services
            builder.Services.AddScoped<IBrandService, BrandService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

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

        private static IEdmModel GetEdmModel()
        {
            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<User>("Users");
            odataBuilder.EntitySet<Product>("Products");
            odataBuilder.EntitySet<Brand>("Brands");
            return odataBuilder.GetEdmModel();
        }
    }
}
