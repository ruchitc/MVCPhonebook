using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVCPhonebook.ViewModels
{
    public class ContactViewModel
    {
        [DisplayName("Contact Id")]
        public int ContactId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(15)]
        [DisplayName("First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(15)]
        [DisplayName("Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Contact number is required")]
        [StringLength(15)]
        [DataType(DataType.PhoneNumber)]
        [DisplayName("Contact number")]
        [RegularExpression(@"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$", ErrorMessage = "Invalid contact number.")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [StringLength(50)]
        [EmailAddress]
        [DisplayName("Email address")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Company name is required")]
        [StringLength(15)]
        [DisplayName("Company")]
        public string Company { get; set; }

        public string FileName { get; set; } = string.Empty;

        [DisplayName("File")]
        public IFormFile File { get; set; }
    }
}
