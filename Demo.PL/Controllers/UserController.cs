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
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}
		public async Task<IActionResult> Index(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
			{
		
				var users = await _userManager.Users.Select(User => new UserViewModel  
				{
					Email = User.Email,
					FName = User.FName,
					lName = User.LName,
					Phone = User.PhoneNumber,
					Roles = _userManager.GetRolesAsync(User).Result,
					Id = User.Id
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
						lName = user.LName,
						Phone = user.PhoneNumber,
						Roles = _userManager.GetRolesAsync(user).Result,
						Id = user.Id
					};
					return View(new List<UserViewModel> { mapped });
				}
			}
			return View();
		}
	}
}
