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
using System.Globalization;
using StudentsCourses.Services;
using Command = MvvmHelpers.Commands.Command;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Behaviors;
using System.Collections.Specialized;



  



namespace StudentsCourses.ViewModels
{
    class StudentsViewModel
    {



        public IStudentsCoursesDataStore SqliteDataStore => DependencyService.Get<IStudentsCoursesDataStore>();

        public ObservableRangeCollection<Student> StudentList { get; set; }
        public ObservableRangeCollection<Course> CourseList { get; set; }

        public AsyncCommand PageAppearingCommand { get; set; }
        public ICommand SubmitStudent { get; set; } //+
        public ICommand SubmitCourse { get; set; }//+

        public ICommand DeleteCoursesCommand { get; set; }
        public ICommand DeleteStudentsCommand { get; set; }

        public ICommand SelectCommand { get; }

        public ICommand EditStudent { get; set; }


        public ICommand SubmitStudentChanges { get; set; }

        public ICommand SubmitStudentChangesPressed { get; set; }

        public ICommand ShowStudentFillForm { get; set; }//+
        public ICommand CancelStudent { get; set; }
        public ICommand DeleteStudent { get; set; }


        public ICommand ShowCourseFillForm { get; set; }//+
        public ICommand CancelCourse { get; set; }//+

        public ICommand SubmitCourseChanges { get; set; }//+

        public ICommand EditCourse { get; set; }//+


        public ICommand DeleteCourse { get; set; }//+

        public StudentsViewModel()
        {
            PageAppearingCommand = new AsyncCommand(PageAppearing);
            ShowStudentFillForm = new Command(onShowStudentFillForm);
            SubmitStudent = new Command(onStudentSubmitClicked);
            CancelStudent = new Command(onCancelStudentClicked);
            SubmitCourse = new Command(onSubmitCourseClicked);
            StudentList = new ObservableRangeCollection<Student>();
            CourseList = new ObservableRangeCollection<Course>();
            DeleteCoursesCommand = new Command(onDeleteCourseCommand);
            DeleteStudentsCommand = new Command(onDeleteStudentCommand);

            //EditStudents = new AsyncRelayCommand<StackLayout>(onEditStudents);
            EditStudent = new Command(onEditStudent);



            //EditStudents2 = new AsyncRelayCommand<StackLayout stack>(onEditStudents2);

            SubmitStudentChanges = new Command(onSubmitStudentChanges);
            DeleteStudent = new Command(onDeleteStudent);

            ShowCourseFillForm = new Command(onShowCourseFillForm);

            CancelCourse = new Command(onCancelCourseClicked);

            SubmitCourseChanges = new Command(onSubmitCourseChanges);

            EditCourse = new Command(onEditCourse);

            DeleteCourse = new Command(onDeleteCourse);



        }

        //------------------FILL-STUDENT-FORM-START------------------//
        private void onShowStudentFillForm(object obj)
        {
            //Find the form
            var mainGrid = obj as Grid;
            //var studentFillOutForm = mainGrid.Children[1] as Grid;
            var studentScrollView = mainGrid.Children[0] as ScrollView;
            var studentStackLayout = studentScrollView.Children[0] as StackLayout;
            var studentFillOutForm = studentStackLayout.Children[1] as Grid;
            // If it is not visible do it visible
            if (!studentFillOutForm.IsVisible)
            {
                studentFillOutForm.IsVisible = true;
                //Clear all text of the card 
                var studentEntriesStackFillOutForm = studentFillOutForm.Children[0] as StackLayout;
                (studentEntriesStackFillOutForm.Children[0] as Label).IsVisible = false;
                (studentEntriesStackFillOutForm.Children[1] as Entry).Text = "";
                (studentEntriesStackFillOutForm.Children[2] as Label).IsVisible = false;
                (studentEntriesStackFillOutForm.Children[3] as Entry).Text = "";
                (studentEntriesStackFillOutForm.Children[4] as Label).IsVisible = false;
                (studentEntriesStackFillOutForm.Children[5] as Entry).Text = "";
            }
        }

        private void onCancelStudentClicked(object obj)
        {
            //Find the form
            var filloutForm = (obj as Grid);
            if (filloutForm.IsVisible)
                filloutForm.IsVisible = false;
        }

