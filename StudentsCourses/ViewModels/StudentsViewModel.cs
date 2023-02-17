using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelpers.Commands;
using MvvmHelpers;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using StudentsCourses.Models;

namespace StudentsCourses.ViewModels
{
    class StudentsViewModel
    {
        public ObservableRangeCollection<Student> StudentList { get; set; }
        public AsyncCommand PageAppearingCommand { get; set; }
        public ICommand SubmitStudent { get; set; }


        public StudentsViewModel()
        {
            PageAppearingCommand = new AsyncCommand(PageAppearing);
            SubmitStudent = new MvvmHelpers.Commands.Command( () => onStudentSubmitClicked());
            StudentList = new ObservableRangeCollection<Student>();
        }

        public async Task Refresh()
        {
            //when the page starting
            //
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

        
        private void onStudentSubmitClicked()
        {
            string name = _pageStudentName;
            string surname = _pageStudentSurname;
            var student = new Student(StudentList.Count(), name, surname);
            StudentList.Add(student);
        }


    }
}
