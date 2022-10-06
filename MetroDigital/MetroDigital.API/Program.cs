using MetroDigital.API;
using MetroDigital.Application.Extensions;
using MetroDigital.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication().AddInfrastructure(builder.Configuration);

builder.MetroDigitalDBInitializer(builder.Configuration);

var app = builder.Build();
app.SetupEndpoints();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.Run();