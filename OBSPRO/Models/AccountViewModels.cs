﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OBSPRO.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        //[Required]
        //[Display(Name = "Email")]
        //[EmailAddress]
        //public string Email { get; set; }

        //[Display(Name = "Remember me?")]
        //public bool RememberMe { get; set; }

    }

}