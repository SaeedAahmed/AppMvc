using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class SignInViewModel
	{
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid Email")]
		public string Email { get; set; }
		[Required(ErrorMessage = "Password is Required")]
		[MinLength(5, ErrorMessage = "Password is short")]
		public string Password { get; set; }
		[Required(ErrorMessage = "Required To Agree")]
		public bool IsAgree { get; set; }
		public bool RememberMe { get; set; }
	}
}
