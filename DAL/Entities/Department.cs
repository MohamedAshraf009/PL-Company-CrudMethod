using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Department
    {
        public Department()
        {
            DateOfCreation = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Required")]
        public string code { get; set; } = "";
        [Required(ErrorMessage = "Department name is required")]
        [MinLength(3,ErrorMessage = "minimum length for name is 3 characters")]

        public string Name { get; set; } = "";
        public DateTime DateOfCreation { get; set; }

        public ICollection<Employee> employees { get; set; } = new List<Employee>();
    }
}
