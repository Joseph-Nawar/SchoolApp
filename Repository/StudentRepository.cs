using SchoolApp.Interface;
using SchoolApp.Models;
using SchoolApp.Data;

namespace SchoolApp.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly DataContext _context;

        public StudentRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateStudent(Student student)
        {
            _context.Add(student);
            return Save();
        }

        public bool DeleteStudent(Student student)
        {
            _context.Remove(student);
            return Save();
        }

        public Student GetStudent(int studentId)
        {
            return _context.Students
                .Where(s => s.Id == studentId)
                .FirstOrDefault();
        }

        public ICollection<Student> GetStudentsByTeacher(int teacherId)
        {
            return _context.TeacherStudents
                .Where(t => t.Teacher.Id == teacherId)
                .Select(s => s.Student)
                .ToList();
        }

        public ICollection<Student> GetStudents()
        {
            return _context.Students.ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool StudentExists(int studentId)
        {
            return _context.Students.Any(s => s.Id == studentId);
        }

        public bool UpdateStudent(Student student)
        {
            _context.Update(student);
            return Save();
        }
    }
}
