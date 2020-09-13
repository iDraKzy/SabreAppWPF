using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
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
using System.Data.SQLite;

namespace SabreAppWPF.AddPages
{
    /// <summary>
    /// Logique d'interaction pour AddHomeworkClassroom.xaml
    /// </summary>
    public partial class AddHomeworkClassroom : Page
    {
        private List<ClassroomEntry> classroomEntries = new List<ClassroomEntry>();
        public AddHomeworkClassroom(int? classroomId = null)
        {
            InitializeComponent();

            List<ClassroomInfo> classroomList = Database.Get.Classroom.All("new");
            foreach (ClassroomInfo classroom in classroomList)
            {
                ClassroomEntry classroomEntry = new ClassroomEntry()
                {
                    ID = classroom.ClassroomId,
                    Name = classroom.Name
                };
                classroomEntries.Add(classroomEntry);
            }

            _classroomComboBox.ItemsSource = classroomEntries;

            if (classroomId != null)
            {
                _classroomComboBox.SelectedIndex = (int)classroomId - 1;
            } 
            else
            {
                _classroomComboBox.SelectedIndex = 0;
            }
        }
        

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (_titleTextBox.Text == null)
            {
                error.Foreground = new SolidColorBrush(Colors.Red);
                error.Content = "Intitulé obligatoire";
                return;
            }
            string description = _titleTextBox.Text;
            if (_datePicker.SelectedDate == null)
            {
                error.Foreground = new SolidColorBrush(Colors.Red);
                error.Content = "Date obligatoire";
                return;
            }
            DateTime endDateTime = (DateTime)_datePicker.SelectedDate;
            int endDateTimestamp = (int)new DateTimeOffset(endDateTime).ToUnixTimeSeconds();
            int selectedClassroomId = (int)_classroomComboBox.SelectedValue;
            List<StudentInfo> studentList = Database.Get.Student.AllFromClassroomId(selectedClassroomId);
            DateTime currentDate = DateTime.Now;
            int currentTimestamp = (int)new DateTimeOffset(currentDate).ToUnixTimeSeconds();
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            List<HomeworkInfo> homeworkInfos = new List<HomeworkInfo>();
            
            foreach (StudentInfo student in studentList)
            {
                HomeworkInfo homework = new HomeworkInfo()
                {
                    studentId = student.studentId,
                    description = description,
                    endDate = endDateTimestamp,
                };
                homeworkInfos.Add(homework);
            }

            int generatedHomework = Database.Insert.Homework.Multiple(homeworkInfos);

            error.Foreground = new SolidColorBrush(Colors.Green);
            error.Content = $"{generatedHomework} devoirs ajoutés avec succès";
        }

        public class ClassroomEntry
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }
    }
}
