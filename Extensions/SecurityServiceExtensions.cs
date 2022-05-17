using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessAPI.Extensions
{
    public static class SecurityServiceExtensions
    {
        public static IServiceCollection AddSecurityServices(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", policy =>
                {
                    policy.AllowAnyMethod().AllowAnyOrigin();
                });
            });

            return services;
        }
    }
}
