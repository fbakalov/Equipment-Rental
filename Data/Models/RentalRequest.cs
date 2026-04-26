using EquipmentRental.Models;

namespace EquipmentRental.Data
{
    public class RentalRequest
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Comment { get; set; } = null!;
        public string Status { get; set; }

        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        public ICollection<RentalRequestItem> RentalRequestItems { get; set; }
            = new List<RentalRequestItem>();
    }
}
