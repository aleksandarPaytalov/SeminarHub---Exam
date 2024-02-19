using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using SeminarHub.Data.DataConstants;

namespace SeminarHub.Data.Models
{
    [Comment("category data model")]
    public class Category
    {
        [Key]
        [Comment("Category identifier")]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.CategoryNameMaxLength)]
        [Comment("Category name")]
        public string Name { get; set; } = string.Empty;

        public ICollection<Seminar> Seminars { get; set; } = new List<Seminar>();
    }
}
