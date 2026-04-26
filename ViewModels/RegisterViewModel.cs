using System.ComponentModel.DataAnnotations;

namespace EquipmentRental.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(64, ErrorMessage = "Username cannot be longer than 64 characters")]
        public string UserName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(64, ErrorMessage = "First name cannot be longer than 64 characters")]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(64, ErrorMessage = "Last name cannot be longer than 64 characters")]
        public string LastName { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = null!;
    }
}