using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;



namespace StudentsCourses.Models
{
    [Table("student")]
    public class Student
    {
         
        private string _name;
        [Column("_name"), MaxLength(100)]
        public string StudentName { get => _name; set => _name = value; }


        private int _id;
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int StudentId { get => _id; set => _id = value; }

        private string _surname;
        [Column("_surname"),MaxLength(100)]
        public string StudentSurname { get => _surname; set => _surname = value; }

        public Student(int id, string name, string surname)
        {
            this.StudentId = id;
            this.StudentName = name;
            this.StudentSurname = surname;
        }

        public string getStudentName() => StudentName;
        public string getStudentSurname() => StudentSurname;
        public int getStudentId() => StudentId;
    }
}
