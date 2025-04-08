using AutoMapper;
using Course_Management_System.Models.Domain;
using Course_Management_System.Models.DTO;

namespace CourseManagementSystem.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Course, CreateCourseRequestDto>().ReverseMap();
            CreateMap<Course, GetCourseRequestDto>().ReverseMap();
            CreateMap<Course, UpdateCourseRequestDto>().ReverseMap();
        }
    }
}
