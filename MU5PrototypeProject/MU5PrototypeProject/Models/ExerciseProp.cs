using System.ComponentModel.DataAnnotations;

namespace MU5PrototypeProject.Models
{
    public class ExerciseProp
    {
        public int ID { get; set; }

        [Required]
        public int ExerciseID { get; set; }
        public Exercise Exercise { get; set; } = null!;

        [Required]
        public int PropID { get; set; }
        public Prop Prop { get; set; } = null!;
    }
}