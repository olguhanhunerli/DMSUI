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
            services.AddScoped<ICategoryManager, CategoryManager>();
            services.AddScoped<IDocumentManager, DocumentManager>();
            services.AddScoped<IDocumentApprovalManager, DocumentApprovalManager>();
            services.AddScoped<IDocumentAttachmentManager, DocumentAttachmentManager>();
            services.AddScoped<ISearchManager, SearchManager>();
            services.AddScoped<IAuditManager, AuditManager>();
            services.AddScoped<IInstrumentManager, InstrumentManager>();
            services.AddScoped<ICalibrationManager, CalibrationManager>();
            services.AddScoped<ICalibrationFileManager, CalibrationFileManager>();
            services.AddScoped<ICustomerManager, CustomerManager>();

            services.AddScoped<IComplaintManager, ComplaintManager>();

            services.AddScoped<ICAPAManager, CAPAManager>();

            services.AddScoped<ICapaActionsManager, CapaActionsManager>();

            services.AddScoped<ICapaActionFilesManager, CapaActionFilesManager>();

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
            services.AddHttpClient<ICategoryApiClient, CategoryApiClient>(client =>
            {
                var apiSettings = configuration.GetSection("APISettings").Get<APISettings>();
                client.BaseAddress = new Uri(apiSettings.BaseUrl);
            });
            services.AddHttpClient<IDocumentApiClient, DocumentApiClient>(client =>
            {
                var apiSettings = configuration.GetSection("APISettings").Get<APISettings>();
                client.BaseAddress = new Uri(apiSettings.BaseUrl);
            });

            services.AddHttpClient<IDocumentApprovalApiClient, DocumentApprovalApiClient>(client =>
            {
                var apiSettings = configuration.GetSection("APISettings").Get<APISettings>();
                client.BaseAddress = new Uri(apiSettings.BaseUrl);
            });
            services.AddHttpClient<IDocumentAttachmentApiClient, DocumentAttachmentApiClient>(client =>
            {
                var apiSettings = configuration.GetSection("APISettings").Get<APISettings>();
                client.BaseAddress = new Uri(apiSettings.BaseUrl);
            });
			services.AddHttpClient<ISearchApiClient, SearchApiClient>(client =>
			{
				var apiSettings = configuration.GetSection("APISettings").Get<APISettings>();
				client.BaseAddress = new Uri(apiSettings.BaseUrl);
			});
            services.AddHttpClient<IAuditApiClient, AuditApiClient>(client =>
            {
                var apiSettings = configuration.GetSection("APISettings").Get<APISettings>();
                client.BaseAddress = new Uri(apiSettings.BaseUrl);
            });
			services.AddHttpClient<IInstrumentApiClient, InstrumentApiClient>(client =>
			{
				var apiSettings = configuration.GetSection("APISettings").Get<APISettings>();
				client.BaseAddress = new Uri(apiSettings.BaseUrl);
			});
			services.AddHttpClient<ICalibrationApiClient, CalibrationApiClient>(client =>
			{
				var apiSettings = configuration.GetSection("APISettings").Get<APISettings>();
				client.BaseAddress = new Uri(apiSettings.BaseUrl);
			});
			services.AddHttpClient<ICalibrationFileApiClient, CalibrationFileApiClient>(client =>
			{
				var apiSettings = configuration.GetSection("APISettings").Get<APISettings>();
				client.BaseAddress = new Uri(apiSettings.BaseUrl);
			});
            services.AddHttpClient<ICustomerApiClient, CustomerApiClient>(client =>
            {
                var apiSettings = configuration.GetSection("APISettings").Get<APISettings>();
                client.BaseAddress = new Uri(apiSettings.BaseUrl);
            });
            services.AddHttpClient<IComplaintApiClient, ComplaintApiClient>(client =>
            {
                var apiSettings = configuration.GetSection("APISettings").Get<APISettings>();
                client.BaseAddress = new Uri(apiSettings.BaseUrl);
            });
            services.AddHttpClient<ICAPAApiClient, CAPAApiClient>(client =>
            {
                var apiSettings = configuration.GetSection("APISettings").Get<APISettings>();
                client.BaseAddress = new Uri(apiSettings.BaseUrl);
            });
            services.AddTransient<ApiAuthMessageHandler>();
            services.AddHttpClient<ICapaActionsApiClient, CapaActionApiClient>(client =>
            {
                var apiSettings = configuration.GetSection("APISettings").Get<APISettings>();
                client.BaseAddress = new Uri(apiSettings.BaseUrl);
            });
            services.AddHttpClient<ICapaActionFilesApiClient, CapaActionFilesApiClient>(client =>
            {
                var apiSettings = configuration.GetSection("APISettings").Get<APISettings>();
                client.BaseAddress = new Uri(apiSettings.BaseUrl);
            });
            services.AddTransient<ApiAuthMessageHandler>();
            return services;
        }
    }
}
