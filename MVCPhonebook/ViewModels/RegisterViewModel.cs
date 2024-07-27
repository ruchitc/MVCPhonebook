﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MVCPhonebook.ViewModels
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

        [Required(ErrorMessage = "Login Id is required")]
        [StringLength(15)]
        [DisplayName("Login Id")]
        public string LoginId { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [StringLength(50)]
        [DisplayName("Email address")]
        [EmailAddress]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contact number is required")]
        [StringLength(15)]
        [DisplayName("Contact number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$", ErrorMessage = "Invalid contact number.")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
        ErrorMessage = "The password must be at least 8 characters long and contain at least 1 uppercase letter, 1 number, and 1 special character.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [DisplayName("Confirm password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
