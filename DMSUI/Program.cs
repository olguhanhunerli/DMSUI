using DMSUI.Business.Exceptions;
using DMSUI.Extensions;
using DMSUI.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
	{
		options.LoginPath = "/Auth/Login";
		options.AccessDeniedPath = "/Error/403";

		options.Events = new CookieAuthenticationEvents
		{
			OnValidatePrincipal = context =>
			{
				var token = context.HttpContext.Request.Cookies["access_token"];
				if (string.IsNullOrWhiteSpace(token))
					return Task.CompletedTask;

				var handler = new JwtSecurityTokenHandler();
				var jwt = handler.ReadJwtToken(token);
				var claims = jwt.Claims.ToList();

				var userId =
					claims.FirstOrDefault(x => x.Type == "userId")?.Value ??
					claims.FirstOrDefault(x => x.Type == "sub")?.Value;

				if (!string.IsNullOrEmpty(userId))
					claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));

				var identity = new ClaimsIdentity(
					claims,
					CookieAuthenticationDefaults.AuthenticationScheme
				);

				context.ReplacePrincipal(new ClaimsPrincipal(identity));
				return Task.CompletedTask;
			}
		};
	});

builder.Services.AddControllersWithViews(o =>
{
	o.Filters.Add<UiExceptionFilter>();
});

builder.Services.AddDmsServices(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler("/Error");

app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
