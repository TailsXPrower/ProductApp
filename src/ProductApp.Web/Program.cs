using ProductApp.Web.Components;
using ProductApp.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Telemetry, Health Checks, etc.
builder.AddServiceDefaults();

// HttpClient with service discovery
builder.Services
    .AddHttpClient<ProductApiClient>(client =>
    {
        client.BaseAddress = new Uri("http://api");
    })
    .AddServiceDiscovery();

// Razor Components with interactive server-side rendering
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

app.MapDefaultEndpoints();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
