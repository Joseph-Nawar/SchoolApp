using SchoolApp.Interface;
using SchoolApp.Models;
using SchoolApp.Data;

namespace SchoolApp.Repository
{
    public class TeacherRepository : ITeacherRepository
    {
        private DataContext _context;

        public TeacherRepository(DataContext context)
        {
                _context = context;
        }

        public bool CreateTeacher(Teacher teacher)
        {
            _context.Add(teacher);
            return Save();
        }

        public bool DeleteTeacher(Teacher teacher)
        {
            _context.Remove(teacher);
            return Save();
        }

        public ICollection<Student> GetStudentByTeacher(int teacherId)
        {
            return _context.TeacherStudents.Where(t => t.Teacher.Id == teacherId).Select(s => s.Student).ToList();
        }

        public Teacher GetTeacher(int teacherId)
        {
            return _context.Teachers.Where(t => t.Id == teacherId).FirstOrDefault();
        }

        public ICollection<Teacher> GetTeacherByStudent(int studentId)
        {
            return _context.TeacherStudents.Where(s => s.Student.Id == studentId).Select(t => t.Teacher).ToList();
        }

        public ICollection<Teacher> GetTeachers()
        {
            return _context.Teachers.ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool TeacherExists(int teacherDd)
        {
            return _context.Teachers.Any(t => t.Id == teacherDd);
        }

        public bool UpdateTeacher(Teacher teacher)
        {
            _context.Update(teacher);
            return Save();
        }
    }
}
