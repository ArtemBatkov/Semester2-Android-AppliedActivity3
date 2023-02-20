//using StudentsCourses.DataBase;  //related to SQLite

namespace StudentsCourses;

public partial class App : Application
{
    // public static Repository MyRepo { get; private set; }  //related to SQLite

    public App()//parameter Repository repo
    {
		InitializeComponent();

		MainPage = new AppShell();

       // MyRepo = repo;//

    }
}

