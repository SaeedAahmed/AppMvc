using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Demo.PL.Controllers
{
    // inheritance => DepartmentController is a Controller
    //Associatian  => DepartmentController has a DepartmentRepository
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private IDepartmentRepository _departmentRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public DepartmentController( IUnitOfWork unitOfWork /*IDepartmentRepository repository*/ , IWebHostEnvironment env, IMapper mapper)
        {
           //_departmentRepository = repository;
           _unitOfWork = unitOfWork;
            _env = env;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            TempData.Keep();
           var departments = _unitOfWork.DepartmentRepository.GetAll();
            _unitOfWork.Complete();
            // ViewData
            ViewData["message"] = "Hello :)";
            // ViewBag
            // ViewBag.message = "Hello ";
            var department = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);

            return View(department);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(DepartmentViewModel departmentVM)
        {
            if (ModelState.IsValid)
            {
                var mapperDept = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                  _unitOfWork.DepartmentRepository.Add(mapperDept);
                var count = _unitOfWork.Complete();
                if (count > 0)
                    TempData["message"] = "The Department created successfully";
                    return RedirectToAction(nameof(Index));
            }
            return View(departmentVM);
        }
        [HttpGet]
        public IActionResult Details(int? id , string viewName = "Details")
        {
            if (!id.HasValue)
            {
                return BadRequest(); // 400
            }
            var department = _unitOfWork.DepartmentRepository.GetById(id.Value);
            if (department == null)
            {
                return NotFound(); // 404
            }
            var MappedDept = _mapper.Map<Department, DepartmentViewModel>(department);

            return View(viewName, MappedDept);
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
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id,DepartmentViewModel departmentVM)
        {
            if(id!= departmentVM.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(departmentVM);    
            try
            {
                var MappedDept = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                _unitOfWork.DepartmentRepository.Update(MappedDept);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Occurred During Update Department");

                return View(departmentVM);
            }
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(DepartmentViewModel departmentVM)
        {
            try
            {
                var MappedDept = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                _unitOfWork.DepartmentRepository.Delete(MappedDept);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));         
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Occurred During Delete Department");

                return View(departmentVM);
            }
        }
    }
}

