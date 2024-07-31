namespace SchoolApp.Models
{
    public class Student
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int? Grade { get; set; }

        public double GPA { get; set; }

        public ICollection<TeacherStudent> TeacherStudents { get; set; } = new List<TeacherStudent>();

    }
}
