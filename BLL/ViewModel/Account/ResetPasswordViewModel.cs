using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ViewModel.Account
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "password is required")]
        [MinLength(5, ErrorMessage = "minimum length is 5")]

        public string Password { get; set; }

        [Compare("Password")]
        [Required]
        [MinLength(5, ErrorMessage = "minimum length is 5")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "email is required")]
        [EmailAddress(ErrorMessage = "invalid email")]
        public string Email { get; set; }

        public string Token { get; set; }



    }
}
