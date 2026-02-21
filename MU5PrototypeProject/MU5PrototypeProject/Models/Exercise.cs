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

        [Display(Name = "Apparatus")]
        public int? ApparatusID { get; set; }
        public Apparatus? Apparatus { get; set; }

        public ICollection<SessionExercise> SessionExercises { get; set; } = new HashSet<SessionExercise>();
    }
}
