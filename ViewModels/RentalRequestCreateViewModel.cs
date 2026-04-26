using EquipmentRental.Models;
using System.Collections.Generic;

namespace EquipmentRental.ViewModels
{
    public class RentalRequestCreateViewModel
    {
        public RentalRequest Request { get; set; } = new RentalRequest();
        public IEnumerable<EquipmentItem> Items { get; set; } = new List<EquipmentItem>();
    }
}