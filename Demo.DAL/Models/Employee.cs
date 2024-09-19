using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public int? Age { get; set; }
        public string ImageName { get; set; }
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        
        public string Phone { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsDeleted { get; set; }// soft delete
        public Gender Gender { get; set; }
        public int? DepartmentId { get; set; } // Foreign Key
        public Department Department { get; set; }
    }
}
