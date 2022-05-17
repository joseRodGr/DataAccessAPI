using DataAccessAPI.Helpers;
using DataAccessAPI.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;


namespace DataAccessAPI.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DataAccessAPI", Version = "v1" });
            });
            services.AddAutoMapper(typeof(AutomapperProfiles).Assembly);
            //services.AddScoped<IUnitOfWork, UnitOfWorkEF>();
            services.AddScoped<IUnitOfWork, UnitOfWorkDapper>();
            //services.AddScoped<IUnitOfWork, UnitOfWorkADO>();
            return services;
        }
    }
}
