using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SeminarHub.Data.DataConstants;

namespace SeminarHub.Data.Models
{
    [Comment("Seminar data model")]
    public class Seminar
    {
        [Key]
        [Comment("Seminar identifier")]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.SeminarTopicMaxLength)]
        [Comment("Seminar topic")]
        public string Topic { get; set; } = string.Empty;

        [Required]
        [MaxLength(ValidationConstants.SeminarLecturerMaxLength)]
        [Comment("Seminar lecturer")]
        public string Lecturer { get; set; } = string.Empty;

        [Required]
        [MaxLength(ValidationConstants.SeminarDetailsMaxLength)]
        [Comment("Seminar details information")]
        public string Details { get; set; } = string.Empty;

        [Required]
        [Comment("Identifier of the seminar organizer")]
        public string OrganizerId { get; set; } = string.Empty;

        [Required]
        public IdentityUser Organizer { get; set; } = null!;

        [Required]
        [Comment("Date and time of the seminar")]
        public DateTime DateAndTime { get; set; }

        [Required]
        [MaxLength(180)]
        [Comment("Duration of the seminar")]
        public int Duration { get; set; }

        [Required]
        [ForeignKey(nameof(Category))]
        [Comment("Category identifier")]
        public int CategoryId { get; set; }

        [Required]
        public Category Category { get; set; } = null!;

        public ICollection<SeminarParticipant> SeminarsParticipants { get; set; } = new List<SeminarParticipant>();
    }
}
