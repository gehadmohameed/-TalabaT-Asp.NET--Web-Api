﻿using System.ComponentModel.DataAnnotations;

namespace Store.Api.DTOs
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
       [RegularExpression("^(?=.{6,10}$)(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&*()_+]).*$",
            ErrorMessage =" Password Must Contains 1 UpperCase , 1 LowerCase , 1  Digit, 1 Special Character ")]

        public string Password { get; set; }


    }
}
// "(?=^.{6,10}$)(?=. *[a-z])(?=. *[A-Z])(?=.*//d)(?=.*[!@#$%^&amp;*()_+]).*$"