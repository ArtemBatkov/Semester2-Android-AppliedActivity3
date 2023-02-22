﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelpers.Commands;
using MvvmHelpers;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using StudentsCourses.Models;
using System.Globalization;
using StudentsCourses.Services;

namespace StudentsCourses.ViewModels
{
    class StudentsViewModel
    {

        public IStudentsCoursesDataStore SqliteDataStore => DependencyService.Get<IStudentsCoursesDataStore>();

        public ObservableRangeCollection<Student> StudentList { get; set; }
        public ObservableRangeCollection<Course> CourseList { get; set; }

        public AsyncCommand PageAppearingCommand { get; set; }
        public ICommand SubmitStudent { get; set; }
        public ICommand SubmitCourse { get; set; }

        public StudentsViewModel()
        {
            PageAppearingCommand = new AsyncCommand(PageAppearing);
            SubmitStudent = new MvvmHelpers.Commands.Command( () => onStudentSubmitClicked());
            SubmitCourse = new MvvmHelpers.Commands.Command(() => onCourseSubmitClicked());
            StudentList = new ObservableRangeCollection<Student>();
            CourseList = new ObservableRangeCollection<Course>();
        }

        public async Task Refresh()
        {
            //when the page starting
            //

            StudentList.Clear();
            var students = await SqliteDataStore.GetStudentsAsync();
            StudentList.AddRange(students);

            CourseList.Clear();
            var courses = await SqliteDataStore.GetCoursesAsync();
            CourseList.AddRange(courses);

        }



        public async Task PageAppearing()
        {
            //page apearing is linked with the refresh relates to the page
            await Refresh();
        }
        private string _pageStudentName;
        public string EntryName {             
            set
            {
                if (_pageStudentName != value)
                {
                    _pageStudentName = value;                  
                }
            }
        }

        private string _pageStudentSurname;
        public string EntrySurname
        {
            set
            {
                if (_pageStudentSurname != value)
                {
                    _pageStudentSurname = value;
                }
            }
        }

        private string _pageStudentGPA;
        public string EntryStudentGPA
        {
            set
            {
                if (_pageStudentGPA != value)
                {
                    _pageStudentGPA = value;
                }
            }
        }

        // Courses
        private string _pageCourseName;
        public string EntryCourseName
        {
            set
            {
                if(_pageCourseName != value)
                {
                    _pageCourseName = value;
                }
            }
        }

        private string _pageCourseLength;
        public string  EntryCourseLength
        {
            set
            {
                if (_pageCourseLength != value)
                {
                    _pageCourseLength = value;
                }
            }
        }

        private string _pageCourseCost;
        public string EntryCourseCost
        {
            set
            {
                if (_pageCourseCost != value)
                {
                    _pageCourseCost = value;
                }
            }
        }

        

        private async void onStudentSubmitClicked()
        {
            try
            {
                string name = _pageStudentName;
                string surname = _pageStudentSurname;
                float gpa = float.Parse(_pageStudentGPA, CultureInfo.InvariantCulture.NumberFormat);
                if (String.IsNullOrEmpty(surname)) return;
                if (String.IsNullOrEmpty(name)) return;
                if (gpa > 4 || gpa < 0) return;
                var student = new Student() { StudentName = name, StudentSurname = surname, StudentGPA = gpa };
                StudentList.Add(student);

                await SqliteDataStore.SaveStudentAsync(student);
            }
            catch(Exception ex) { return; }
        }


        private async void onCourseSubmitClicked()
        {
            try { 
                string name = _pageCourseName;
                int length =  Int32.Parse(_pageCourseLength);
                double cost = Double.Parse(_pageCourseCost, CultureInfo.InvariantCulture.NumberFormat);
                if(String.IsNullOrEmpty(name)) return;
                if (length <= 0) return;
                if(cost < 0) return;
                var course = new Course() { CourseName = name, CourseLength = length, CourseCost = cost };
                CourseList.Add(course);

                await SqliteDataStore.SaveCourseAsync(course);
            }
            catch(Exception ex)
            {
                return;
            }
        }

    }
}
