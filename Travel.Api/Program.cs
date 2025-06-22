using Travel.Api;
using Travel.Api.DTOs;
using Travel.Api.Middleware;
using Travel.Domain.Extensions;
using Travel.Infrastructure.Context;
using Travel.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddDomain();
builder.Services.AddAutoMapper(typeof(TripProfile));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TripsDbContext>();
    await TripsSeeder.SeedAsync(context);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.MapTripEndpoints();
app.MapImageEndpoints();

app.Run();