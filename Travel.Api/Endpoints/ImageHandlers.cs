using Microsoft.AspNetCore.Http.HttpResults;
using Travel.Model;

namespace Travel.Api.Endpoints;

internal static class ImageHandlers
{
    private static List<TripImage> _tripImages =
    [
        new TripImage
        {
            Id = Guid.Parse("10AAC809-C188-458D-A12E-1AFC92E2FEE3"),
            Url = "http://google.com/myimage.png",
            Caption = "This is my image",
            TripId = Guid.Parse("BB68B434-3026-41E1-B253-97BA308D764F")
        },
        new TripImage
        {
            Id = Guid.Parse("10AAC809-C188-458D-A12E-1AFC92E2FEE2"),
            Url = "http://google.com/myimage2.png",
            Caption = "This is my second image",
            TripId = Guid.Parse("BB68B434-3026-41E1-B253-97BA308D764F")
        },
        new TripImage
        {
            Id = Guid.Parse("9AD58C86-AF81-4B97-BEA5-0218E5DC9E9B"),
            Url = "http://google.com/myimage3.png",
            Caption = "This is my third image",
            TripId = Guid.Parse("2358D025-1796-4A99-B527-D59120930E25")
        }
    ];

    public static List<TripImage> GetImages(string id)
    {
        var result = _tripImages.Where(x => x.TripId == Guid.Parse(id)).ToList();
        return result;
    }

    public static TripImage? GetImage(string tripId, string id)
    {
        return  _tripImages.FirstOrDefault(x => x.TripId == Guid.Parse(tripId) && x.Id == Guid.Parse(id));
    }

    public static Created<TripImage> PostImage(TripImage tripImage)
    {
        _tripImages.Add(tripImage);
        return TypedResults.Created("", tripImage);
    }
    public static Created<TripImage> PutImage(string tripId, string id, TripImage tripImage)
    {
        DeleteTripImage(tripId, id);
        _tripImages.Add(tripImage);
        return TypedResults.Created("",tripImage);
    }
    
    public static void DeleteTripImage(string tripId, string id)
    {
        var image = GetImage(tripId, id);
        if (image != null)
        {
            _tripImages.Remove(image);
        }
    }
}