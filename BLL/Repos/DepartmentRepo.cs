using BLL.Interfaces;
using DAL.ContextDB;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repos
{
    public class DepartmentRepo : GenricRepo<Department>,IDepartmentRepo
    {
        private readonly AppDbContexts context;

        public DepartmentRepo(AppDbContexts _context):base(_context)
        {
            context = _context;
        }
    }
}
