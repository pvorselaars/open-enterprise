using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using OpenEnterprise.UI;
using OpenEnterprise.Ontology;
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

var facts = new Facts();
var actor1 = new Actor(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), facts);
var actor2 = new Actor(Guid.Empty, facts);

builder.Services.AddSingleton<IEnumerable<Actor>>([actor1, actor2]);
builder.Services.AddSingleton(facts);
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
