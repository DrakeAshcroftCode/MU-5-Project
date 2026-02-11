using System.ComponentModel.DataAnnotations;

namespace MU5PrototypeProject.Models
{
    public class Session : IValidatableObject
    {
        public int ID { get; set; }

        [Display(Name = "Session Date")]
        [Required(ErrorMessage = "You must specify the date for the session.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime SessionDate { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Sessions/Week")]
        public int? SessionsPerWeekRecommended { get; set; }

        [Display(Name = "Archived")]
        public bool IsArchived { get; set; }

        // Navigation
        public ICollection<SessionExercise> Exercises { get; set; } = new HashSet<SessionExercise>();
        public SessionNotes? Notes { get; set; }
        public AdminStatus? AdminStatus { get; set; }
        public Equipment? Equipment { get; set; }

        [Display(Name = "Trainer")]
        [Required(ErrorMessage = "A session must be assigned to a trainer.")]
        public int TrainerID { get; set; }
        public Trainer? Trainer { get; set; }

        [Display(Name = "Client")]
        [Required(ErrorMessage = "A session must be assigned to a client.")]
        public int ClientID { get; set; }
        public Client? Client { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SessionDate <= DateTime.Today)
            {
                yield return new ValidationResult("Session cannot be in the future.", new[] { "SessionDate" });
            }

        }

    }
}
