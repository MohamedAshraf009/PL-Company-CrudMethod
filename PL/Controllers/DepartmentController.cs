using AutoMapper;
using BLL.Interfaces;
using BLL.ViewModel;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork context;
        private readonly IMapper mapper;

        public DepartmentController(IUnitOfWork _context,IMapper _Mapper) 
        {
            context = _context;
            mapper = _Mapper;
        }
        public async Task<IActionResult> Index()
        {
            var departments = await context.DepartmentRepo.GetAll();
            var Row = mapper.Map<IEnumerable<Department>,IEnumerable<DepartmentViewModel>>(departments);

            return View(Row);
        }
        [HttpGet]
        public IActionResult create()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> create(DepartmentViewModel department)
        {
            if(ModelState.IsValid)
            {
                var DeptCreated = mapper.Map<DepartmentViewModel,Department>(department);
                await context.DepartmentRepo.AddEntity(DeptCreated);
                return RedirectToAction("Index");
            }
            return View(department);
        }

        public async Task<IActionResult> details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }
            var department = await context.DepartmentRepo.GetById(id);
            var detail = mapper.Map<Department,DepartmentViewModel>(department);

            if(detail is null)
            {
                return NotFound();
            }

            return View(detail);
        }

        public async Task<IActionResult> delete(int? id)
        {
            if(id is null)
            {
                return NotFound();
            }
            var row = await context.DepartmentRepo.GetById(id);

            if (row is null)
            {
                return NotFound();
            }
            await context.DepartmentRepo.DeleteEntity(row);

            return RedirectToAction("Index");

        }

        public async Task<IActionResult> update(int? id)
        {
            if(id is null)
            {
                return NotFound();
            }
            var row =await context.DepartmentRepo.GetById(id);
            var rowToUpdate= mapper.Map<DepartmentViewModel>(row);

            if(row is null)
            {
                return NotFound();
            }

            //await context.UpdateDepartment(row);
            return View(rowToUpdate);
        }
        [HttpPost]
        public async Task<IActionResult> update(int? id, DepartmentViewModel department)
        {

            if (id != department.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var deptToEdit = mapper.Map<DepartmentViewModel, Department>(department);

                    await context.DepartmentRepo.UpdateEntity(deptToEdit);
                    return RedirectToAction("Index");
                }
                catch(Exception e)
                {
                    return View(department);
                }
            }
            return View(department);
        }

    }
}
