namespace SchoolApp.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int? Hours { get; set; }
        public School? School { get; set; } // Navigation property
        public int SchoolId { get; set; } // Foreign key property

        //many to many
        public ICollection<TeacherStudent>? TeacherStudents { get; set; }
    }
}
