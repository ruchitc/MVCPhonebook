﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PhonebookClient.ViewModels
{
    public class RegisterViewModel
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

        [Required(ErrorMessage = "Email address is required")]
        [StringLength(50)]
        [EmailAddress]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email format.")]
        [DisplayName("Email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contact number is required")]
        [StringLength(15)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$", ErrorMessage = "Invalid contact number.")]
        [DisplayName("Contact number")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
        ErrorMessage = "The password must be at least 8 characters long and contain at least 1 uppercase letter, 1 number, and 1 special character.")]
        [DisplayName("Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DisplayName("Confirm password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Security question id 1 is required")]
        [DisplayName("Security question id 1")]
        public int SecurityQuestionId_1 { get; set; }

        [Required(ErrorMessage = "Security answer 1 is required")]
        [StringLength(50)]
        [DisplayName("Security answer 1")]
        public string SecurityAnswer_1 { get; set; }

        [Required(ErrorMessage = "Security question id 2 is required")]
        [DisplayName("Security question id 2")]
        public int SecurityQuestionId_2 { get; set; }

        [Required(ErrorMessage = "Security answer 2 is required")]
        [StringLength(50)]
        [DisplayName("Security answer 2")]
        public string SecurityAnswer_2 { get; set; }
    }
}