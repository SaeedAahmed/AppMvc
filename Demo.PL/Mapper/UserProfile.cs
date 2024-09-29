using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Demo.PL.Mapper
{
    public class UserProfile :Profile
    {
        public UserProfile()
        {
            CreateMap<IdentityUser, UserViewModel>().ReverseMap();
        }
    }
}
