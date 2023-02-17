using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsCourses.Models
{
    class Course
    {
        private int CourseId { get; set; }
        private string Name { get; set; }

        public Course(int id, string name )
        {
            this.CourseId = id;
            this.Name = name;
        }

        public int getCourseId => CourseId;
        public string getName() => Name;

    }
}
