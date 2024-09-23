using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
	
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
			_userManager = userManager;
			_signInManager = signInManager;
		}
		#region SignUp
		public IActionResult SignUp()
		{

			return View();

		}
		[HttpPost]
		public async Task<IActionResult> SignUp(SignUpViewModel ViewModel)
		{
			if (ModelState.IsValid) //server side validation
			{
				var user = new ApplicationUser()
				{
					UserName = ViewModel.Email.Split('@')[0],
					Email = ViewModel.Email,
					IsAgree = ViewModel.IsAgree,
					FName = ViewModel.FName,
					LName = ViewModel.LName,
				};
				var result = await _userManager.CreateAsync(user, ViewModel.Password);
				if (result.Succeeded)
					return RedirectToAction(nameof(SignIn));
				foreach (var error in result.Errors)
					ModelState.AddModelError(string.Empty, error.Description);
			}
			return View(ViewModel);
		}

		#endregion

		#region SignIn

		public IActionResult SignIn()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> SignIn(SignInViewModel ViewModel)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(ViewModel.Email);
				if (user is not null)
				{
					bool flag = await _userManager.CheckPasswordAsync(user, ViewModel.Password);
					if (flag)
					{
						var result = await _signInManager.PasswordSignInAsync(user, ViewModel.Password, ViewModel.RememberMe, false);
						if (result.Succeeded)
						{
							return RedirectToAction(nameof(HomeController.Index), "Home");
						}
					}
					ModelState.AddModelError(string.Empty, "Invalid Password");
				}
				ModelState.AddModelError(string.Empty, "Email is not Exsited");
			}
			return View(ViewModel);
		}
		#endregion

		#region SignOut
		public async Task<IActionResult> SignOut()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(SignIn));
		}
		#endregion

		#region ForgetPassword

		public IActionResult ForgetPassword()
		{
			return View();  
		}

		[HttpPost]
		public async Task<IActionResult> ForgetPassword( ForgetPasswordViewModel forgetPasswordVM)
		{
			if (ModelState.IsValid)
			{
				var user =await _userManager.FindByEmailAsync(forgetPasswordVM.Email);
				if (user is not null) 
				{
					string token = await _userManager.GeneratePasswordResetTokenAsync(user);
					var PasswordResetLink = Url.Action("ResetPassword", "Account", new { email = user.Email , token = token }, "https", "localhost:44376");

					var email = new Email()
					{
						Subject = "Reset Email",
						Body = PasswordResetLink,
						To = user.Email
					};
					EmailSetting.SendEmail(email);
					return RedirectToAction(nameof(checkedYourInbox));
				}
				ModelState.AddModelError(string.Empty, "Email Is Not Exsited");
			}
			return View(forgetPasswordVM);
		}

		public ActionResult checkedYourInbox()
		{
			return View();
		}

		#endregion

		#region RestPassword
		public IActionResult ResetPassword(string email , string token)
		{
			TempData["email"] = email;
			TempData["token"] = token;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordVM)
		{
			if (ModelState.IsValid)
			{
				string email = TempData["email"] as string;
				string token = TempData["token"] as string;

				var user = await _userManager.FindByEmailAsync(email);
				var result = await _userManager.ResetPasswordAsync(user,token, resetPasswordVM.Password);
				if (result.Succeeded)
				{
					return RedirectToAction("SignIn");
				}
				foreach(var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}
			return View(resetPasswordVM);
		}

		#endregion

	}
}
