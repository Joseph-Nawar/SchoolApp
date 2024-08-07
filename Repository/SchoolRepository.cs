using SchoolApp.Interface;
using SchoolApp.Models;
using SchoolApp.Data;

namespace SchoolApp.Repository
{
    public class SchoolRepository : ISchoolRepository
    {
        private readonly DataContext _context;

        public SchoolRepository(DataContext context)
        {
            _context = context;
        }

        public bool SchoolExists(int id)
        {
            return _context.Schools.Any(c => c.Id == id);
        }

        public bool CreateSchool(School school)
        {
            _context.Add(school);
            return Save();
        }

        public bool DeleteSchool(School school)
        {
            _context.Remove(school);
            return Save();
        }

        public ICollection<School> GetSchools()
        {
            return _context.Schools.ToList();
        }

        public School GetSchool(int id)
        {
            return _context.Schools.Where(c => c.Id == id).FirstOrDefault();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateSchool(School school)
        {
            _context.Update(school);
            return Save();
        }

        public School GetSchoolByTeacher(int teacherId)
        {
            return _context.Teachers
                           .Where(t => t.Id == teacherId)
                           .Select(t => t.School)
                           .FirstOrDefault();
        }

        public ICollection<Teacher> GetTeachersFromSchool(int schoolId)
        {
            return _context.Teachers
                           .Where(t => t.SchoolId == schoolId)
                           .ToList();
        }

        public School GetSchoolByStudent(int studentId)
        {
            return _context.Students
                           .Where(s => s.Id == studentId)
                           .Select(s => s.School)
                           .FirstOrDefault();
        }

        public ICollection<Student> GetStudentsFromSchool(int schoolId)
        {
            return _context.Students
                           .Where(s => s.SchoolId == schoolId)
                           .ToList();
        }
    }
}
