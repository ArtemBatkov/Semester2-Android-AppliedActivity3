using StudentsCourses.Models;

namespace StudentsCourses.Services
{
    interface IStudentsCoursesDataStore
    {
        Task<List<Student>> GetStudentsAsync();
        Task<Student> GetStudentAsync(int studentId);
        Task<int> SaveStudentAsync(Student student);
        Task<int> DeleteStudentAsync(Student student);

        Task<List<Course>> GetCoursesAsync();
        Task<Course> GetCourseAsync(int courseId);
        Task<int> SaveCourseAsync(Course course);
        Task<int> DeleteCourseAsync(Course course);

        Task  RedefinedCoursesPK();
        Task RedefinedStudentsPK();
    }
}
