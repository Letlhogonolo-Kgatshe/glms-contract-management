using GLMS.POE.Shared.Models;
using GLMS.POE.Shared.Models.Enums;
using Microsoft.EntityFrameworkCore;
using GLMS.POE.Backend.Services.Implementations;
using GLMS.POE.Backend.Services.Interfaces;
using GLMS.POE.Backend.Data;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Services
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IServiceRequestService, ServiceRequestService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();

builder.Services.AddHttpClient<ICurrencyService, CurrencyService>();

builder.Services.AddScoped<IContractFactory, ContractFactory>();
builder.Services.AddScoped<IContractObserver, ContractObserver>();
builder.Services.AddScoped<IValidationStrategy, ActiveContractValidationStrategy>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Middleware order matters — this sequence is correct
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowBlazor");   

app.UseAuthorization();
app.MapControllers();

app.Run();
