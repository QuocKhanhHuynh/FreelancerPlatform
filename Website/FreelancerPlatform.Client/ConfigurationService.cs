using FluentValidation.AspNetCore;
using FreelancerPlatform.Application.Dtos.Account;
using FreelancerPlatform.Application.Dtos.Contract;
using FreelancerPlatform.Client.Models;
using FreelancerPlatform.Client.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

namespace FreelancerPlatform.Client
{
    public static class ConfigurationService
    {
        public static IServiceCollection AddPresentService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddTransient<IStorageService, FileStorageService>();
            services.AddTransient<IVnPayService, VnPaymentService>();

            services.AddSession(x => x.IdleTimeout = TimeSpan.FromMinutes(60));
            services.AddHttpClient();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpClient();
            services.AddHttpContextAccessor();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
            {
                x.LoginPath = "/home";
            }).AddGoogle(GoogleDefaults.AuthenticationScheme, option =>
            {
               
            });

			services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<PasswordUpdateRequestValidator>());
            services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<FormFreelancerUpdateRequestValidator>());

            return services;
        }
    }
}
