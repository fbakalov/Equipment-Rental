using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EquipmentRental.Models;

namespace EquipmentRental.Models
{
    public class RentalRequest
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime EndDate { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; } = string.Empty;

        public string Status { get; set; } = "awaiting";

        [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever]
        [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
        public string? UserId { get; set; }
        // Navigation property should be nullable to avoid model-validation requiring it during binding
        [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
        public ApplicationUser? User { get; set; }

        [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
        public ICollection<RentalRequestItem> RentalRequestItems { get; set; }
            = new List<RentalRequestItem>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var today = DateTime.Today;
            if (StartDate.Date < today)
                yield return new ValidationResult("Start date cannot be in the past", new[] { nameof(StartDate) });

            if (EndDate.Date < today)
                yield return new ValidationResult("End date cannot be in the past", new[] { nameof(EndDate) });

            if (StartDate.Date > EndDate.Date)
                yield return new ValidationResult("Start date must be before end date", new[] { nameof(StartDate), nameof(EndDate) });
        }
    }
}
