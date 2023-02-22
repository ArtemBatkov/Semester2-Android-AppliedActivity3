//using StudentsCourses.DataBase;  //related to SQLite

using StudentsCourses.Services;

namespace StudentsCourses;

public partial class App : Application
{
    // public static Repository MyRepo { get; private set; }  //related to SQLite

    public App()//parameter Repository repo
    {
		InitializeComponent();

        DependencyService.Register<StudentsCoursesDataStoreSqlite>();

        MainPage = new AppShell();

       // MyRepo = repo;//

    }
}

