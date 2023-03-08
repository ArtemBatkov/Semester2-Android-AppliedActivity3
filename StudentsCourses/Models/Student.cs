using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentsCourses.Services;
using StudentsCourses.ViewModels;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
//using SQLite;  //related to SQLite




namespace StudentsCourses.Models
{
    // [Table("student")]  //related to SQLite
    public class Student: ObservableObject
    {



        private string _name;
        // [Column("_name"), MaxLength(100)] //related to SQLite
        public string StudentName
        {
            get => _name;
            set => _name = value;
        }

        private static readonly StudentsViewModel vm = new StudentsViewModel();

        private int _id;
        //[PrimaryKey, AutoIncrement, Column("_id")]  //related to SQLite
        [PrimaryKey, AutoIncrement]
        public int StudentId
        {
            get => _id;
            set => _id = value;



        }

        private string _surname;
        //[Column("_surname"),MaxLength(100)]  //related to SQLite
        public string StudentSurname { get => _surname; set => _surname = value; }

        private float _gpa;
        public float StudentGPA
        {
            get => _gpa; set
            {
                float fvalue = 0;
                if (float.TryParse(value.ToString(CultureInfo.InvariantCulture.NumberFormat), out fvalue))
                    _gpa = fvalue;
                else return;
            }
        }


        //private string _courseName;
        //public string StudentCourseName { get => _courseName; set => _courseName = value; }


        private string _course;
        [Column("Course")]
        public string Course
        {
            get => _course;
            set
            {
                _course = value;
                if (value != null) IsShowEnrollement = true;
                else IsShowEnrollement = false;
                OnPropertyChanged();
            }
        }


        [Ignore]
        public bool IsShowEnrollement { get => _isShow;
            set {
                _isShow = value;
                OnPropertyChanged();    
            } 
        }

        private bool _isShow;
    }
}
