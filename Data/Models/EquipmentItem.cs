using System.ComponentModel.DataAnnotations;

namespace EquipmentRental.Models
{
    public class EquipmentItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "Name cannot be longer than 64 characters")]
        public string Name { get; set; } = null!;          

        [Required]
        [StringLength(255, ErrorMessage = "Description cannot be longer than 255 characters")]
        public string Description { get; set; } = null!;   

        [Range(0, int.MaxValue)]
        public int AvailableQuantity { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; } = null!;

        [Required]
        [StringLength(32)]
        public string Condition { get; set; } = null!;     

        public ICollection<RentalRequestItem> RentalRequestItems { get; set; }
            = new List<RentalRequestItem>();
    }
}
