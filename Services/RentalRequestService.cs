using EquipmentRental.Data;
using EquipmentRental.Models;
using Microsoft.EntityFrameworkCore;

namespace EquipmentRental.Services
{
    public class RentalRequestService : IRentalRequestService
    {
        private readonly ApplicationDbContext _db;

        public RentalRequestService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<RentalRequest>> GetAllAsync()
        {
            return await _db.RentalRequests
                .Include(r => r.User)
                .Include(r => r.RentalRequestItems)
                    .ThenInclude(ri => ri.EquipmentItem)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<RentalRequest>> GetByUserIdAsync(string userId)
        {
            return await _db.RentalRequests
                .Where(r => r.UserId == userId)
                .Include(r => r.User)
                .Include(r => r.RentalRequestItems)
                    .ThenInclude(ri => ri.EquipmentItem)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<RentalRequest?> GetByIdAsync(int id)
        {
            return await _db.RentalRequests
                .Include(r => r.User)
                .Include(r => r.RentalRequestItems)
                    .ThenInclude(ri => ri.EquipmentItem)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<RentalRequest> CreateAsync(RentalRequest request)
        {
            request.Status = "awaiting";
            // validate requested quantities against available stock
            if (request.StartDate.Date < DateTime.Today)
                throw new InvalidOperationException("Start date cannot be in the past");

            if (request.EndDate.Date < DateTime.Today)
                throw new InvalidOperationException("End date cannot be in the past");

            if (request.StartDate.Date > request.EndDate.Date)
                throw new InvalidOperationException("Start date must be before end date");

            foreach (var ri in request.RentalRequestItems)
            {
                var equipment = await _db.EquipmentItems.FindAsync(ri.EquipmentItemId);
                if (equipment == null)
                    throw new InvalidOperationException($"Equipment with id {ri.EquipmentItemId} not found");

                if (ri.Quantity <= 0)
                    throw new InvalidOperationException("Requested quantity must be greater than zero");

                if (ri.Quantity > equipment.AvailableQuantity)
                    throw new InvalidOperationException($"Requested quantity for {equipment.Name} exceeds available stock");
            }

            _db.RentalRequests.Add(request);
            await _db.SaveChangesAsync();
            return request;
        }

        public async Task UpdateStatusAsync(int id, string status)
        {
            var r = await _db.RentalRequests
                .Include(x => x.RentalRequestItems)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (r == null) return;
            var previous = r.Status?.ToLowerInvariant();
            var incoming = status?.ToLowerInvariant();

            // If changing to approved from non-approved, decrease stock
            if (incoming == "approved" && previous != "approved")
            {
                foreach (var ri in r.RentalRequestItems)
                {
                    var equipment = await _db.EquipmentItems.FindAsync(ri.EquipmentItemId);
                    if (equipment == null) continue;
                    equipment.AvailableQuantity = Math.Max(0, equipment.AvailableQuantity - ri.Quantity);
                    _db.EquipmentItems.Update(equipment);
                }
            }

            // If previously approved and now moving away from approved (e.g., rejected), restore stock
            if (previous == "approved" && incoming != "approved")
            {
                foreach (var ri in r.RentalRequestItems)
                {
                    var equipment = await _db.EquipmentItems.FindAsync(ri.EquipmentItemId);
                    if (equipment == null) continue;
                    equipment.AvailableQuantity = equipment.AvailableQuantity + ri.Quantity;
                    _db.EquipmentItems.Update(equipment);
                }
            }

            r.Status = status;
            _db.RentalRequests.Update(r);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var r = await _db.RentalRequests.FindAsync(id);
            if (r == null) return;
            _db.RentalRequests.Remove(r);
            await _db.SaveChangesAsync();
        }
    }
}