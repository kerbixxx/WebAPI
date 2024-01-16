using SimbirSoft.Models;

namespace SimbirSoft.Services.Interfaces
{
    public interface ILocationService
    {
        public bool IsValid(LocationRequest request);
        public bool IsConflict(LocationRequest request);
    }
}
