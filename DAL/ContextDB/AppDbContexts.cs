using DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ContextDB
{
    public class AppDbContexts:IdentityDbContext<ApplicationUser>
    {
        public AppDbContexts(DbContextOptions<AppDbContexts> options):base(options) 
        { 

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("server=.;database=MVCProjectRoute; trusted_connection=true;");
        //}

        public virtual DbSet<Department> Departments { get; set; }  

        public virtual DbSet<Employee> Employees { get; set; }


    }
}
