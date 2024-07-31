using SchoolApp.Models;

namespace SchoolApp.Interface
{
    public interface ITeacherRepository
    {
        ICollection<Teacher> GetTeachers(); 

        Teacher GetTeacher(int teacherId);

        bool TeacherExists(int teacherDd);

        ICollection<Teacher> GetTeacherByStudent(int studentId);

        ICollection<Student> GetStudentByTeacher(int teacherId);

        bool CreateTeacher(Teacher teacher);

        bool UpdateTeacher(Teacher teacher);

        bool DeleteTeacher(Teacher teacher);

        bool Save();

    }
}