        private async void onStudentSubmitClicked(object obj)
        {
            try
            {
                //ParseObjects
                var gridStudentFillOutForm = obj as Grid;
                var stackStudentEntries = gridStudentFillOutForm.Children[0] as StackLayout;
                var LabelNameError = stackStudentEntries.Children[0] as Label;
                var LabelSurnameError = stackStudentEntries.Children[2] as Label;
                var LabelGPAError = stackStudentEntries.Children[4] as Label;

                //Name checking 
                if (String.IsNullOrEmpty(_pageStudentName)) { LabelNameError.IsVisible = true; return; }//_pageStudent could be null
                else LabelNameError.IsVisible = false;

                string name = _pageStudentName.Trim();
                if (String.IsNullOrEmpty(name)) { LabelNameError.IsVisible = true; return; }
                else LabelNameError.IsVisible = false;

                //Surname checking
                if (String.IsNullOrEmpty(_pageStudentSurname)) { LabelSurnameError.IsVisible = true; return; }
                else LabelSurnameError.IsVisible = false;
                string surname = _pageStudentSurname.Trim();
                if (String.IsNullOrEmpty(surname)) { LabelSurnameError.IsVisible = true; return; }
                else LabelSurnameError.IsVisible = false;

                //GPA checking
                if (String.IsNullOrEmpty(_pageStudentGPA)) { LabelGPAError.IsVisible = true; return; }
                else LabelGPAError.IsVisible = false;

                float gpa = float.Parse(_pageStudentGPA, CultureInfo.InvariantCulture.NumberFormat);
                if (gpa > 4 || gpa < 0) { LabelGPAError.IsVisible = true; return; }
                else { LabelGPAError.IsVisible = false; }

                //Hide the card
                if (gridStudentFillOutForm.IsVisible)
                    gridStudentFillOutForm.IsVisible = false;

                // Add a new student to a list and DB
                var student = new Student() { StudentName = name, StudentSurname = surname, StudentGPA = gpa };
                await SqliteDataStore.SaveStudentAsync(student);
                //StudentList.Add(student);
                StudentList.Insert(0, student); // new feature
            }
            catch (Exception ex)
            {
                //ParseObjects
                var gridStudentFillOutForm = obj as Grid;
                var stackStudentEntries = gridStudentFillOutForm.Children[0] as StackLayout;
                var LabelNameError = stackStudentEntries.Children[0] as Label;
                var LabelSurnameError = stackStudentEntries.Children[2] as Label;
                var LabelGPAError = stackStudentEntries.Children[4] as Label;
                LabelNameError.IsVisible = true; LabelSurnameError.IsVisible = true; LabelGPAError.IsVisible = true;
                return;
            }
        }

        //------------------FILL-STUDENT-FORM-END---------------------//

        //-------------STUDENT-CARD-MANIPULATIONS-START--------------//
        private void onEditStudent(object obj)
        {
            //find children
            var grid = obj as Grid;
            var stacklayoutEntries = grid.Children[0] as StackLayout;
            var gridImageButtons = grid.Children[1] as Grid;

            //children 1 - StackLayout
            //StackLayout children
            //entries
            var entryStudentName = stacklayoutEntries.Children[1] as Entry;
            var entryStudentSurname = stacklayoutEntries.Children[3] as Entry;
            var entryStudentGPA = stacklayoutEntries.Children[5] as Entry;

            //labels
            var labelStudentNameError = stacklayoutEntries.Children[0] as Label;
            var labelStudentSurnameError = stacklayoutEntries.Children[2] as Label;
            var labelStudentGPAError = stacklayoutEntries.Children[4] as Label;

            //get access for editing
            entryStudentName.IsEnabled = true;
            entryStudentSurname.IsEnabled = true;
            entryStudentGPA.IsEnabled = true;

            //hide labels
            labelStudentNameError.IsVisible = false;
            labelStudentSurnameError.IsVisible = false;
            labelStudentGPAError.IsVisible = false;

            //children 2  - Grid
            //Grid children
            //ImageButtons 
            var SubmitIB = gridImageButtons.Children[0] as ImageButton;
            var EditIB = gridImageButtons.Children[1] as ImageButton;
            var DeleteIB = gridImageButtons.Children[2] as ImageButton;

            //hide edit button 
            EditIB.IsVisible = false;

            //show submit and delete button
            SubmitIB.IsVisible = true;
            DeleteIB.IsVisible = true;

        }

