using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SabreAppWPF.AddPages;
using SabreAppWPF.Classrooms.Options;

namespace SabreAppWPF.Classrooms
{
    /// <summary>
    /// Logique d'interaction pour ClassroomsOptions.xaml
    /// </summary>
    public partial class ClassroomsOptions : Page, INotifyPropertyChanged
    {
        //need to contazin schedule list from the classroom and allow plan creation
        private int classroomId;
        public ClassroomsOptions(int classroomId)
        {
            InitializeComponent();
            this.classroomId = classroomId;
            this.DataContext = this;

            Name = Database.Get.Classroom.NameFromID(classroomId);
            List<StudentInfo> studentList = Database.Get.Student.AllFromClassroomId(classroomId);
            int studentCount = studentList.Count;
            StudentNumber = $"{studentCount} étudiant(e)s";
        }

        private void ScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            _optionsFrame.Navigate(new ScheduleOption(classroomId));
            MainWindow window = GlobalFunction.GetMainWindow();
            window._addFrame.Navigate(new AddSchedules());
        }


        //Binding of the page
        private string _name;
        private string _studentNumberText;
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string StudentNumber
        {
            get { return _studentNumberText; }
            set
            {
                _studentNumberText = value;
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
