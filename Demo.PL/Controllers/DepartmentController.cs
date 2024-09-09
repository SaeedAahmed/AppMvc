using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;

namespace Demo.PL.Controllers
{
    // inheritance => DepartmentController is a Controller
    //Associatian  => DepartmentController has a DepartmentRepository
    public class DepartmentController : Controller
    {
        private IDepartmentRepository _departmentRepository;
        private readonly IWebHostEnvironment _env;

        public DepartmentController( IDepartmentRepository repository , IWebHostEnvironment env)
        {
           _departmentRepository = repository;
            _env = env;
        }
        public IActionResult Index()
        {
           var departments = _departmentRepository.GetAll(); 
            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                var count = _departmentRepository.Add(department);
                if (count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(department);
        }
        [HttpGet]
        public IActionResult Details(int? id , string viewName = "Details")
        {
            if (!id.HasValue)
            {
                return BadRequest(); // 400
            }
            var department = _departmentRepository.GetById(id.Value);
            if (department == null)
            {
                return NotFound(); // 404
            }
            return View(viewName,department);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ///if(!id.HasValue)
            ///{
            ///    return BadRequest();
            ///}
            ///var department = _departmentRepository.GetById(id.Value);
            ///if(department == null)
            ///{
            ///    return NotFound();
            ///}
            ///return View(department);

            return Details(id, "Edit");
        }
        [HttpPost]
        public IActionResult Edit([FromRoute] int id,Department department)
        {
            if(id!=department.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(department);
            
         
            try
            {
                _departmentRepository.Update(department);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Occured During Update Department");

                return View(department);
            }
        }
    }
}

