using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.ApiService
{
    public static class ConfigurationService
    {
        public static IServiceCollection AddApiService(this IServiceCollection services)
        {
            services.AddTransient<ISystemRecommendationApiService, SystemRecommendationApiService>();

            return services;
        }
    }
}
