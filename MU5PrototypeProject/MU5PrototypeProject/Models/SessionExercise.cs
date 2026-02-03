using System.ComponentModel.DataAnnotations;

namespace MU5PrototypeProject.Models
{
    public class SessionExercise
    {
        public int ID { get; set; }

        [Required]
        public int SessionID { get; set; }
        public Session Session { get; set; } = null!;

        [Required]
        public int ExerciseID { get; set; }
        public Exercise Exercise { get; set; } = null!;

        [Display(Name = "Spring Setting")]
        [StringLength(50, ErrorMessage = "Spring settings cannot exceed 50 characters.")]
        public string? Springs { get; set; }

        [StringLength(100, ErrorMessage = "Props description cannot exceed 100 characters.")]
        public string? Props { get; set; }

        [DataType(DataType.MultilineText)]
        public string? Notes { get; set; }
    }
}
