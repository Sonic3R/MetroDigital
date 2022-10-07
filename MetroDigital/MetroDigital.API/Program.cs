using MetroDigital.API;
using MetroDigital.API.Swagger;
using MetroDigital.Application.Extensions;
using MetroDigital.Infrastructure;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
    opts.OperationFilter<SwaggerOperationFilter>();
});

builder.Services.AddApplication().AddInfrastructure(builder.Configuration);
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = false;
    options.SerializerOptions.PropertyNamingPolicy = null;
    options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault | System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    options.SerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.Strict;
});

builder.MetroDigitalDBInitializer(builder.Configuration);

var app = builder.Build();
app.SetupEndpoints();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.Run();