using AutoMapper;
using Travel.Model;

namespace Travel.Api.DTOs;
public class TripProfile : Profile
{
    public TripProfile()
    {
        CreateMap<CreateTripDto, Trip>();
        CreateMap<CreateTripImageDto, TripImage>();
        CreateMap<PatchTripDto, Trip>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) =>
            {
                // Skip nulls
                if(srcMember == null) return false;

                // For DateTime properties, skip MinValue (if any slipped through)
                if(srcMember is DateTime dt && dt == default(DateTime))
                    return false;

                return true;
            }));
        
        CreateMap<PatchTripImageDto, TripImage>()
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember, destMember, context) =>
                {
                    if (srcMember == null)
                        return false;

                    if (srcMember is Guid guid && guid == Guid.Empty)
                        return false;

                    if (srcMember is DateTime dt && dt == DateTime.MinValue)
                        return false;

                    return true;
                }));
    }
}