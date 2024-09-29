using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        #region Index
        public async Task<IActionResult> Index(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {

                var users = await _userManager.Users.Select(U => new UserViewModel
                {
                    Email = U.Email,
                    FName = U.FName,
                    LName = U.LName,
                    PhoneNumber = U.PhoneNumber,
                    Roles = _userManager.GetRolesAsync(U).Result,
                    Id = U.Id
                }).ToListAsync();
                return View(users);
            }
            else
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user is not null)
                {
                    var mapped = new UserViewModel
                    {
                        Email = user.Email,
                        FName = user.FName,
                        LName = user.LName,
                        PhoneNumber = user.PhoneNumber,
                        Roles = _userManager.GetRolesAsync(user).Result,
                        Id = user.Id
                    };
                    return View(new List<UserViewModel> { mapped });
                }
            }
            return View();
        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return NotFound();

            var MappedUser = _mapper.Map<ApplicationUser, UserViewModel>(user);
            return View(viewName, MappedUser);
        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(string id)
        {

            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Edit([FromRoute] string id, UserViewModel userVM)
        {
            if (id == userVM.Id)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var user = await _userManager.FindByIdAsync(id);
                        user.FName = userVM.FName;
                        user.LName = userVM.LName;
                        user.PhoneNumber = userVM.PhoneNumber;
                        var result = await _userManager.UpdateAsync(user);

                        return RedirectToAction(nameof(Index));
                    }
                    catch (System.Exception)
                    {

                        return BadRequest();
                    }
                }
                return View(userVM);

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
     
        public async Task<IActionResult> Delete(UserViewModel userVM)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userVM.Id);
                if (user is null)
                    return NotFound();

                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction(nameof(Index));
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            catch (System.Exception)
            {
                throw;
            }
          
            return View(userVM);

        }
        #endregion
    }
}
