using AutoMapper;
using LMS20.Core.Entities;
using LMS20.Core.ViewModels;

namespace LMS20.Web.Automapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Course, CreateCoursePartialViewModel>().ReverseMap();

            CreateMap<ApplicationUser, RegistrationViewModel>().ReverseMap();


        }
    }
}