        private async void onSubmitStudentChanges(object obj)
        {
            //find children
            var grid = obj as Grid;
            var stacklayoutEntries = grid.Children[0] as StackLayout;
            var gridImageButtons = grid.Children[1] as Grid;

            //children 1 - StackLayout
            //StackLayout children
            //entries
            var entryStudentName = stacklayoutEntries.Children[1] as Entry;
            var entryStudentSurname = stacklayoutEntries.Children[3] as Entry;
            var entryStudentGPA = stacklayoutEntries.Children[5] as Entry;
            var _entryStudentId = stacklayoutEntries.Children[6] as Entry;

            //labels
            var labelStudentNameError = stacklayoutEntries.Children[0] as Label;
            var labelStudentSurnameError = stacklayoutEntries.Children[2] as Label;
            var labelStudentGPAError = stacklayoutEntries.Children[4] as Label;

            //children 2  - Grid
            //Grid children
            //ImageButtons 
            var SubmitIB = gridImageButtons.Children[0] as ImageButton;
            var EditIB = gridImageButtons.Children[1] as ImageButton;
            var DeleteIB = gridImageButtons.Children[2] as ImageButton;

            //update DB
            var student = StudentList.FirstOrDefault(s => s.StudentId == Convert.ToInt32(_entryStudentId.Text));
            try
            {
                student.StudentName = student.StudentName.Trim();
                student.StudentSurname = student.StudentSurname.Trim();
                if (String.IsNullOrEmpty(student.StudentName))
                {
                    labelStudentNameError.IsVisible = true;
                    return;
                }
                else labelStudentNameError.IsVisible = false;

                if (String.IsNullOrEmpty(student.StudentSurname))
                {
                    labelStudentSurnameError.IsVisible = true;
                    return;
                } else labelStudentSurnameError.IsVisible = false;

                if (student.StudentGPA > 4 || student.StudentGPA < 0)
                {
                    labelStudentGPAError.IsVisible = true;
                    return;
                }
                else labelStudentGPAError.IsVisible = false;
                entryStudentGPA.Text = student.StudentGPA.ToString();
                await SqliteDataStore.UpdateStudentAsync(student);
            }
            catch (Exception ex)
            {
                labelStudentGPAError.IsVisible = true;
                labelStudentNameError.IsVisible = true;
                labelStudentSurnameError.IsVisible = true;
                return;
            }
            //disable access for editing
            entryStudentName.IsEnabled = false;
            entryStudentSurname.IsEnabled = false;
            entryStudentGPA.IsEnabled = false;
            //hide submit and delete buttons
            SubmitIB.IsVisible = false;
            DeleteIB.IsVisible = false;

            //show edit button 
            EditIB.IsVisible = true;
        }

        private async void onDeleteStudent(object obj)
        {
            //find children
            var grid = obj as Grid;
            var stacklayoutEntries = grid.Children[0] as StackLayout;
            var gridImageButtons = grid.Children[1] as Grid;

            //children 1 - StackLayout
            //StackLayout children
            //entries
            var entryStudentName = stacklayoutEntries.Children[1] as Entry;
            var entryStudentSurname = stacklayoutEntries.Children[3] as Entry;
            var entryStudentGPA = stacklayoutEntries.Children[5] as Entry;
            var _entryStudentId = stacklayoutEntries.Children[6] as Entry;

            //labels
            var labelStudentNameError = stacklayoutEntries.Children[0] as Label;
            var labelStudentSurnameError = stacklayoutEntries.Children[2] as Label;
            var labelStudentGPAError = stacklayoutEntries.Children[4] as Label;

            //children 2  - Grid
            //Grid children
            //ImageButtons 
            var SubmitIB = gridImageButtons.Children[0] as ImageButton;
            var EditIB = gridImageButtons.Children[1] as ImageButton;
            var DeleteIB = gridImageButtons.Children[2] as ImageButton;

            //delete from DB
            var student = StudentList.FirstOrDefault(s => s.StudentId == Convert.ToInt32(_entryStudentId.Text));
            try
            {
                await SqliteDataStore.DeleteStudentAsync(student);
                //var students = await SqliteDataStore.GetStudentsAsync();
                //StudentList.Clear();
                //StudentList.AddRange(students);
                //StudentList.RemoveAt(0);
                StudentList.Remove(student);
                var parentBorder = grid.Parent as Border;
                var parentCollectionView = parentBorder.Parent as CollectionView;
                var parentMainStackLayout = parentCollectionView.Parent as StackLayout;

                //var parentMainScrollView = parentMainStackLayout.Parent as IView;
                //parentMainScrollView.InvalidateMeasure();
                parentCollectionView.RemoveLogicalChild(parentBorder);
                //parentCollectionView.ItemsSource.

            }
            catch (Exception ex)
            {
                await Refresh(); //the exception occures on Windows:
                //when a user tries to delete a card that HAS ALREADY DELETED
                return;
            }
            //disable access for editing
            entryStudentName.IsEnabled = false;
            entryStudentSurname.IsEnabled = false;
            entryStudentGPA.IsEnabled = false;
            //hide submit and delete buttons
            SubmitIB.IsVisible = false;
            DeleteIB.IsVisible = false;

            //show edit button 
            EditIB.IsVisible = true;
        }



