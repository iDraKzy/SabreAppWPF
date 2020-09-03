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

namespace SabreAppWPF.MainMenu
{
    /// <summary>
    /// Logique d'interaction pour MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuPage : Page, INotifyPropertyChanged
    {
        private string _nextSessionTime;
        private string _nextSessionClassroom;
        private int _classroomId;
        private bool _listEnabled = true;
        public MainMenuPage()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void MainMenuPage_Load(object sender, RoutedEventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            int currentTimeStamp = (int)new DateTimeOffset(currentTime).ToUnixTimeSeconds();
            List<ScheduleInfo> scheduleInfoList = Getter.GetAllSchedules();
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
                //TODO: IF time duration past update nextDate recursively until reach non past one IMPORTANT
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
            NextSessionClassroom = $"Classe : {Getter.GetClassrommNameFromID((int)scheduleInfoList[scheduleIndex].classroomId)} -  Salle : {Getter.GetRoomNameFromID((int)scheduleInfoList[scheduleIndex].roomId)}";
            ClassroomId = (int)scheduleInfoList[scheduleIndex].classroomId;
            if (currentTimeStamp > finishedTime)
            {
                int daysToNextSession = 7 * ((int)scheduleInfoList[scheduleIndex].repetitivity + 1);
                DateTime nextSessionDateTime = nextDateTime.AddDays(daysToNextSession);
                using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
                cmd.CommandText = "UPDATE schedules SET nextDate = @nextDate WHERE scheduleId = @scheduleId";
                cmd.Parameters.AddWithValue("nextDate", (int)new DateTimeOffset(nextSessionDateTime).ToUnixTimeSeconds());
                cmd.Parameters.AddWithValue("scheduleId", scheduleInfoList[scheduleIndex].scheduleId);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                MainMenuPage_Load(sender, e);
            }
        }

        private void Populate_ScheduleDataGrid()
        {

        }

        private void Populate_ReminderDataGrid()
        {

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
