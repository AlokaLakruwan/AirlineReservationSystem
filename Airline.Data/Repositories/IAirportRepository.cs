using System.Collections.Generic;
using System.Threading.Tasks;
using Airline.Data.Models;

namespace Airline.Data.Repositories
{
    public interface IAirportRepository
    {
        Task<IEnumerable<Airport>> GetAllAsync();
        Task<Airport> GetByIdAsync(int id);
        Task<Airport> GetByCodeAsync(string code);
        Task AddAsync(Airport airport);
        Task UpdateAsync(Airport airport);
        Task DeleteAsync(int id);
    }
}
