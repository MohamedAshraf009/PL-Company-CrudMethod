using BLL.Interfaces;
using DAL.ContextDB;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repos
{
    public class EmployeeRepo : GenricRepo<Employee>, IEmployeeRepo
    {
        private readonly AppDbContexts context;

        public EmployeeRepo(AppDbContexts _context):base(_context)
        {
            context = _context;
        }

        public async Task<string> GetDepartmentName(int? id)
        {
            var deptName = await 
                context.Employees.Include(a=>a.department).FirstOrDefaultAsync(dept=> dept.Id == id);

            return deptName.department.Name;

        }

        public async Task<IEnumerable<Employee>> GetEmployeeByDeptName(string deptName)
        {
           return await context.Employees.Where(a => a.department.Name == deptName).ToListAsync();
        }

        public async Task<IEnumerable<Employee>> SearchEmployees(string name)
        {
            var searched = await context.Employees.Where(emp => emp.Name.Contains(name)).ToListAsync();

            return searched;
        }
    }

}
