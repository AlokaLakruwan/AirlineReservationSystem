using System.Collections.Generic;
using System.Threading.Tasks;
using Airline.Data.Models;

namespace Airline.Data.Repositories
{
    public interface IAirplaneConfigurationRepository
    {
        Task<IEnumerable<AirplaneConfiguration>> GetAllAsync();
        Task<AirplaneConfiguration> GetByIdAsync(int id);
        Task AddAsync(AirplaneConfiguration config);
        Task UpdateAsync(AirplaneConfiguration config);
        Task DeleteAsync(int id);
    }
}
