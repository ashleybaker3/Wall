using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wall.Models
{
    public class LoginUser
    {
        
        [NotMapped]
        public int LoginUserID { get; set; }

        [Required(ErrorMessage = "Required")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string LogEmail { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Required")]
        [MinLength(8, ErrorMessage = "Invalid email/password.")]
        [Display(Name = "Password")]
        public string LogPassword { get; set; }
    }
}