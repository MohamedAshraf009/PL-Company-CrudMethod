using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IEmployeeRepo : IGenricRepo<Employee>
    {

        public Task<IEnumerable<Employee>> GetEmployeeByDeptName(string deptName);

        public Task<string> GetDepartmentName(int? id);

        public Task<IEnumerable<Employee>> SearchEmployees(string name); 
    }
}
