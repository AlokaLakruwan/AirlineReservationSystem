using System.Collections.Generic;
using System.Threading.Tasks;
using Airline.Data.Models;

namespace Airline.Data.Repositories
{
    public interface IAirplaneLocationRepository
    {
        Task<IEnumerable<AirplaneLocation>> GetAllAsync();
        Task<AirplaneLocation> GetByAirplaneIdAsync(int airplaneId);
        Task AddAsync(AirplaneLocation location);
        Task UpdateAsync(AirplaneLocation location);
        Task DeleteAsync(int airplaneId);
    }
}
