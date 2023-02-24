using StudentsCourses.Models;
using SQLite;

namespace StudentsCourses.Services
{
    class StudentsCoursesDataStoreSqlite : IStudentsCoursesDataStore
    {
        SQLiteAsyncConnection Database;

        private const string DatabaseFilename = "Students.db3";

        private const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        private static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(DatabasePath, Flags);
            var studentTable = await Database.CreateTableAsync<Student>();
            var courseTabe = await Database.CreateTableAsync<Course>();
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            await Init();
            return await Database.Table<Student>().ToListAsync();
        }

        public async Task<Student> GetStudentAsync(int studentId)
        {
            await Init();
            return await Database.Table<Student>().Where(i => i.StudentId == studentId).FirstOrDefaultAsync();
        }

        public async Task<int> SaveStudentAsync(Student student)
        {
            await Init();
            if (student.StudentId != 0)
            {
                return await Database.UpdateAsync(student);
            }
            else
            {
                return await Database.InsertAsync(student);
            }
        }

        public async Task<int> DeleteStudentAsync(Student student)
        {
            await Init();
            return await Database.DeleteAsync(student);
        }

        public async Task<List<Course>> GetCoursesAsync()
        {
            await Init();
            return await Database.Table<Course>().ToListAsync();
        }

        public async Task<Course> GetCourseAsync(int courseId)
        {
            await Init();
            return await Database.Table<Course>().Where(i => i.CourseId == courseId).FirstOrDefaultAsync();
        }

        public async Task<int> SaveCourseAsync(Course course)
        {
            await Init();
            if (course.CourseId != 0)
            {
                return await Database.UpdateAsync(course);
            }
            else
            {
                return await Database.InsertAsync(course);
            }
        }

        public async Task<int> DeleteCourseAsync(Course course)
        {
            await Init();
            return await Database.DeleteAsync(course);
        }

        public async Task RedefinedCoursesPK()
        {
            await Database.DropTableAsync<Course>();
            await Database.CreateTableAsync<Course>();
        }

        public async Task RedefinedStudentsPK()
        {
            await Database.DropTableAsync<Student>();
            await Database.CreateTableAsync<Student>();
        }
    }
}
