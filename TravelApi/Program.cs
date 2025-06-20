using Minio;
using TravelApi;
using TravelApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var endpoint = @"";
var accessKey = "";
var secretKey = "";

builder.Services.AddMinio(configureClient => configureClient
    .WithEndpoint(endpoint)
    .WithCredentials(accessKey, secretKey)
    .Build());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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