using Microsoft.AspNetCore.Http.Features;
using Travel.Api;
using Travel.Api.DTOs;
using Travel.Api.Middleware;
using Travel.Domain.Extensions;
using Travel.Infrastructure.Context;
using Travel.Infrastructure.Extensions;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Travel.Application.Extensions;
using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddAutoMapper(typeof(TripProfile));
builder.Services.AddDomain();
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 268435456; // 256 MB limit
});
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowLocalhost",
//         builder =>
//         {
//             builder.WithOrigins("*") // or "*", if you want to allow any origin (not recommended in prod)
//                 .AllowAnyMethod()
//                 .AllowAnyHeader();
//         });
// });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
var app = builder.Build();

var migrateAndSeed = true;
if (migrateAndSeed)
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<TripsDbContext>();
        await context.Database.MigrateAsync();
        await TripsSeeder.SeedAsync(context);
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseCors("AllowLocalhost");
app.UseCors("AllowFrontend");
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.MapTripEndpoints();
app.MapImageEndpoints();

app.Run();

public partial class Program {}