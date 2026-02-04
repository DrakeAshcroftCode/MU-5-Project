using System.ComponentModel.DataAnnotations;

namespace MU5PrototypeProject.Models
{
    public class AdminStatus//cool
    {
        public int ID { get; set; }

        [Required]
        public int SessionID { get; set; }
        public Session Session { get; set; } = null!;

        [Display(Name = "Paid")]
        public bool IsPaid { get; set; }

        [Display(Name = "Admin Notes")]
        [DataType(DataType.MultilineText)]
        public string? AdminNotes { get; set; }

        [Display(Name = "Initials")]
        [StringLength(10)]
        public string? AdminInitials { get; set; }

        [Display(Name = "Next Appointment Booked")]
        public bool NextAppointmentBooked { get; set; }

        [Display(Name = "Communicated Progress")]
        public bool CommunicatedProgress { get; set; }

        [Display(Name = "Ready to Progress")]
        public bool ReadyToProgress { get; set; }

        [Display(Name = "Course Correction Needed")]
        public bool CourseCorrectionNeeded { get; set; }

        [Display(Name = "Team Consult")]
        public bool TeamConsult { get; set; }

        [Display(Name = "Referred Externally")]
        public bool ReferredExternally { get; set; }

        [Display(Name = "Referred To")]
        [StringLength(100, ErrorMessage = "Referred To cannot exceed 100 characters.")]
        public string? ReferredTo { get; set; }
    }
}
