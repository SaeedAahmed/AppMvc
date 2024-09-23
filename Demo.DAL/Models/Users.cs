using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
	public class Users :IdentityUser
	{
		[Required(ErrorMessage = "FName Is Required")]
		public string FName { get; set; }
		[Required(ErrorMessage = "LName Is Required")]
		public string LName { get; set; }
		[Required(ErrorMessage = "Required To Agree")]
		public bool IsAgree { get; set; }
	}
}
