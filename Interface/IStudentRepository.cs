using SchoolApp.Models;
using System.Collections.Generic;

namespace SchoolApp.Interface
{
    public interface IStudentRepository
    {
        ICollection<Student> GetStudents();

        Student GetStudent(int studentId);

        bool StudentExists(int studentId);

        bool CreateStudent(Student student);

        bool UpdateStudent(Student student);

        bool DeleteStudent(Student student);

        bool Save();
    }
}
