using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airline.Data.Models;

namespace Airline.Data.Repositories
{
    public interface IAirportRepository
    {
        Task<Airport> GetByIdAsync(int id);
        Task<IEnumerable<Airport>> GetAllAsync();
        Task AddAsync(Airport airport);
        Task UpdateAsync(Airport airport);
        Task DeleteAsync(int id);
    }
}
