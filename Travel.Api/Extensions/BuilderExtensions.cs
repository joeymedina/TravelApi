using Travel.Api.Endpoints;

namespace Travel.Api;

public static class BuilderExtensions
{
    public static IApplicationBuilder MapTripEndpoints(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.MapGet("api/trips", TripHandlers.GetTrips)
            .WithName("GetTrips")
            .WithOpenApi();

        app.MapGet("api/trips/{id}", TripHandlers.GetTrip)
            .WithName("GetTrip")
            .WithOpenApi();

        app.MapPost("api/trips", TripHandlers.PostTrip)
            .WithName("CreateTrip")
            .WithOpenApi();
        
        // app.MapPut("api/trips/{id}", TripHandlers.PutTrip)
        //     .WithName("UpdateTrip")
        //     .WithOpenApi();
        
        app.MapPatch("api/trips/{id}", TripHandlers.PatchTrip)
            .WithName("PatchTrip")
            .WithOpenApi();
        
        app.MapDelete("api/trips/{id}", TripHandlers.DeleteTrip)
            .WithName("DeleteTrip")
            .WithOpenApi();
        
        // app.MapGet("/weatherforecast", TripHandlers.GetWindow)
        //     .WithName("GetWeatherForecast")
        //     .WithOpenApi();
        return app;
    }

    public static IApplicationBuilder MapImageEndpoints(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);
        
        app.MapGet("api/trips/{id}/images", ImageHandlers.GetImages)
            .WithName("GetImages")
            .WithOpenApi();
        
        app.MapGet("api/trips/{tripId}/images/{id}", ImageHandlers.GetImage)
            .WithName("GetImage")
            .WithOpenApi();
        
        app.MapPost("api/trip/{tripId}/images", ImageHandlers.PostImage)
            .WithName("PostImage")
            .WithOpenApi();
        
        app.MapPatch("api/trips/{tripId}/images/{id}", ImageHandlers.PatchImage)
            .WithName("PatchImage")
            .WithOpenApi();
        
        app.MapDelete("api/trips/{tripId}/images/{id}", ImageHandlers.DeleteTripImage)
            .WithName("DeleteTripImage")
            .WithOpenApi();
        return app;
    }
}