        //-------------STUDENT-CARD-MANIPULATIONS-END--------------//



        //********************COURSES*SECTION*********************//

        //---------------FILL-COURSE-FORM-START-------------------//       
        private void onShowCourseFillForm(object obj)
        {
            //Find the form
            var mainGrid = obj as Grid;
            var courseScrollView = mainGrid.Children[1] as ScrollView;
            var courseStackLayout = courseScrollView.Children[0] as StackLayout;
            var courseFillOutForm = courseStackLayout.Children[1] as Grid;
            // If it is not visible do it visible
            if (!courseFillOutForm.IsVisible)
            {
                courseFillOutForm.IsVisible = true;
                //Clear all text of the card 
                var courseEntriesStackFillOutForm = courseFillOutForm.Children[0] as StackLayout;
                (courseEntriesStackFillOutForm.Children[0] as Label).IsVisible = false;
                (courseEntriesStackFillOutForm.Children[1] as Entry).Text = "";
                (courseEntriesStackFillOutForm.Children[2] as Label).IsVisible = false;
                (courseEntriesStackFillOutForm.Children[3] as Entry).Text = "";
                (courseEntriesStackFillOutForm.Children[4] as Label).IsVisible = false;
                (courseEntriesStackFillOutForm.Children[5] as Entry).Text = "";
            }
        }



        private async void onSubmitCourseClicked(object obj)
        {
            //input is the intial form as Grid
            try
            {
                //ParseObjects
                var gridCourseFillOutForm = obj as Grid;
                var stackCourseEntries = gridCourseFillOutForm.Children[0] as StackLayout;
                var LabelCourseNameError = stackCourseEntries.Children[0] as Label;
                var LabelLengthError = stackCourseEntries.Children[2] as Label;
                var LabelCostError = stackCourseEntries.Children[4] as Label;

                //Course name checking
                if (String.IsNullOrEmpty(_pageCourseName)) { LabelCourseNameError.IsVisible = true; return; }//_pageStudent could be null
                else LabelCourseNameError.IsVisible = false;

                string name = _pageCourseName.Trim();
                if (String.IsNullOrEmpty(name)) { LabelCourseNameError.IsVisible = true; return; }
                else LabelCourseNameError.IsVisible = false;

                //Length checking
                if (String.IsNullOrEmpty(_pageCourseLength)) { LabelLengthError.IsVisible = true; return; }
                else LabelLengthError.IsVisible = false;
                //try to parse
                int length;
                if (int.TryParse(_pageCourseLength, out length))
                {//parsing successful
                    if (length < 0) { LabelLengthError.IsVisible = true; return; }
                    else LabelLengthError.IsVisible = false;
                }
                else
                {
                    LabelLengthError.IsVisible = true;
                    return;
                }

                //cost checking
                if (String.IsNullOrEmpty(_pageCourseCost)) { LabelCostError.IsVisible = true; return; }
                else LabelCostError.IsVisible = false;
                //try to parse
                double cost;
                if (double.TryParse(_pageCourseCost, CultureInfo.InvariantCulture.NumberFormat, out cost))
                {
                    if (cost < 0) { LabelCostError.IsVisible = true; return; }
                    else { LabelCostError.IsVisible = false; }
                }
                else
                {
                    LabelCostError.IsVisible = true;
                    return;
                }

                //Hide the card
                if (gridCourseFillOutForm.IsVisible)
                    gridCourseFillOutForm.IsVisible = false;

                // Add a new course to a list and DB
                var course = new Course() { CourseName = name, CourseLength = length, CourseCost = cost };
                await SqliteDataStore.SaveCourseAsync(course);
                CourseList.Insert(0, course); // new feature
            }
            catch (Exception ex)
            {
                //ParseObjects
                var gridCourseFillOutForm = obj as Grid;
                var stackCourseEntries = gridCourseFillOutForm.Children[0] as StackLayout;
                var LabelCourseNameError = stackCourseEntries.Children[0] as Label;
                var LabelLengthError = stackCourseEntries.Children[2] as Label;
                var LabelCostError = stackCourseEntries.Children[4] as Label;
                LabelCourseNameError.IsVisible = true; LabelLengthError.IsVisible = true; LabelCostError.IsVisible = true;
                return;
            }
        }


