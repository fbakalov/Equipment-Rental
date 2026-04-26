using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EquipmentRental.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(64, ErrorMessage = "First name cannot be longer than 64 characters")]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(64, ErrorMessage = "Last name cannot be longer than 64 characters")]
        public string LastName { get; set; } = null!;
    }
}

