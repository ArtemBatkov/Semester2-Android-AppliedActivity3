using StudentsCourses.DataBase;

namespace StudentsCourses;

public partial class App : Application
{
    public static Repository MyRepo { get; private set; }

    public App(Repository repo)
	{
		InitializeComponent();

		MainPage = new AppShell();

        MyRepo = repo;//

    }
}

