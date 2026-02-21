using System.ComponentModel.DataAnnotations;

namespace MU5PrototypeProject.Models
{
    public class SessionNotes
    {
        public int ID { get; set; }

        [Required]
        public int SessionID { get; set; }
        public Session Session { get; set; } = null!;

        [Display(Name = "Completed By")]
        public int? CompletedByTrainerID { get; set; }
        public Trainer? CompletedByTrainer { get; set; }

        [Display(Name = "Goals")]
        [DataType(DataType.MultilineText)]
        public string? Goals { get; set; }

        [Display(Name = "Health / Medical History")]
        [DataType(DataType.MultilineText)]
        public string? HealthMedicalHistory { get; set; }

        [Display(Name = "General Comments")]
        [DataType(DataType.MultilineText)]
        public string? GeneralComments { get; set; }

        [Display(Name = "Subjective Reports")]
        [DataType(DataType.MultilineText)]
        public string? SubjectiveReports { get; set; }

        [Display(Name = "Objective Findings")]
        [DataType(DataType.MultilineText)]
        public string? ObjectiveFindings { get; set; }

        [Display(Name = "Plan")]
        [DataType(DataType.MultilineText)]
        public string? Plan { get; set; }
    }
}
