using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PhonebookClient.ViewModels
{
    public class ContactViewModel
    {
        [DisplayName("Contact id")]
        public int ContactId { get; set; }

        [DisplayName("First name")]
        public string FirstName { get; set; }

        [DisplayName("Last name")]
        public string LastName { get; set; }

        [DisplayName("Contact number")]
        public string ContactNumber { get; set; }

        [DisplayName("Gender")]
        public string Gender { get; set; }

        [DisplayName("Country id")]
        public int CountryId { get; set; }

        [DisplayName("State id")]
        public int StateId { get; set; }

        [DisplayName("Is favourite")]
        public bool IsFavourite { get; set; }

        [DisplayName("Email id")]
        public string Email { get; set; }

        [DisplayName("Company name")]
        public string Company { get; set; }

        [DisplayName("File name")]
        public string FileName { get; set; }

        [DisplayName("Image bytes")]
        public byte[] ImageBytes { get; set; }

        public virtual CountryViewModel Country { get; set; }
        public virtual StateViewModel State { get; set; }
    }
}
