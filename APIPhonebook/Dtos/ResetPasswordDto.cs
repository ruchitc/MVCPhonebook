using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace APIPhonebook.Dtos
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Login id is required")]
        [StringLength(15)]
        [DisplayName("Login id")]
        public string LoginId { get; set; }

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

        [Required(ErrorMessage = "New password is required")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
        ErrorMessage = "The password must be at least 8 characters long and contain at least 1 uppercase letter, 1 number, and 1 special character.")]
        [DisplayName("New password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm new password is required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        [DisplayName("Confirm new password")]
        public string ConfirmNewPassword { get; set; }
    }
}
