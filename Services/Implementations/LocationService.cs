using SimbirSoft.Models;
using SimbirSoft.Repositories.Interfaces;
using SimbirSoft.Services.Interfaces;

namespace SimbirSoft.Services.Implementations
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepo;

        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepo = locationRepository;
        }

        public bool IsConflict(LocationRequest request)
        {
            return _locationRepo.FirstOrDefault(x => x.latitude == request.latitude && x.longitude == request.longitude) != null;
        }

        public bool IsValid(LocationRequest request)
        {
            return !(request.latitude <-90 || request.latitude > 90 || 
                request.longitude <-180 || request.longitude > 180);
        }
    }
}
