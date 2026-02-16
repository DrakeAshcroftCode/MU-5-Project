using System.ComponentModel.DataAnnotations;

namespace MU5PrototypeProject.Models
{
    public class Client : Auditable, IValidatableObject
    {
        public int ID { get; set; }

        //Summary Properties
        [Display(Name = "Client Name")]
        public string ClientName
        {
            get
            {
                return FirstName + " " + (string.IsNullOrEmpty(LastName) ? "" :
                        (" " + (char?)LastName[0]).ToUpper());

            }
        }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;

            }
        }
        public int? Age
        {
            get
            {
                if (DOB == null) { return null; }
                DateTime today = DateTime.Today;
                int? a = today.Year - DOB?.Year
                    - ((today.Month < DOB?.Month ||
                        (today.Month == DOB?.Month && today.Day < DOB?.Day) ? 1 : 0));
                return a;
            }
        }

        [Display(Name = "Age (DOB)")]
        public string AgeSummary => (DOB == null) ? "Unknown" : Age + " (" + DOB.GetValueOrDefault().ToString("yyyy-MM-dd") + ")";

        [Display(Name = "Phone")]
        public string PhoneFormatted => "(" + Phone?.Substring(0, 3) + ") "
           + Phone?.Substring(3, 3) + "-" + Phone?[6..];


        [Display(Name = "First Name")]
        [Required(ErrorMessage = "You cannot leave the first name blank.")]
        [StringLength(50, ErrorMessage = "First name cannot be more than 50 characters long.")]
        [RegularExpression(@"^[A-Za-z-]+$", ErrorMessage = "First name can only contain letters and hyphens.")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "You cannot leave the last name blank.")]
        [StringLength(100, ErrorMessage = "Last name cannot be more than 100 characters long.")]
        [RegularExpression(@"^[A-Za-z-]+$", ErrorMessage = "Last name can only contain letters and hyphens.")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DOB { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Enter a valid 10-digit phone number.")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10)]
        public string Phone { get; set; } = string.Empty;

        [StringLength(255)]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Please follow the correct email format test@email.com")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Client Folder URL")]
        [StringLength(2048)]
        [DataType(DataType.Url)]
        public string? ClientFolderUrl { get; set; }

        [Display(Name = "Archived")]
        public bool IsArchived { get; set; }

        public ICollection<Session> Sessions { get; set; } = new HashSet<Session>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DOB > DateTime.Today)
            {
                yield return new ValidationResult("Date of Birth cannot be in the future.", new[] { "DOB" });
            }
            else if (Age < 7)
            {
                yield return new ValidationResult("Client must be at least 18 years old.", new[] { "DOB" });
            }
        }
    }
}
