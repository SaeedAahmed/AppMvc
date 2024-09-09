using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
   public enum Gender
    {
        [EnumMember(Value ="Male")]
        Male=1,
        [EnumMember(Value = "Female")]
        Female =2
    }
    public enum EmployeeType
    {
        [EnumMember(Value = "FullTime")]
        FullTime =1,
        [EnumMember(Value = "PartTime")]
        PartTime =2
    }
    public class Employee
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Name is required") ]
        [MaxLength(50, ErrorMessage = "Max Length For Name is 50")]
        [MinLength(4, ErrorMessage = "MIn Length For Name is 4")]
        public string Name { get; set; }
        public int? Age { get; set; }
        [RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]-{5-10}-[a-zA-Z]-{4-10}-[a-zA-Z]-{5-10}$" 
                           ,ErrorMessage = "Address Must be Like 123-Street-City-Country"
        )]
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }
        [Display(Name ="Hire Date")]
        public DateTime HireDate { get; set; }
        public bool IsDeleted { get; set; }// soft delete
        public Gender Gender { get; set; }
    }
}