        private void onCancelCourseClicked(object obj)
        {
            //Find the form
            var filloutForm = (obj as Grid);
            if (filloutForm.IsVisible)
                filloutForm.IsVisible = false;
        }

        //----------------FILL-COURSE-FORM-END-------------------//




        //-------------COURSE-CARD-MANIPULATIONS-START------------//
        private async void onSubmitCourseChanges(object obj)
        {
            //find children
            var grid = obj as Grid;
            var stacklayoutEntries = grid.Children[0] as StackLayout;
            var gridImageButtons = grid.Children[1] as Grid;

            //children 1 - StackLayout
            //StackLayout children
            //entries
            var entryCourseName = stacklayoutEntries.Children[1] as Entry;
            var entryCourseLength = stacklayoutEntries.Children[3] as Entry;
            var entryCourseCost = stacklayoutEntries.Children[5] as Entry;
            var _entryCourseId = stacklayoutEntries.Children[6] as Entry;

            //labels
            var labelCourseNameError = stacklayoutEntries.Children[0] as Label;
            var labelCourseLengthError = stacklayoutEntries.Children[2] as Label;
            var labelCourseCostError = stacklayoutEntries.Children[4] as Label;

            //children 2  - Grid
            //Grid children
            //ImageButtons 
            var SubmitIB = gridImageButtons.Children[0] as ImageButton;
            var EditIB = gridImageButtons.Children[1] as ImageButton;
            var DeleteIB = gridImageButtons.Children[2] as ImageButton;

            //update DB
            var course = CourseList.FirstOrDefault(s => s.CourseId == Convert.ToInt32(_entryCourseId.Text));
            try
            {
                //naming checking 
                course.CourseName = course.CourseName.Trim();                
                if (String.IsNullOrEmpty(course.CourseName))
                {
                    labelCourseNameError.IsVisible = true;
                    return;
                }
                else labelCourseNameError.IsVisible = false;

                //length checking
                if(course.CourseLength < 0)
                {
                    labelCourseLengthError.IsVisible = true;
                    return;
                }
                else
                    labelCourseLengthError.IsVisible = false;

                //cost checking
                if (course.CourseCost < 0) 
                {
                    labelCourseCostError.IsVisible = true;
                    return;
                }
                else labelCourseCostError.IsVisible = false;

                entryCourseCost.Text = course.CourseCost.ToString();
                entryCourseLength.Text = course.CourseLength.ToString();
                await SqliteDataStore.UpdateCourseAsync(course);
            }
            catch (Exception ex)
            {
                labelCourseNameError.IsVisible = true;
                labelCourseLengthError.IsVisible = true;
                labelCourseCostError.IsVisible = true;
                return;
            }
            //disable access for editing
            entryCourseCost.IsEnabled = false;
            entryCourseLength.IsEnabled = false;
            entryCourseName.IsEnabled = false;
            //hide submit and delete buttons
            SubmitIB.IsVisible = false;
            DeleteIB.IsVisible = false;

            //show edit button 
            EditIB.IsVisible = true;
        }


        private void onEditCourse(object obj)
        {
            //find children
            var grid = obj as Grid;
            var stacklayoutEntries = grid.Children[0] as StackLayout;
            var gridImageButtons = grid.Children[1] as Grid;

            //children 1 - StackLayout
            //StackLayout children
            //entries
            var entryCourseName = stacklayoutEntries.Children[1] as Entry;
            var entryCourseLength = stacklayoutEntries.Children[3] as Entry;
            var entryCourseCost = stacklayoutEntries.Children[5] as Entry;

            //labels
            var labelCourseNameError = stacklayoutEntries.Children[0] as Label;
            var labelCourseLengthError = stacklayoutEntries.Children[2] as Label;
            var labelCourseCostError = stacklayoutEntries.Children[4] as Label;

            //get access for editing
            entryCourseName.IsEnabled = true;
            entryCourseLength.IsEnabled = true;
            entryCourseCost.IsEnabled = true;

            //hide labels
            labelCourseNameError.IsVisible = false;
            labelCourseLengthError.IsVisible = false;
            labelCourseCostError.IsVisible = false;

            //children 2  - Grid
            //Grid children
            //ImageButtons 
            var SubmitIB = gridImageButtons.Children[0] as ImageButton;
            var EditIB = gridImageButtons.Children[1] as ImageButton;
            var DeleteIB = gridImageButtons.Children[2] as ImageButton;

            //hide edit button 
            EditIB.IsVisible = false;

            //show submit and delete button
            SubmitIB.IsVisible = true;
            DeleteIB.IsVisible = true;

        }

