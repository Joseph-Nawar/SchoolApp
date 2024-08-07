using AutoMapper;
using SchoolApp.Dto;
using SchoolApp.Models;

namespace SchoolApp.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TeacherDto, Teacher>();
            CreateMap<StudentDto, Student>();
            CreateMap<SchoolDto, School>();
        }
    }
}
