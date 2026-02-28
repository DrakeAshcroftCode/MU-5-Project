using System.ComponentModel.DataAnnotations;

namespace MU5PrototypeProject.Models
{
    public enum HeadPadOption
    {
        Down,
        Middle,
        Full,
        [Display(Name = "1 Extra Cushion")]
        OneExtraCushion,
        [Display(Name = "2 Extra Cushion")]
        TwoExtraCushion,
        [Display(Name = "Posture Pillow")]
        PosturePillow
    }
    public enum StrapOrHandleOption
    {
        Straps = 1,
        Handles = 2
    }
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