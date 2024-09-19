using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.Helper;
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
        //private IEmployeeRepository _EmployeeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        //private readonly IDepartmentRepository _departmentRepository;

        public EmployeeController(IUnitOfWork unitOfWork,/* IEmployeeRepository employeeRepository*/ IWebHostEnvironment env,IMapper mapper)
        {
            //_EmployeeRepository = repository;
           _unitOfWork = unitOfWork;
            _env = env;
            _mapper = mapper;
            //_departmentRepository = departmentRepository;
        }
        public IActionResult Index(string SearchValue)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchValue)) { employees = _unitOfWork.EmployeeRepository.GetAll(); }
            else { employees = _unitOfWork.EmployeeRepository.SearchEmployeeByName(SearchValue); }
                
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
                ///

                if(employeeVM.Image!=null)
                {
                    employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");
                }
                var mapperEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _unitOfWork.EmployeeRepository.Add(mapperEmp);
              var count =  _unitOfWork.Complete();
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
            var Employee = _unitOfWork.EmployeeRepository.GetById(id.Value);
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
                employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");

                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _unitOfWork.EmployeeRepository.Update(mappedEmp);
                _unitOfWork.Complete();
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
                _unitOfWork.EmployeeRepository.Delete(MappedEmp);
              var count = _unitOfWork.Complete();
                if (count > 0)
                {
                    DocumentSettings.Delete(employeeVM.ImageName, "Images");
                }
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
