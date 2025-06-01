using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airline.Data.Models;

namespace Airline.Data.Repositories
{
    public interface IFlightRepository
    {
        Task<Flight> GetByIdAsync(int id);
        Task<IEnumerable<Flight>> GetAllAsync();
        Task<IEnumerable<Flight>> SearchAsync(string originCode, string destinationCode, DateTime date);
        Task AddAsync(Flight flight);
        Task UpdateAsync(Flight flight);
        Task DeleteAsync(int id);
    }
}
