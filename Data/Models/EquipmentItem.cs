namespace EquipmentRental.Models
{
    public class EquipmentItem
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;          // до 64 символа
        public string Description { get; set; } = null!;   // до 255 символа
        public int AvailableQuantity { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string Condition { get; set; } = null!;     // "ново", "използвано", "за ремонт"

        public ICollection<RentalRequestItem> RentalRequestItems { get; set; }
            = new List<RentalRequestItem>();
    }
}
