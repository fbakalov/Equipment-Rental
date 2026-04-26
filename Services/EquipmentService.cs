using EquipmentRental.Data;
using EquipmentRental.Models;
using Microsoft.EntityFrameworkCore;

namespace EquipmentRental.Services
{
    public class EquipmentService : IEquipmentService
    {
        private readonly ApplicationDbContext _db;

        public EquipmentService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<EquipmentItem>> GetAllAsync()
        {
            return await _db.EquipmentItems.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<EquipmentItem>> SearchByNameAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return await GetAllAsync();

            return await _db.EquipmentItems
                .Where(e => EF.Functions.Like(e.Name, $"%{query}%"))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<EquipmentItem?> GetByIdAsync(int id)
        {
            return await _db.EquipmentItems.FindAsync(id);
        }

        public async Task AddAsync(EquipmentItem item)
        {
            _db.EquipmentItems.Add(item);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(EquipmentItem item)
        {
            _db.EquipmentItems.Update(item);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _db.EquipmentItems.FindAsync(id);
            if (item == null) return;
            _db.EquipmentItems.Remove(item);
            await _db.SaveChangesAsync();
        }
    }
}