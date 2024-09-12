using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;

namespace Demo.PL.Controllers
{

    public class EmployeeController : Controller
    {
        private IEmployeeRepository _EmployeeRepository;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IEmployeeRepository repository, IWebHostEnvironment env)
        {
            _EmployeeRepository = repository;
            _env = env;
        }
        public IActionResult Index()
        {
            var Employees = _EmployeeRepository.GetAll();
            return View(Employees);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee Employee)
        {
            if (ModelState.IsValid)
            {
                var count = _EmployeeRepository.Add(Employee);
                if (count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(Employee);
        }
        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (!id.HasValue)
            {
                return BadRequest(); 
            }
            var Employee = _EmployeeRepository.GetById(id.Value);
            if (Employee == null)
            {
                return NotFound(); 
            }
            return View(viewName, Employee);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {

            return Details(id, "Edit");
        }
        [HttpPost]
        public IActionResult Edit([FromRoute] int id, Employee Employee)
        {
            if (id != Employee.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(Employee);


            try
            {
                _EmployeeRepository.Update(Employee);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Occurred During Update Employee");

                return View(Employee);
            }
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }
        [HttpPost]
        public IActionResult Delete(Employee Employee)
        {
            try
            {
                _EmployeeRepository.Delete(Employee);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Occurred During Delete Employee");

                return View(Employee);
            }
        }
    }
}
