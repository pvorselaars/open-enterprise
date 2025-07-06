using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using OpenEnterprise.UI;
using OpenEnterprise.Ontology;
using OpenEnterprise.Models;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(443, listenOptions =>
    {
        listenOptions.UseHttps();
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
    });

});

builder.Services.AddAuthentication(options => {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie()
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = builder.Configuration["Authentication:OIDC:Authority"];
                    options.ClientId = builder.Configuration["Authentication:OIDC:ClientId"];
                    options.ClientSecret = builder.Configuration["Authentication:OIDC:ClientSecret"];
                    options.CallbackPath = builder.Configuration["Authentication:OIDC:CallbackPath"];

                    options.ResponseType = OpenIdConnectResponseType.Code;
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                    options.SaveTokens = true;

                    options.ClaimActions.MapJsonKey("name", "name");
                    options.TokenValidationParameters.NameClaimType = "name";

                    var scopes = builder.Configuration.GetSection("Authentication:OIDC:Scope").Get<string[]>();
                    if (scopes != null)
                    {
                        foreach (var scope in scopes)
                        {
                            options.Scope.Add(scope);
                        }
                    }

                });

builder.Services.AddAuthorizationBuilder()
    .SetFallbackPolicy(new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build());

builder.Services.AddSingleton<HashSet<Role>>([new("Alice"), new("Bob"), new("John")]);
builder.Services.AddSingleton<HashSet<ProductionFact>>([]);
builder.Services.AddSingleton<HashSet<CommunicationFact>>([]);
builder.Services.AddSingleton<ProcessModel>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler("/error");
app.UseHsts();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
