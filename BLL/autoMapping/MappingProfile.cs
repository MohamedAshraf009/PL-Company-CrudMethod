using AutoMapper;
using BLL.ViewModel;
using DAL.Entities;
using DAL.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.autoMapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeViewModel>().ReverseMap();
                

            CreateMap<Department, DepartmentViewModel>().ReverseMap();  

            
        }

    }
}
