using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repos
{
    public class UnitOfWork : IUnitOfWork
    {
        public IEmployeeRepo EmployeeRepo { get; set; }

        public IDepartmentRepo DepartmentRepo { get; set; }

        public UnitOfWork(IEmployeeRepo employeeRepo, IDepartmentRepo departmentRepo)
        {
            EmployeeRepo = employeeRepo;
            DepartmentRepo = departmentRepo;

        }
    }
}
