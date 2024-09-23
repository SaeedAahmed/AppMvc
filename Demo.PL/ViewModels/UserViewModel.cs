using System.Collections.Generic;
using System;

namespace Demo.PL.ViewModels
{
	public class UserViewModel
	{
		public string Id { get; set; }
		public string FName { get; set; }
		public string lName { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public IEnumerable<string> Roles { get; set; }
		public UserViewModel()
		{
			Id = Guid.NewGuid().ToString();
		}
	}
}
