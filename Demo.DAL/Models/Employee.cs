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
    public class Employee:ModelBase
    {
        
        [Required(ErrorMessage ="Name is required") ]
        [MaxLength(50, ErrorMessage = "Max Length For Name is 50")]
        [MinLength(4, ErrorMessage = "MIn Length For Name is 4")]
        public string Name { get; set; }
        [Range(22, 60, ErrorMessage = "Age Must Be Between 22 and 60")]
        public int? Age { get; set; }
        [RegularExpression(@"^[0-9]{1,10}-[a-zA-Z]{1,40}-[a-zA-Z]{1,40}-[a-zA-Z]{1,40}$", ErrorMessage = "Address Must be in form of '123-Street-Region-City'")]
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        [Range(4000, 8000, ErrorMessage = "Salary Must Be Between 4000 & 8000")]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        [EmailAddress(ErrorMessage = "Enter Email in a correct form ")]
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
