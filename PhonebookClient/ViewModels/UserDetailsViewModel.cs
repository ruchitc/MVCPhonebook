using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PhonebookClient.ViewModels
{
    public class UserDetailsViewModel
    {
        public int userId { get; set; }

        [DisplayName("First name")]
        public string FirstName { get; set; }

        [DisplayName("Last name")]
        public string LastName { get; set; }

        [DisplayName("Login id")]
        public string LoginId { get; set; }

        [DisplayName("Email address")]
        public string Email { get; set; }

        [DisplayName("Contact number")]
        public string ContactNumber { get; set; }
    }
}
