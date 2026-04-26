using EquipmentRental.Models;

namespace EquipmentRental.Services
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        Task<ApplicationUser?> GetByIdAsync(string id);
        Task CreateAsync(ApplicationUser user, string password);
        Task UpdateAsync(ApplicationUser user);
        Task DeleteAsync(string id);
    }
}