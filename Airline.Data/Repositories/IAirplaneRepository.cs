using System.Collections.Generic;
using System.Threading.Tasks;
using Airline.Data.Models;

namespace Airline.Data.Repositories
{
    public interface IAirplaneRepository
    {
        Task<IEnumerable<Airplane>> GetAllAsync();
        Task<Airplane> GetByIdAsync(int id);
        Task AddAsync(Airplane airplane);
        Task UpdateAsync(Airplane airplane);
        Task DeleteAsync(int id);
    }
}
