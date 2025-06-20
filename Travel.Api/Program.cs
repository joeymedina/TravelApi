using Minio;
using Travel.Api;
using Travel.Api.Endpoints;
using Travel.Infrastructure;
using Travel.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapTripEndpoints();
app.MapImageEndpoints();

app.Run();