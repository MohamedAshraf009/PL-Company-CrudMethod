using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BLL.ViewModel
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "name is required")]
        [MinLength(4)]

        public string Name { get; set; }

        public int? age { get; set; }
        public string ImageUrl { get; set; }

        public IFormFile Image { get; set; }

        public string address { get; set; }
        [DataType(DataType.Currency)]

        public decimal salary { get; set; }

        public bool isActive { get; set; }

        [EmailAddress]
        public string email { get; set; }

        public string phoneNumber { get; set; }

        public DateTime HireDate { get; set; }

        public DateTime dateOfcreation { get; set; } = DateTime.Now;

        [ForeignKey("department")]
        public int depId { get; set; }
        public DepartmentViewModel department { get; set; }

    }
}
