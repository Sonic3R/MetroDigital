using MetroDigital.API;
using MetroDigital.API.Swagger;
using MetroDigital.Application.Extensions;
using MetroDigital.Infrastructure;
using Microsoft.AspNetCore.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
    opts.OperationFilter<SwaggerOperationFilter>();
});

builder.Services.AddApplication().AddInfrastructure(builder.Configuration);
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.WriteIndented = builder.Environment.IsDevelopment();
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault | JsonIgnoreCondition.WhenWritingNull;
    options.SerializerOptions.NumberHandling = JsonNumberHandling.Strict;
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.MetroDigitalDBInitializer(builder.Configuration);

var app = builder.Build();
app.SetupEndpoints();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.Run();

/// <summary>
/// For UT purpose
/// </summary>
public partial class Program { }