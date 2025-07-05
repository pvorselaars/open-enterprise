using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using OpenEnterprise.UI;
using OpenEnterprise.Ontology;
using OpenEnterprise.Models;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(443, listenOptions =>
    {
        listenOptions.UseHttps();
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
    });

});

builder.Services.AddSingleton<HashSet<Role>>([new("Alice"), new("Bob"), new("John")]);
builder.Services.AddSingleton<HashSet<ProductionFact>>([]);
builder.Services.AddSingleton<HashSet<CommunicationFact>>([]);
builder.Services.AddSingleton<ProcessModel>();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

var app = builder.Build();

app.UseHsts();
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
