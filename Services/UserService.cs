using EquipmentRental.Data;
using EquipmentRental.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EquipmentRental.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;

        public UserService(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            return await _db.Users.AsNoTracking().ToListAsync();
        }

        public async Task<ApplicationUser?> GetByIdAsync(string id)
        {
            return await _db.Users.FindAsync(id);
        }

        public async Task CreateAsync(ApplicationUser user, string password)
        {
            await _userManager.CreateAsync(user, password);
            await _userManager.AddToRoleAsync(user, "User");
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            var existing = await _userManager.FindByIdAsync(user.Id);
            if (existing == null)
                throw new InvalidOperationException("User not found");

            existing.UserName = user.UserName;
            existing.Email = user.Email;
            existing.FirstName = user.FirstName;
            existing.LastName = user.LastName;

            var result = await _userManager.UpdateAsync(existing);
            if (!result.Succeeded)
            {
                var msg = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException(msg);
            }
        }

        public async Task DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return;

            if (await _userManager.IsInRoleAsync(user, "Administrator"))
                throw new InvalidOperationException("Cannot delete an administrator");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var msg = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException(msg);
            }
        }
    }
}