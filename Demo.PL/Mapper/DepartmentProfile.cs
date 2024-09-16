using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;

namespace Demo.PL.Mapper
{
    public class DepartmentProfile: Profile
    {
        public DepartmentProfile() 
        {
            CreateMap<DepartmentViewModel , Department>().ReverseMap();
        }
    }
}
