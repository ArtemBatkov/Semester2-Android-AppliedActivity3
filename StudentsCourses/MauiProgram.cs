using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
//using SQLite; //related to SQLite

using StudentsCourses.Models;
//using StudentsCourses.DataBase;  //related to SQLite

using System;
using System.IO;
using StudentsCourses.ViewModels;

namespace StudentsCourses;

public static class MauiProgram
{
    //FileAccessHelper.GetLocalFilePath("people.db3");
    public static string filename = "students";

    //public static SQLiteConnection conn;


    public static MauiApp CreateMauiApp()
    { 
        var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
        //string path = FileAccessHelper.GetLocalFilePath("people.db3");


#if DEBUG
        builder.Logging.AddDebug();
#endif

        // string dbPath = Repository.DBPATH; //related to SQLite
        builder.Services.AddSingleton<StudentsViewModel>();


        return builder.Build();
	}

	 
}
