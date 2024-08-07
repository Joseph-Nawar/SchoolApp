using SchoolApp.Models;

namespace SchoolApp.Dto
{
    public class StudentDto : CreatSchoolDto
    {
        public int Id { get; set; }
    }

    public class CreatStudentDto
    {
        public string? Name { get; set; }

        public int? Grade { get; set; }

        public double GPA { get; set; }
    }
}
