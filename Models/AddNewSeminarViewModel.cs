using SeminarHub.Data.DataConstants;
using System.ComponentModel.DataAnnotations;

namespace SeminarHub.Models
{
    public class AddNewSeminarViewModel
    {

        [Required]
        [StringLength(ValidationConstants.SeminarTopicMaxLength,
            MinimumLength = ValidationConstants.SeminarTopicMinLength,
            ErrorMessage = ValidationConstants.LengthErrorMessage)]
        public string Topic { get; set; } = string.Empty;

        [Required]
        [StringLength(ValidationConstants.SeminarLecturerMaxLength,
            MinimumLength = ValidationConstants.SeminarLecturerMinLength,
            ErrorMessage = ValidationConstants.LengthErrorMessage)]
        public string Lecturer { get; set; } = string.Empty;

        [Required]
        [StringLength(ValidationConstants.SeminarDetailsMaxLength,
            MinimumLength = ValidationConstants.SeminarDetailsMinLength,
            ErrorMessage = ValidationConstants.LengthErrorMessage)]
        public string Details { get; set; } = string.Empty;


        [Required] 
        public string DateAndTime { get; set; } = string.Empty;

        [Required]
        [Range(ValidationConstants.SeminarDurationMinLength, 
            ValidationConstants.SeminarDurationMaxLength)]
        public int Duration { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public ICollection<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
    }
}
