using AutoMapper;
using BLL.Interfaces;
using BLL.ViewModel;
using DAL.Entities;
using DAL.Migrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using PL.Helper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork context;
        //private readonly IDepartmentRepo departmentRepo;
        private readonly IMapper mapper;

        public EmployeeController(IUnitOfWork _context,IMapper _mapper)
        {
            context = _context;
            //this.departmentRepo = departmentRepo;
            mapper = _mapper;
        }

        public async Task<IActionResult> Index(string searchValue ="")
        {
            //ViewData["MessageData"] = "hello from viewData";
            //ViewBag.MessageBag = "Hello from VeiwBag";
            IEnumerable<Employee> employees;

            if (string.IsNullOrEmpty(searchValue))
            {
               employees = await context.EmployeeRepo.GetAll();
            }
            else
            {
                employees = await context.EmployeeRepo.SearchEmployees(searchValue);
            }

            var allEmployees = mapper.Map<IEnumerable<EmployeeViewModel>>(employees);

            return View(allEmployees);
        }


        [HttpGet]
        public async Task<IActionResult> create()
        {
            ViewBag.Departments = await context.DepartmentRepo.GetAll();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> create(EmployeeViewModel employee)
        {
            if (ModelState.IsValid)
            {
                employee.ImageUrl = DocumentsSettings.UploadFile(employee.Image, "images");
                TempData["Message"] = $"employee has been created with name {employee.Name}";
                var createEmp = mapper.Map<EmployeeViewModel, Employee>(employee);
                await context.EmployeeRepo.AddEntity(createEmp);

                return RedirectToAction("Index");
            }
            return View(employee);
        }

        public async Task<IActionResult> delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }
            var row = await context.EmployeeRepo.GetById(id);
            if (row is null)
            {
                return NotFound();
            }
            DocumentsSettings.DeleteFile("images", row.ImageUrl);
            await context.EmployeeRepo.DeleteEntity(row);

            return RedirectToAction("Index");

        }
        public async Task<IActionResult> update(int? id)
        {
            ViewBag.Departments = await context.DepartmentRepo.GetAll();
            if (id is null)
            {
                return NotFound();
            }
            var row = await context.EmployeeRepo.GetById(id);
            var rowToUpdate = mapper.Map<Employee, EmployeeViewModel>(row);

            if (rowToUpdate is null)
            {
                return NotFound();
            }

            //await context.UpdateDepartment(row);
            return View(rowToUpdate);
        }
        [HttpPost]
        public async Task<IActionResult> update(int? id, EmployeeViewModel employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    employee.ImageUrl = DocumentsSettings.UploadFile(employee.Image, "images");

                    var rowToEdit = mapper.Map<EmployeeViewModel, Employee>(employee);

                    await context.EmployeeRepo.UpdateEntity(rowToEdit);
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    return View(employee);
                }
            }
            return View(employee);
        }
        public async Task<IActionResult> details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }
            var employee = await context.EmployeeRepo.GetById(id);
            var departmentName = await context.EmployeeRepo.GetDepartmentName(id);

            employee.department.Name = departmentName;
            var detail = mapper.Map<Employee, EmployeeViewModel>(employee);
            if (detail is null)
            {
                return NotFound();
            }

            return View(detail);
        }
    }
}
