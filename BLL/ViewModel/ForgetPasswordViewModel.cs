using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ViewModel
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "email is required")]
        [EmailAddress(ErrorMessage = "invalid email")]
        public string Email { get; set; }
    }

}
