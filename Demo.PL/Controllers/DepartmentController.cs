using Demo.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    // inheritance => DepartmentController is a Controller
    //Associatian  => DepartmentController has a DepartmentRepository
    public class DepartmentController : Controller
    {
        private IDepartmentRepository _departmentRepository;
        public DepartmentController( IDepartmentRepository repository )
        {
           _departmentRepository = repository;  
        }
        public IActionResult Index()
        {
           var departments = _departmentRepository.GetAll(); 
            return View(departments);
        }
    }
}

