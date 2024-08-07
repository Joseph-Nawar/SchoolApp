namespace SchoolApp.Models
{
    public class School
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public ICollection<Student> Students { get; set; }
        public ICollection<Teacher> Teachers { get; set; }

    }
}
