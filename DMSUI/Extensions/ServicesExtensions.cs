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
            services.AddScoped<IPositionManager, PositionManager>();
            services.AddScoped<IDepartmentManager, DepartmentManager>();
            services.AddScoped<IRoleManager, RoleManager>();
            services.AddScoped<ICompanyManager, CompanyManager>();

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
			services.AddHttpClient<IPositionApiClient, PositionApiClient>(client =>
			{
				var apiSettings = configuration.GetSection("APISettings").Get<APISettings>();
				client.BaseAddress = new Uri(apiSettings.BaseUrl);
			});
            services.AddHttpClient<IDepartmentApiClient, DepartmentApiClient>(client =>
			{
				var apiSettings = configuration.GetSection("APISettings").Get<APISettings>();
				client.BaseAddress = new Uri(apiSettings.BaseUrl);
			});
            services.AddHttpClient<IRoleApiClient, RoleApiClient>(client =>
            {
				var apiSettings = configuration.GetSection("APISettings").Get<APISettings>();
				client.BaseAddress = new Uri(apiSettings.BaseUrl);
			});
            services.AddHttpClient<ICompanyApiClient, CompanyApiClient>(client =>
            {
                var apiSettings = configuration.GetSection("APISettings").Get<APISettings>();
                client.BaseAddress = new Uri(apiSettings.BaseUrl);
            });


            services.AddTransient<ApiAuthMessageHandler>();
            return services;
        }
    }
}
