using System.ComponentModel.DataAnnotations.Schema;

namespace EquipmentRental.Models
{
    public class RentalRequestItem
    {
        public int Id { get; set; }

        public int RentalRequestId { get; set; }
        [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
        public RentalRequest? RentalRequest { get; set; }

        public int EquipmentItemId { get; set; }

        [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
        public EquipmentItem? EquipmentItem { get; set; }

        public int Quantity { get; set; }
    }
}
