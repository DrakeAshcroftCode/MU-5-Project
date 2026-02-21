using System.ComponentModel.DataAnnotations;

namespace MU5PrototypeProject.Models
{
    public class PhysioInfo
    {
        public int ID { get; set; }

        [Required]
        public int SessionID { get; set; }
        public Session Session { get; set; } = null!;

        [Display(Name = "Insurance Company")]
        [StringLength(100)]
        public string? InsuranceCompany { get; set; }

        [Display(Name = "Coverage Amount/Year")]
        [DataType(DataType.Currency)]
        public decimal? CoverageAmountPerYear { get; set; }

        [Display(Name = "Amount Used")]
        [DataType(DataType.Currency)]
        public decimal? AmountUsed { get; set; }

        [Display(Name = "Coverage Resets Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CoverageResetsDate { get; set; }

        [Display(Name = "Physiotherapist Name")]
        [StringLength(100)]
        public string? PhysiotherapistName { get; set; }

        [Display(Name = "Coverage Shared")]
        public bool CoverageShared { get; set; }

        [Display(Name = "Communicated with Physio")]
        public bool CommunicatedWithPhysio { get; set; }
    }
}