using Demo.DAL.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name Is Required")]
        [MaxLength(50, ErrorMessage = "Maximum length of name 50 ")]
        [MinLength(5, ErrorMessage = "Minimum length of name 5 ")]
        public string Name { get; set; }

        [Range(22, 60, ErrorMessage = "Age Must Be Between 22 and 60")]
        public int? Age { get; set; }
        [RegularExpression(@"^[0-9]{1,10}-[a-zA-Z]{1,40}-[a-zA-Z]{1,40}-[a-zA-Z]{1,40}$", ErrorMessage = "Address Must be in form of '123-Street-Region-City'")]
        public string Address { get; set; }
        
        [Range(4000, 8000, ErrorMessage = "Salary Must Be Between 4000 & 8000")]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        [EmailAddress(ErrorMessage = "Enter Email in a correct form ")]
        public string Email { get; set; }
        [Phone]
        public string Phone { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsDeleted { get; set; }// soft delete
        public Gender Gender { get; set; }
        public int? DepartmentId { get; set; } // Foreign Key
        public Department Department { get; set; }

    }
}
