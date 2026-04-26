using EquipmentRental.Models;

namespace EquipmentRental.Services
{
    public interface IRentalRequestService
    {
        Task<IEnumerable<RentalRequest>> GetAllAsync();
        Task<IEnumerable<RentalRequest>> GetByUserIdAsync(string userId);
        Task<RentalRequest?> GetByIdAsync(int id);
        Task<RentalRequest> CreateAsync(RentalRequest request);
        Task UpdateStatusAsync(int id, string status);
        Task DeleteAsync(int id);
    }
}