using AutoMapper;
using Laptop.DataModels;
using Laptop.Models;
using Microsoft.AspNetCore.Routing.Constraints;

namespace Laptop.Helper
{
    public class AutoMapperHandler : Profile
    {
        public AutoMapperHandler()
        {
            CreateMap<Banks, BanksModel>()
                .ForMember(dest=> dest.Details, opt=> opt.MapFrom(src => src.Name + " " + src.Email))
                .ReverseMap();
        }
    }
}
