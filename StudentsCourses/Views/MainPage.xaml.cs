using StudentsCourses.ViewModels;
using StudentsCourses.Views;

namespace StudentsCourses.Views;

public partial class MainPage : ContentPage
{
	
	public MainPage()
	{
		InitializeComponent();
        BindingContext = new StudentsViewModel();
    }

	 
}

