using System.ComponentModel.DataAnnotations;

namespace MU5PrototypeProject.Models
{
    public class SessionExercise
    {
        public int ID { get; set; }

        [Display(Name = "Exercise Name")]
        [Required(ErrorMessage = "You must provide the name of the exercise.")]
        [StringLength(100, ErrorMessage = "Exercise name cannot be more than 100 characters long.")]
        public string ExerciseName { get; set; }

        [Required(ErrorMessage = "Please specify the apparatus used.")]
        [StringLength(100, ErrorMessage = "Apparatus description cannot exceed 100 characters.")]
        public string Apparatus { get; set; }

        [Display(Name = "Spring Setting")]
        [StringLength(50, ErrorMessage = "Spring settings cannot exceed 50 characters.")]
        public string Springs { get; set; }

        [StringLength(100, ErrorMessage = "Props description cannot exceed 100 characters.")]
        public string Props { get; set; }

        public string Notes { get; set; }
    }
}