        private async void onDeleteCourse(object obj)
        {
            //find children
            var grid = obj as Grid;
            var stacklayoutEntries = grid.Children[0] as StackLayout;
            var gridImageButtons = grid.Children[1] as Grid;

            //children 1 - StackLayout
            //StackLayout children
            //entries
            var entryCourseName = stacklayoutEntries.Children[1] as Entry;
            var entryCourseLength = stacklayoutEntries.Children[3] as Entry;
            var entryCourseCost = stacklayoutEntries.Children[5] as Entry;
            var _entryCourseId = stacklayoutEntries.Children[6] as Entry;

            //labels
            var labelCourseNameError = stacklayoutEntries.Children[0] as Label;
            var labelCourseLengthError = stacklayoutEntries.Children[2] as Label;
            var labelCourseCostError = stacklayoutEntries.Children[4] as Label;

            //children 2  - Grid
            //Grid children
            //ImageButtons 
            var SubmitIB = gridImageButtons.Children[0] as ImageButton;
            var EditIB = gridImageButtons.Children[1] as ImageButton;
            var DeleteIB = gridImageButtons.Children[2] as ImageButton;

            //delete DB
            var course = CourseList.FirstOrDefault(s => s.CourseId == Convert.ToInt32(_entryCourseId.Text));
            try
            {
                await SqliteDataStore.DeleteCourseAsync(course);
                CourseList.Remove(course);
                var parentBorder = grid.Parent as Border;
                if(parentBorder == null) {
                    var b = 4 + 4;
                }
                var parentCollectionView = parentBorder.Parent as CollectionView;
                if (parentCollectionView == null)
                {
                    var c = 3 + 3;
                }

                //parentCollectionView.RemoveLogicalChild(parentBorder);
               
                //CollectionViewSource.GetDefaultView(CourseList).Refresh();

                //var parentMainStackLayout = parentCollectionView.Parent as StackLayout;
                //parentCollectionView.RemoveLogicalChild(parentBorder);
            }
            catch (Exception ex)
            {
                await Refresh();
                return;
            }
            //disable access for editing
            entryCourseCost.IsEnabled = false;
            entryCourseLength.IsEnabled = false;
            entryCourseName.IsEnabled = false;
            //hide submit and delete buttons
            SubmitIB.IsVisible = false;
            DeleteIB.IsVisible = false;

            //show edit button 
            EditIB.IsVisible = true;
        }

        //-------------COURSE-CARD-MANIPULATIONS-END------------//














        private async void onDeleteStudentCommand(object obj) {
            Student student = obj as Student;
            int index = StudentList.IndexOf(student);
            if (index < 0) return;
            //remove from DB
            await SqliteDataStore.DeleteStudentAsync(student);
            //remove from collection
            StudentList.RemoveAt(index);

            var students = await SqliteDataStore.GetStudentsAsync();
            int quantity = students.Count();
            if (quantity < 1) _ = SqliteDataStore.RedefinedStudentsPK();
        }


        private async void onDeleteCourseCommand(object obj)
        {
            Course course = obj as Course;
            int index = CourseList.IndexOf(course);
            if (index < 0) return;
            //remove from DB
            await SqliteDataStore.DeleteCourseAsync(course);
            //remove from collection
            CourseList.RemoveAt(index);

            var courses = await SqliteDataStore.GetCoursesAsync();
            int quantity = courses.Count();
            if (quantity < 1) _ = SqliteDataStore.RedefinedCoursesPK();
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

        private string _pageStudentId;
        public string EntryStudentId
        {
            set
            {
                if (_pageStudentId != value)
                {
                    _pageStudentId = value;
                }
            }
        }
        private string _pageStudentName;
        public string EntryStudentName {             
            set
            {
                if (_pageStudentName != value)
                {
                    _pageStudentName = value;                  
                }
            }
        }

        private string _pageStudentSurname;
        public string EntryStudentSurname
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
