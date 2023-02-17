using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using SQLite;

using StudentsCourses.Models;
using StudentsCourses.DataBase;

using System;
using System.IO;

namespace StudentsCourses;

public static class MauiProgram
{
    //FileAccessHelper.GetLocalFilePath("people.db3");
    public static string filename = "students";

    public static SQLiteConnection conn;


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

        string dbPath = Repository.DBPATH;
        builder.Services.AddSingleton<Repository>(s => ActivatorUtilities.CreateInstance<Repository>(s, dbPath));
         

        return builder.Build();
	}

	 
}
