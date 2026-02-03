using System.ComponentModel.DataAnnotations;

namespace MU5PrototypeProject.Models
{
    public class Exercise
    {
        public int ID { get; set; }

        [Display(Name = "Exercise Name")]
        [Required]
        [StringLength(100)]
        public string ExerciseName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Apparatus { get; set; } = string.Empty;

        public ICollection<SessionExercise> SessionExercises { get; set; } = new HashSet<SessionExercise>();
    }
}
