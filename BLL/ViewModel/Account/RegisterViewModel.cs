using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ViewModel.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "email is required")]
        [EmailAddress(ErrorMessage = "invalid email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "password is required")]
        [MinLength(5, ErrorMessage = "minimum length is 5")]

        public string Password { get; set; }

        [Compare("Password")]
        [Required]
        [MinLength(5,ErrorMessage = "minimum length is 5")]
        public string ConfirmPassword { get; set; }

        public bool IsAgree { get; set; } 

    }
}
