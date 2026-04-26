using EquipmentRental.Models;

namespace EquipmentRental.Services
{
    public interface IEquipmentService
    {
        Task<IEnumerable<EquipmentItem>> GetAllAsync();
        Task<IEnumerable<EquipmentItem>> SearchByNameAsync(string query);
        Task<EquipmentItem?> GetByIdAsync(int id);
        Task AddAsync(EquipmentItem item);
        Task UpdateAsync(EquipmentItem item);
        Task DeleteAsync(int id);
    }
}