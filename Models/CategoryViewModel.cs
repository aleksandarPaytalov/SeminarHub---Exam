using SeminarHub.Data.DataConstants;
using System.ComponentModel.DataAnnotations;

namespace SeminarHub.Models
{
    public class CategoryViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(ValidationConstants.CategoryNameMaxLength,
            MinimumLength = ValidationConstants.CategoryNameMinLength,
            ErrorMessage = ValidationConstants.LengthErrorMessage)]
        public string Name { get; set; } = string.Empty;
    }
}
