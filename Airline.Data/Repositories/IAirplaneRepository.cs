using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airline.Data.Models;

namespace Airline.Data.Repositories
{
    public interface IAirplaneRepository
    {
        Task<Airplane> GetByIdAsync(int id);
        Task<IEnumerable<Airplane>> GetAllAsync();
        Task AddAsync(Airplane airplane);
        Task UpdateAsync(Airplane airplane);
        Task DeleteAsync(int id);
    }
}
