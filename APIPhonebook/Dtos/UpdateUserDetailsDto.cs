using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APIPhonebook.Dtos
{
    public class UpdateUserDetailsDto
    {
        public int userId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(15)]
        [DisplayName("First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(15)]
        [DisplayName("Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Login id is required")]
        [StringLength(15)]
        [DisplayName("Login id")]
        public string LoginId { get; set; }

        [Required(ErrorMessage = "Contact number is required")]
        [StringLength(15)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$", ErrorMessage = "Invalid contact number.")]
        [DisplayName("Contact number")]
        public string ContactNumber { get; set; }
    }
}
