﻿using System.ComponentModel.DataAnnotations;

namespace APIPhonebook.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
