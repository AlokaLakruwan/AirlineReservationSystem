using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Airline.Data.Models;

namespace Airline.Data.Repositories
{
    public interface IFlightRepository
    {
        Task<IEnumerable<Flight>> GetAllAsync();
        Task<Flight> GetByIdAsync(int id);
        Task<IEnumerable<Flight>> SearchAsync(int originAirportId, int destinationAirportId, DateTime date);
        Task AddAsync(Flight flight);
        Task UpdateAsync(Flight flight);
        Task DeleteAsync(int id);
    }
}
