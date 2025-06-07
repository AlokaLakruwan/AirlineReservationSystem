using System.Collections.Generic;
using System.Threading.Tasks;
using Airline.Data.Models;

namespace Airline.Data.Repositories
{
    public interface IBookingRepository
    {
        Task<Booking> GetByIdAsync(int id);
        Task<IEnumerable<Booking>> GetAllAsync();
        Task<IEnumerable<Booking>> GetByUserIdAsync(int userId);
        Task AddAsync(Booking booking);
        Task UpdateAsync(Booking booking);
        Task DeleteAsync(int id);
    }

}
