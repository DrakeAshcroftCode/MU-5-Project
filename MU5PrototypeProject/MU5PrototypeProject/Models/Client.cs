using Azure;
using System.ComponentModel.DataAnnotations;

namespace MU5PrototypeProject.Models
{
    public class Client : IValidatableObject
    {
        public int ID { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "You cannot leave the first name blank.")]
        [StringLength(50, ErrorMessage = "First name cannot be more than 50 characters long.")]
        [RegularExpression(@"^[A-Za-z-]+$", ErrorMessage = "First name can only contain letters and hyphens.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "You cannot leave the last name blank.")]
        [StringLength(100, ErrorMessage = "Last name cannot be more than 100 characters long.")]
        [RegularExpression(@"^[A-Za-z-]+$", ErrorMessage = "Last name can only contain letters and hyphens.")]
        public string LastName { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^[2-9]\d{2}[2-9]\d{6}$", ErrorMessage = "Enter a valid 10-digit phone number.")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10)]
        public string Phone {  get; set; }

        [StringLength(255)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Session> Sessions { get; set; } = new HashSet<Session>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DOB > DateTime.Today)
            {
                yield return new ValidationResult("Date of Birth cannot be in the future.", new[] { "DOB" });
            }       
        }

    }
}
