using SchoolApp.Models;
using System.Diagnostics.Metrics;

namespace SchoolApp.Interface
{
    public interface ISchoolRepository
    {
        ICollection<School> GetSchools();
        School GetSchool(int id);
        bool SchoolExists(int id);
        bool CreateSchool(School school);
        bool UpdateSchool(School school);
        bool DeleteSchool(School school);
        bool Save();

        School GetSchoolByTeacher(int teacherId);
        ICollection<Teacher> GetTeachersFromSchool(int schoolId);
        School GetSchoolByStudent(int studentId);
        ICollection<Student> GetStudentsFromSchool(int schoolId);
    }
}
