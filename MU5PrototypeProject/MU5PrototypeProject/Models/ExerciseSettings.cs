using System.ComponentModel.DataAnnotations;

namespace MU5PrototypeProject.Models
{
    public class ExerciseSettings
    {
        public int ID { get; set; }

        [Required]
        public int ExerciseID { get; set; }
        public Exercise Exercise { get; set; } = null!;

        [Display(Name = "Gear Bar Level")]
        public int? GearBarLevel { get; set; }

        [Display(Name = "Stopper Setting")]
        public int? StopperSetting { get; set; }

        [Display(Name = "Headrest Position")]
        [StringLength(50)]
        public string? HeadrestPosition { get; set; }

        [Display(Name = "Straps Type")]
        [StringLength(50)]
        public string? StrapsType { get; set; }
    }
}