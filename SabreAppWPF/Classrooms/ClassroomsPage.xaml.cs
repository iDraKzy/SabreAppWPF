using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Data.SQLite;
using SabreAppWPF.AddPages;

namespace SabreAppWPF.Classrooms
{
    /// <summary>
    /// Logique d'interaction pour ClassroomsPage.xaml
    /// </summary>
    public partial class ClassroomsPage : Page
    {
        public static ObservableCollection<ClassroomDisplay> classroomCollection;
        public ClassroomsPage()
        {
            classroomCollection = new ObservableCollection<ClassroomDisplay>();
            InitializeComponent();
            classroomList.ItemsSource = classroomCollection;
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "SELECT * FROM classrooms";

            using SQLiteDataReader rdr = cmd.ExecuteReader();
            //TODO: Rework this to work with nextDate
            while (rdr.Read())
            {
                int classroomId = rdr.GetInt32(0);
                List<ScheduleInfo> scheduleClassroomList = ClassroomsShared.GetSchedulesFromClassroomId(classroomId);
                string scheduleTime;

                int dayToSessionTemp = 14;
                ScheduleInfo selectedSchedule = new ScheduleInfo();
                if (scheduleClassroomList.Count != 0)
                {
                    for (int i = 0; i < scheduleClassroomList.Count; i++)
                    {
                        int daysToNextSession = ClassroomsShared.GetNumberOfDaysToNextSession(scheduleClassroomList[i]);
                        if (daysToNextSession < dayToSessionTemp)
                        {
                            selectedSchedule = scheduleClassroomList[i];
                            dayToSessionTemp = daysToNextSession;
                        }
                    }
                    int nextSessionTimestamp = (int)selectedSchedule.nextDate;
                    DateTime nextSessionDateTime = DateTimeOffset.FromUnixTimeSeconds(nextSessionTimestamp).LocalDateTime;
                    scheduleTime = nextSessionDateTime.ToString("g", GlobalVariable.culture);
                } 
                else
                {
                    scheduleTime = "Aucun horaire défini";
                }


                int studentNumber = GetStudentNumber(classroomId);
                string studentNumberString = studentNumber.ToString() + " étudiant(e)s";
                ClassroomDisplay classroomDisplay = new ClassroomDisplay()
                {
                    ID = classroomId,
                    ClassroomName = rdr.GetString(1),
                    StudentsNumber = studentNumberString,
                    NextSession = scheduleTime,
                };
                classroomCollection.Add(classroomDisplay);
            }
        }

        private void StudentsListButton_Click(object sender, RoutedEventArgs e)
        {
            ClassroomDisplay classroomDisplay = (ClassroomDisplay)((FrameworkElement)sender).DataContext;
            MainWindow window = GlobalFunction.GetMainWindow();
            window._mainFrame.Navigate(new studentsPage(classroomDisplay.ID));
        }

        private void ClassroomOptionsButton_Click(object sender, RoutedEventArgs e)
        {
            ClassroomDisplay classroomDisplay = (ClassroomDisplay)((FrameworkElement)sender).DataContext;
            MainWindow window = GlobalFunction.GetMainWindow();
            window._mainFrame.Navigate(new ClassroomsOptions(classroomDisplay.ID));
            window._addFrame.Navigate(new AddHomeworkClassroom(classroomDisplay.ID));
        }

        private int GetStudentNumber(int classroomId)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "SELECT * FROM linkStudentToClassroom WHERE classroomId = @classroomId";
            cmd.Parameters.AddWithValue("classroomId", classroomId);
            cmd.Prepare();
            using SQLiteDataReader rdr = cmd.ExecuteReader();
            int studentNumber = 0;
            while (rdr.Read())
            {
                studentNumber++;
            }
            rdr.Close();
            return studentNumber;
        }
    }

    public class ClassroomDisplay : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _id;
        private string _classroomName;
        private string _studentsNumber;
        private string _nextSession;
        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }
        public string ClassroomName
        {
            get { return _classroomName; }
            set
            {
                _classroomName = value;
                OnPropertyChanged();
            }
        }
        public string StudentsNumber
        {
            get { return _studentsNumber; }
            set
            {
                _studentsNumber = value;
                OnPropertyChanged();
            }
        }
        public string NextSession
        {
            get { return _nextSession; }
            set
            {
                _nextSession = value;
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
