using GLMS.POE.Frontend.Components;
using GLMS.POE.Frontend.Services.Api;
using GLMS.POE.Frontend.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configurable API URL
var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:5285/";
var apiBase = new Uri(apiBaseUrl);

builder.Services.AddHttpClient<ContractApiService>(c => c.BaseAddress = apiBase);
builder.Services.AddHttpClient<ServiceRequestApiService>(c => c.BaseAddress = apiBase);
builder.Services.AddHttpClient<ClientApiService>(c => c.BaseAddress = apiBase);
builder.Services.AddHttpClient<AuditLogApiService>(c => c.BaseAddress = apiBase);
builder.Services.AddHttpClient<FileApiService>(c => c.BaseAddress = apiBase);

builder.Services.AddScoped<IServiceRequestService>(sp => sp.GetRequiredService<ServiceRequestApiService>());

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
