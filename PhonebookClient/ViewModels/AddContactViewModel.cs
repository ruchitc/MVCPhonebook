using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PhonebookClient.ViewModels
{
    public class AddContactViewModel
    {
        [DisplayName("First name")]
        [Required(ErrorMessage = "First name is required")]
        [StringLength(15)]
        public string FirstName { get; set; }

        [DisplayName("Last name")]
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(15)]
        public string LastName { get; set; }

        [DisplayName("Contact number")]
        [Required(ErrorMessage = "Contact number is required")]
        [StringLength(15)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$", ErrorMessage = "Invalid contact number.")]
        public string ContactNumber { get; set; }

        [DisplayName("Gender")]
        [Required(ErrorMessage = "Gender is required")]
        [StringLength(1)]
        public string Gender { get; set; }

        [DisplayName("Country id")]
        [Required(ErrorMessage = "Country id is required")]
        public int CountryId { get; set; }

        [DisplayName("State id")]
        [Required(ErrorMessage = "State id is required")]
        public int StateId { get; set; }

        [DisplayName("Is favourite")]
        [Required(ErrorMessage = "Is favourite field is required")]
        public bool IsFavourite { get; set; }

        [DisplayName("Email id")]
        [StringLength(50)]
        [EmailAddress]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        [DisplayName("Company name")]
        [StringLength(15)]
        public string? Company { get; set; }

        [DisplayName("Image")]
        public IFormFile? Image { get; set; }
    }
}
