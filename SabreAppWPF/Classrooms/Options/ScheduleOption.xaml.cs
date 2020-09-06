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

namespace SabreAppWPF.Classrooms.Options
{
    /// <summary>
    /// Logique d'interaction pour ScheduleOption.xaml
    /// </summary>
    public partial class ScheduleOption : Page
    {
        public static ObservableCollection<ScheduleOptionDisplay> scheduleDisplayCollection;
        //schedules(scheduleId INTEGER PRIMARY KEY, classroomId INTEGER, roomId INTEGER, repetitivity INTEGER, nextDate INTEGER, duration INTEGER);
        public ScheduleOption(int classroomId)
        {
            InitializeComponent();
            scheduleDisplayCollection = new ObservableCollection<ScheduleOptionDisplay>();
            List<ScheduleInfo> scheduleList = Database.Get.Schedule.AllFromClassroomId(classroomId);

            foreach (ScheduleInfo schedule in scheduleList)
            {
                DateTime nextDateTime = DateTimeOffset.FromUnixTimeSeconds((long)schedule.nextDate).LocalDateTime;
                TimeSpan durationTime = TimeSpan.FromSeconds((double)schedule.duration);

                ScheduleOptionDisplay scheduleDisplay = new ScheduleOptionDisplay()
                {
                    ID = (int)schedule.scheduleId,
                    ClassroomId = (int)schedule.classroomId,
                    Room = Database.Get.Room.NameFromID((int)schedule.roomId),
                    Repetitivity = schedule.repetitivity == 0 ? "Une fois par semaine" : "Une semaine sur deux",
                    NextDate = nextDateTime.ToString("g", GlobalVariable.culture),
                    Duration = durationTime.ToString(@"hh\:mm")
                };
                scheduleDisplayCollection.Add(scheduleDisplay);
            }
            _scheduleDataGrid.ItemsSource = scheduleDisplayCollection;
        }

        private void CreatePlanButton_Click(object sender, RoutedEventArgs e)
        {
            ScheduleOptionDisplay schedule = (ScheduleOptionDisplay)((Button)sender).DataContext;
            MainWindow window = GlobalFunction.GetMainWindow();

            window._mainFrame.Navigate(new Plans.PlanEditPage(schedule.ID, schedule.ClassroomId));
        }

        public class ScheduleOptionDisplay : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            private int _id;
            private int _classroomId;
            private string _roomName;
            private string _repetitivity;
            private string _nextDate;
            private string _duration;

            public int ID
            {
                get { return _id; }
                set
                {
                    _id = value;
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

            public string Room
            {
                get { return _roomName; }
                set
                {
                    _roomName = value;
                    OnPropertyChanged();
                }
            }

            public string Repetitivity
            {
                get { return _repetitivity; }
                set
                {
                    _repetitivity = value;
                    OnPropertyChanged();
                }
            }

            public string NextDate
            {
                get { return _nextDate; }
                set
                {
                    _nextDate = value;
                    OnPropertyChanged();
                }
            }

            public string Duration
            {
                get { return _duration; }
                set
                {
                    _duration = value;
                    OnPropertyChanged();
                }
            }

            protected void OnPropertyChanged([CallerMemberName] string name = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
