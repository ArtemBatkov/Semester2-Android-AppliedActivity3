using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using StudentsCourses.Models;
using System;
using System.IO;

namespace StudentsCourses.DataBase
{
    public class Repository
    {
        private SQLiteConnection conn;
        public Repository()
        {
            Init();
        }

        private void Init()
        {
            if (conn != null)
                return;
            //string _dbPath = FileAccessHelper.GetLocalFilePath("student.db3");
            conn = new SQLiteConnection(DBPATH);
            conn.CreateTable<Student>();
        }

        public static string DBPATH { get; } = 
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "students.db3");


    }



}

