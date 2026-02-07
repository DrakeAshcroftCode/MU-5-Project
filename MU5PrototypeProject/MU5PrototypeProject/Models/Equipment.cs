using System.ComponentModel.DataAnnotations;

namespace MU5PrototypeProject.Models
{
    public class Equipment
    {
        public int ID { get; set; }

        [Required]
        public int SessionID { get; set; }
        public Session Session { get; set; } = null!;

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

        [Display(Name = "Has Towel")]
        public bool HasTowel { get; set; }

        [Display(Name = "Has Posture Pillow")]
        public bool HasPosturePillow { get; set; }

        [Display(Name = "Has Head Pad")]
        public bool HasHeadPad { get; set; }

        [Display(Name = "Has Rubber Pads")]
        public bool HasRubberPads { get; set; }
    }
}