using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using SQLite;  //related to SQLite



namespace StudentsCourses.Models
{
    // [Table("student")]  //related to SQLite
    public class Student
    {
         
        private string _name;
        // [Column("_name"), MaxLength(100)] //related to SQLite
        public string StudentName { get => _name; set => _name = value; }


        private int _id;
        //[PrimaryKey, AutoIncrement, Column("_id")]  //related to SQLite
        public int StudentId { get => _id; set => _id = value; }

        private string _surname;
        //[Column("_surname"),MaxLength(100)]  //related to SQLite
        public string StudentSurname { get => _surname; set => _surname = value; }

        private float _gpa;
        public float StudentGPA { get => _gpa; set => _gpa = value; }
        public Student(int id, string name, string surname, float gpa)
        {
            this.StudentId = id;
            this.StudentName = name;
            this.StudentSurname = surname;
            this.StudentGPA = gpa;
        }

        public string getStudentName() => StudentName;
        public string getStudentSurname() => StudentSurname;
        public int getStudentId() => StudentId;
    }
}
