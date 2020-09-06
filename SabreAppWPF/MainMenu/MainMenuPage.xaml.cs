using System;
using System.Collections.Generic;
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
using Windows.Devices.PointOfService;
using SabreAppWPF.AddPages;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Automation.Peers;
using System.Collections.ObjectModel;

namespace SabreAppWPF.MainMenu
{
    /// <summary>
    /// Logique d'interaction pour MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuPage : Page, INotifyPropertyChanged
    {
        public static ObservableCollection<ReminderGridDisplay> reminderGridDisplayCollection;
        public static ObservableCollection<ScheduleGridDisplay> scheduleGridDisplayCollection;
        private string _nextSessionTime;
        private string _nextSessionClassroom;
        private int _classroomId;
        private bool _listEnabled = true;
        public MainMenuPage()
        {
            InitializeComponent();
            this.DataContext = this;
            reminderGridDisplayCollection = new ObservableCollection<ReminderGridDisplay>();
            scheduleGridDisplayCollection = new ObservableCollection<ScheduleGridDisplay>();

            Populate_ScheduleDataGrid();
            Populate_ReminderDataGrid();
        }

        private void MainMenuPage_Load(object sender, RoutedEventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            int currentTimeStamp = (int)new DateTimeOffset(currentTime).ToUnixTimeSeconds();
            List<ScheduleInfo> scheduleInfoList = Database.Get.Schedule.All();
            if (scheduleInfoList.Count == 0)
            {
                NextSessionTime = "Aucune session prévue";
                ListEnabled = false;
                return;
            }
            int scheduleIndex = 0;
            int timeSelected = int.MaxValue;
            for (int i = 0; i < scheduleInfoList.Count; i++)
            {
                DateTime nextDateTimeTemp = DateTimeOffset.FromUnixTimeSeconds((long)scheduleInfoList[i].nextDate).LocalDateTime;
                DateTime nextDateTimeTempWithSec = nextDateTimeTemp.AddSeconds((double)scheduleInfoList[i].duration);
                int nextDateTimestampWithSec = (int)new DateTimeOffset(nextDateTimeTempWithSec).ToUnixTimeSeconds();
                //TODO: IF time duration past update nextDate recursively until reach non past one IMPORTANT
                if (nextDateTimestampWithSec < currentTimeStamp)
                {
                    UpdateNextDate((int)scheduleInfoList[i].scheduleId, nextDateTimeTemp, (int)scheduleInfoList[i].repetitivity);
                    continue;
                }
                if (scheduleInfoList[i].nextDate < timeSelected)
                {
                    timeSelected = (int)scheduleInfoList[i].nextDate;
                    scheduleIndex = i;
                }
            }

            int finishedTime = timeSelected + (int)scheduleInfoList[scheduleIndex].duration;
            DateTime nextDateTime = DateTimeOffset.FromUnixTimeSeconds(timeSelected).LocalDateTime;
            string nextScheduleString = nextDateTime.ToString("g", GlobalVariable.culture);
            NextSessionTime = nextScheduleString ?? "Aucun cours prévu";
            NextSessionClassroom = $"Classe : {Database.Get.Classroom.NameFromID((int)scheduleInfoList[scheduleIndex].classroomId)} -  Salle : {Database.Get.Room.NameFromID((int)scheduleInfoList[scheduleIndex].roomId)}";
            ClassroomId = (int)scheduleInfoList[scheduleIndex].classroomId;
            if (currentTimeStamp > finishedTime)
            {
                UpdateNextDate((int)scheduleInfoList[scheduleIndex].scheduleId, nextDateTime, (int)scheduleInfoList[scheduleIndex].repetitivity);
                MainMenuPage_Load(sender, e);
            }
        }

        private void UpdateNextDate(int scheduleId, DateTime nextDateTime, int repetitivity)
        {
            int daysToNextSession = 7 * (repetitivity + 1);
            DateTime nextSessionDateTime = nextDateTime.AddDays(daysToNextSession);
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "UPDATE schedules SET nextDate = @nextDate WHERE scheduleId = @scheduleId";
            cmd.Parameters.AddWithValue("nextDate", (int)new DateTimeOffset(nextSessionDateTime).ToUnixTimeSeconds());
            cmd.Parameters.AddWithValue("scheduleId", scheduleId);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        private void Populate_ScheduleDataGrid()
        {
            List<ScheduleInfo> scheduleList = Database.Get.Schedule.All();
            
            foreach (ScheduleInfo schedule in scheduleList)
            {
                DateTime scheduleDateTime = DateTimeOffset.FromUnixTimeSeconds((int)schedule.nextDate).LocalDateTime;
                ScheduleGridDisplay scheduleDisplay = new ScheduleGridDisplay()
                {
                    ID = (int)schedule.scheduleId,
                    Classroom = Database.Get.Classroom.NameFromID((int)schedule.classroomId),
                    Date = scheduleDateTime.ToString("g", GlobalVariable.culture),
                    Room = Database.Get.Room.NameFromID((int)schedule.roomId)
                };
                scheduleGridDisplayCollection.Add(scheduleDisplay);
            }
        }

        private void Populate_ReminderDataGrid()
        {
            //TODO: Implement
            reminderGridDisplayCollection.Add(new ReminderGridDisplay() { Content = "Work in progress" });
        }

        private void AddSchedule_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = GlobalFunction.GetMainWindow();
            window._addFrame.Navigate(new AddSchedules());
        }

        private void ClassroomList_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = GlobalFunction.GetMainWindow();
            window._mainFrame.Navigate(new studentsPage(ClassroomId));
        }




        //Binding Property

        public class ReminderGridDisplay : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            private int _id;
            private string _date;
            private string _classroom;
            private string _content;

            public int ID
            {
                get { return _id; }
                set
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }

            public string Date
            {
                get { return _date; }
                set
                {
                    _date = value;
                    OnPropertyChanged();
                }
            }

            public string Classroom
            {
                get { return _classroom; }
                set
                {
                    _classroom = value;
                    OnPropertyChanged();
                }
            }

            public string Content
            {
                get { return _content; }
                set
                {
                    _content = value;
                    OnPropertyChanged();
                }
            }

            protected void OnPropertyChanged([CallerMemberName] string name = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        public class ScheduleGridDisplay : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            private int _id;
            private string _date;
            private string _classroom;
            private string _room;

            public int ID
            {
                get { return _id; }
                set
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }

            public string Date
            {
                get { return _date; }
                set
                {
                    _date = value;
                    OnPropertyChanged();
                }
            }

            public string Classroom
            {
                get { return _classroom; }
                set
                {
                    _classroom = value;
                    OnPropertyChanged();
                }
            }

            public string Room
            {
                get { return _room; }
                set
                {
                    _room = value;
                    OnPropertyChanged();
                }
            }

            protected void OnPropertyChanged([CallerMemberName] string name = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        public string NextSessionTime
        {
            get { return _nextSessionTime; }
            set
            {
                _nextSessionTime = value;
                OnPropertyChanged();
            }
        }
        public string NextSessionClassroom
        {
            get { return _nextSessionClassroom; }
            set
            {
                _nextSessionClassroom = value;
                OnPropertyChanged();
            }
        }

        public int ClassroomId
        {
            get { return _classroomId; }
            set
            {
                _classroomId = value;
                OnPropertyChanged();
            }
        }

        public bool ListEnabled
        {
            get { return _listEnabled; }
            set
            {
                _listEnabled = value;
                OnPropertyChanged();
            }
        }
        //INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
