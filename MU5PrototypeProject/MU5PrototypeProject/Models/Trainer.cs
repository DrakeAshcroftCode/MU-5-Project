using System.ComponentModel.DataAnnotations;

namespace MU5PrototypeProject.Models
{
    public class Trainer
    {
        public int ID { get; set; }

        //Summary Properties
        [Display(Name = "Trainer Name")]
        public string TrainerName
        {
            get
            {
                return FirstName + " " + (string.IsNullOrEmpty(LastName));

            }
        }

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

        public ICollection<Session> Sessions { get; set; } = new HashSet<Session>();
    }
}
