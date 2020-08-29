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

namespace SabreAppWPF.MainMenu
{
    /// <summary>
    /// Logique d'interaction pour MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuPage : Page
    {
        public MainMenuPage()
        {
            InitializeComponent();
        }

        private void MainMenuPage_Load(object sender, RoutedEventArgs e)
        {
            DateTime currentDay = DateTime.Now;
            List<ScheduleInfo> scheduleInfoList = Getter.GetAllSchedules();
            int scheduleIndex = 0;
            int currentTime = int.MaxValue;
            for (int i = 0; i < scheduleInfoList.Count; i++)
            {
                if (scheduleInfoList[i].nextDate < currentTime)
                {
                    currentTime = (int)scheduleInfoList[i].nextDate;
                    scheduleIndex = i;
                }
            }

            DateTime nextDateTime = DateTimeOffset.FromUnixTimeSeconds((long)currentTime).LocalDateTime;
            string nextScheduleString = nextDateTime.ToString("g", GlobalVariable.culture);
            if (currentDay.Hour == nextDateTime.Hour)
            {
                int daysToNextSession = 7 * ((int)scheduleInfoList[scheduleIndex].repetitivity + 1);
                DateTime nextSessionDateTime = nextDateTime.AddDays(daysToNextSession);
                using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
                cmd.CommandText = "UPDATE schedules SET nextDate = @nextDate WHERE scheduleId = @scheduleId";
                cmd.Parameters.AddWithValue("nextDate", (int)new DateTimeOffset(nextSessionDateTime).ToUnixTimeSeconds());
                cmd.Parameters.AddWithValue("scheduleId", scheduleInfoList[scheduleIndex].scheduleId);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
