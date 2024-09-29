using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleController( RoleManager<IdentityRole> roleManager , IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        #region Index
        public async Task<IActionResult> Index(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {

                var roles = await _roleManager.Roles.Select(R => new RoleViewModel
                {
      
                    Id = R.Id,
                    RoleName=R.Name
                    
                }).ToListAsync();
                return View(roles);
            }
            else
            {
                var role = await _roleManager.FindByNameAsync(name);
                if (role is not null)
                {
                    var mapped = new RoleViewModel
                    {
                        RoleName = role.Name,
                        Id = role.Id
                    };
                    return View(new List<RoleViewModel> { mapped });
                }
            }
            return View();
        }
        #endregion


        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel roleVM)
        {
            if (ModelState.IsValid)
            {
                var mapped = _mapper.Map<RoleViewModel, IdentityRole>(roleVM);
                await _roleManager.CreateAsync(mapped);
                return RedirectToAction(nameof(Index));
            }
            return View(roleVM);
        }

        #region Details
        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var role = await _roleManager.FindByIdAsync(id);

            if (role is null)
                return NotFound();

            var MappedRole = _mapper.Map<IdentityRole, RoleViewModel>(role);
            return View(viewName, MappedRole);
        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(string id)
        {

            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModel roleVM)
        {
            if (id == roleVM.Id)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var role = await _roleManager.FindByIdAsync(id);
                         
                         role.Name=roleVM.RoleName;
                        var result = await _roleManager.UpdateAsync(role);

                        return RedirectToAction(nameof(Index));
                    }
                    catch (System.Exception)
                    {

                        return BadRequest();
                    }
                }
                return View(roleVM);

            }
            return BadRequest();

        }

        #endregion

        #region Delete
        public Task<IActionResult> Delete(string id)
        {
            return Details(id, "Delete");
        }
        [HttpPost]

        public async Task<IActionResult> Delete(UserViewModel roleVM)
        {
            try
            {
                var user = await _roleManager.FindByIdAsync(roleVM.Id);
                if (user is null)
                    return NotFound();

                var result = await _roleManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction(nameof(Index));
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            catch (System.Exception)
            {
                throw;
            }

            return View(roleVM);

        }
        #endregion
    }
}
