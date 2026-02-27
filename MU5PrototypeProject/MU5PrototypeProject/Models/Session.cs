using System.ComponentModel.DataAnnotations;

namespace MU5PrototypeProject.Models
{
    public enum SessionType
    {
        Private,
        SemiPrivate,
        Physio
    }

    public class Session : Auditable, IValidatableObject
    {
        public int ID { get; set; }

        [Display(Name = "Session Date")]
        [Required(ErrorMessage = "You must specify the date for the session.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime SessionDate { get; set; }

        [Display(Name = "Session Type")]
        [Required(ErrorMessage = "You must select a session type.")]
        public SessionType SessionType { get; set; }

        [Display(Name = "Sessions/Week")]
        public int? SessionsPerWeekRecommended { get; set; }

        [Display(Name = "Archived")]
        public bool IsArchived { get; set; }

        // Navigation
        public ICollection<SessionExercise> Exercises { get; set; } = new HashSet<SessionExercise>();
        public SessionNotes? SessionNotes { get; set; }
        public AdminStatus? AdminStatus { get; set; }
        public PhysioInfo? PhysioInfo { get; set; }

        [Display(Name = "Trainer")]
        [Required(ErrorMessage = "You must select a trainer to add to the session.")]
        public int TrainerID { get; set; }
        public Trainer? Trainer { get; set; }

        [Display(Name = "Client")]
        [Required(ErrorMessage = "You must select a client to add to the session.")]
        public int ClientID { get; set; }
        public Client? Client { get; set; }



        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SessionDate < DateTime.Today)
                yield return new ValidationResult("Session cannot be in the past.", new[] { "SessionDate" });
        }
    }
}
