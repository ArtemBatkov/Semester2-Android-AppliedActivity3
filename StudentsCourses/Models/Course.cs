using SQLite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsCourses.Models
{
    class Course
    {
        private int _id;

        [PrimaryKey, AutoIncrement]
        public int CourseId { get => _id; set => _id = value; }

        private string _name;
        public string CourseName { get => _name; set => _name = value; }


        private int _length;
        public int CourseLength { get => _length;
            set {
                int ivalue;
                if (int.TryParse(value.ToString(), CultureInfo.InvariantCulture.NumberFormat, out ivalue))
                    _length = ivalue;
                else return;
            } 
        }


        private double _cost;
        public double CourseCost { get => _cost;
            set
            { 
                double dvalue;
                if (double.TryParse(value.ToString(), CultureInfo.InvariantCulture.NumberFormat, out dvalue))
                    _cost = dvalue;
                else return;   
            }
        }
    }
}
