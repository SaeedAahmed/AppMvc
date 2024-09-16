using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace Demo.PL.Controllers
{

    public class EmployeeController : Controller
    {
        private IEmployeeRepository _EmployeeRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        //private readonly IDepartmentRepository _departmentRepository;

        public EmployeeController(IEmployeeRepository repository, IWebHostEnvironment env,IMapper mapper  /*,IDepartmentRepository departmentRepository*/)
        {
            _EmployeeRepository = repository;
            _env = env;
            _mapper = mapper;
            //_departmentRepository = departmentRepository;
        }
        public IActionResult Index(string SearchValue)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchValue))
                employees = _EmployeeRepository.GetAll();   
            else
                employees=_EmployeeRepository.SearchEmployeeByName(SearchValue);

            var MappedEmp = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(MappedEmp);
        }
        [HttpGet]
        public IActionResult Create()
        {
            //ViewBag.Department=_departmentRepository.GetAll();
            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                ///Manual Mapping
                ///var employee = new Employee()
                ///{
                ///    Name = employeeVM.Name,
                ///    Address = employeeVM.Address,
                ///    Email = employeeVM.Email,
                ///    Salary = employeeVM.Salary,
                ///    Age = employeeVM.Age,
                ///    DepartmentId = employeeVM.DepartmentId,
                ///    IsActive = employeeVM.IsActive,
                ///    HireDate = employeeVM.HireDate,
                ///    Phone = employeeVM.Phone,
                ///    IsDeleted = employeeVM.IsDeleted,
                ///    Gender = employeeVM.Gender
                ///};
                var mapperEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                var count = _EmployeeRepository.Add(mapperEmp);
                if (count > 0)
                    TempData["message"] = "The employee created successfully";

                return RedirectToAction(nameof(Index));
            }
            return View((employeeVM));
        }
        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (!id.HasValue)
            {
                return BadRequest(); 
            }
            var Employee = _EmployeeRepository.GetById(id.Value);
            //ViewBag.Department = _departmentRepository.GetAll();
            if (Employee == null)
            {
               

                return NotFound();

            }
            var MappedEmp = _mapper.Map<Employee, EmployeeViewModel>(Employee);
            return View(viewName, MappedEmp);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {

            return Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(employeeVM);


            try
            {
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _EmployeeRepository.Update(mappedEmp);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Occurred During Update Employee");

                return View(employeeVM);
            }
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int? Id, EmployeeViewModel employeeVM)
        {
            try
            {
                var MappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _EmployeeRepository.Delete(MappedEmp);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Occurred During Delete Employee");

                return View(employeeVM);
            }
        }
    }
}
