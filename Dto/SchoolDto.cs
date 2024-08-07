using SchoolApp.Models;

namespace SchoolApp.Dto
{
    public class SchoolDto : CreatSchoolDto
    {
        public int Id { get; set; }
    }

    public class CreatSchoolDto
    {
        public string Name { get; set; }
        public string Location { get; set; }
    }
}
