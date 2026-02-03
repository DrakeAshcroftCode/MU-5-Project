using System.ComponentModel.DataAnnotations;

namespace MU5PrototypeProject.Models
{
    public class Session
    {

        public int ID { get; set; }

        [Display(Name = "Session Date")]
        [Required(ErrorMessage = "You must specify the date for the session.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime SessionDate { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        //NOTE:(kaiden) Check against data model
        //Do we need to keep these between different sessions?
        //Do we need multiple of these per session?

        //SessionNotes
        [Display(Name = "Health & Medical History")]
        [StringLength(2000, ErrorMessage = "History notes cannot exceed 2000 characters.")]
        public string HealthMedicalHistory { get; set; } //Replace with PDF attachment later I believe

        [Display(Name = "General Comments")]
        [StringLength(1000, ErrorMessage = "Comments cannot exceed 1000 characters.")]
        public string GeneralComments { get; set; }

        [Display(Name = "Subjective Report")]
        [StringLength(1500, ErrorMessage = "Subjective report cannot exceed 1500 characters.")]
        public string SubjectiveReport { get; set; }

        [Display(Name = "Objective Report")]
        [StringLength(1500, ErrorMessage = "Objective report cannot exceed 1500 characters.")]
        public string ObjectiveReport { get; set; }

        [Display(Name = "Session Plan")]
        [StringLength(1500, ErrorMessage = "The plan cannot exceed 1500 characters.")]
        public string Plan { get; set; } //Make more descriptive?


        public ICollection<SessionExercise> Exercises { get; set; } = new HashSet<SessionExercise>();

        [Display(Name = "Trainer")]
        [Required(ErrorMessage = "A session must be assigned to a trainer.")]
        public int TrainerID { get; set; }
        public Trainer? Trainer { get; set; }

        [Display(Name = "Client")]
        [Required(ErrorMessage = "A session must be assigned to a client.")]
        public int ClientID {  get; set; }
        public Client? Client { get; set; }

    }
}
