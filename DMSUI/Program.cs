using DMSUI.Business.Exceptions;
using DMSUI.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.FileProviders;
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
                {
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));
                }

                var identity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                );

                context.ReplacePrincipal(new ClaimsPrincipal(identity));
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddControllersWithViews();

builder.Services.AddDmsServices(builder.Configuration);

var app = builder.Build();
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var feature = context.Features.Get<IExceptionHandlerPathFeature>();
        var ex = feature?.Error;

        if (ex is ForbiddenException)
        {
            context.Response.Redirect("/Error/403");
            return;
        }

        if (ex is UnauthorizedException)
        {
            context.Response.Redirect("/Auth/Login");
            return;
        }

        context.Response.Redirect("/Error");
    });
});
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(context =>
    {
        var feature = context.Features.Get<IExceptionHandlerPathFeature>();
        var ex = feature?.Error;

        if (ex is ForbiddenException)
        {
            context.Response.Redirect("/Error/403");
            return Task.CompletedTask;
        }

        if (ex is UnauthorizedException)
        {
            context.Response.Redirect("/Auth/Login");
            return Task.CompletedTask;
        }

        context.Response.Redirect("/Error");
        return Task.CompletedTask;
    });
});
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
