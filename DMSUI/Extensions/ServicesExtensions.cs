using DMSUI.ApiAuthMessageHandlers;
using DMSUI.Business;
using DMSUI.Business.Interfaces;
using DMSUI.Services;
using DMSUI.Services.Interfaces;
using DMSUI.Settings;
using System.Runtime.CompilerServices;

namespace DMSUI.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddDmsServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();

            services.Configure<APISettings>(configuration.GetSection("APISettings"));
            
            services.AddScoped<IAuthManager, AuthManager>();    
            services.AddScoped<IUserManager, UserManager>();    


            services.AddHttpClient<IAuthApiClient, AuthApiClient>(client =>
            {
                var apiSettings = configuration.GetSection("APISettings").Get<APISettings>();
                client.BaseAddress = new Uri(apiSettings.BaseUrl);
            })
                 .AddHttpMessageHandler<ApiAuthMessageHandler>();
            services.AddHttpClient<IUserApiClient, UserApiClient>(client =>
            {
                var apiSettings = configuration.GetSection("APISettings").Get<APISettings>();
                client.BaseAddress = new Uri(apiSettings.BaseUrl);
            });
            

           
            services.AddTransient<ApiAuthMessageHandler>();
            return services;
        }
    }
}
