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
using System.Net.NetworkInformation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SabreAppWPF.Students.StudentDetails
{
    /// <summary>
    /// Logique d'interaction pour HomeworksDetails.xaml
    /// </summary>
    public partial class HomeworksDetails : Page
    {
        private int studentId;
        public static ObservableCollection<HomeworkDetails> homeworkDetailsCollection = new ObservableCollection<HomeworkDetails>();
        public HomeworksDetails(int studentId)
        {
            InitializeComponent();
            this.studentId = studentId;
            homeworkDetailsCollection = new ObservableCollection<HomeworkDetails>();
        }

        private void HomeworksDetails_Load(object sender, RoutedEventArgs e)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = $"SELECT * FROM homeworks WHERE studentId = {studentId}";
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            homeworkDataGrid.ItemsSource = homeworkDetailsCollection;

            while (rdr.Read())
            {

                //All timestamp needed for further operation
                int currentTimestamp = (int)new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
                int creationDateTimestamp = rdr.GetInt32(2);
                int endDateTimestamp = rdr.GetInt32(3);
                int retrieveDateTimestamp = rdr.GetInt32(4);

                //All datetime format
                DateTime currentDateTime = DateTimeOffset.FromUnixTimeSeconds(currentTimestamp).LocalDateTime;
                DateTime creationDateTime = DateTimeOffset.FromUnixTimeSeconds(creationDateTimestamp).LocalDateTime;
                DateTime endDateTime = DateTimeOffset.FromUnixTimeSeconds(endDateTimestamp).LocalDateTime;

                //tempEndDate for lateness calculation
                DateTime tempEndDate = endDateTime.AddDays(1);
                int tempEndDateTimestamp = (int)new DateTimeOffset(tempEndDate).ToUnixTimeSeconds();

                string statusColor = "Transparent";
                string foregoundColor = "Black";

                bool buttonEnabled = true;

                string retrieveDate = "";
                if (retrieveDateTimestamp == 0)
                {
                    if (tempEndDateTimestamp > currentTimestamp)
                    {
                        statusColor = "DarkRed";
                        foregoundColor = "White";
                    }
                } else
                {
                    if (tempEndDateTimestamp > retrieveDateTimestamp)
                    {
                        statusColor = "Darkred";
                        foregoundColor = "White";
                    }
                    DateTime retrieveDateTime = DateTimeOffset.FromUnixTimeSeconds(retrieveDateTimestamp).LocalDateTime;
                    retrieveDate = retrieveDateTime.ToString("g", GlobalVariable.culture);
                    buttonEnabled = false;
                }
                HomeworkDetails homeworkDetails = new HomeworkDetails()
                {
                    ID = rdr.GetInt32(0),
                    CreationDate = creationDateTime.ToString("g", GlobalVariable.culture),
                    EndDate = endDateTime.ToString("d", GlobalVariable.culture),
                    RetrieveDate = retrieveDate,
                    Description = rdr.GetString(5),
                    StatusColor = statusColor,
                    ForegroundColor = foregoundColor,
                    ButtonEnabled = buttonEnabled
                };
                homeworkDetailsCollection.Add(homeworkDetails);
            }
        }

        private void RetrieveButton_Click(object sender, RoutedEventArgs e)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            HomeworkDetails rowDetails = (HomeworkDetails)((FrameworkElement)sender).DataContext;

            DateTime currentDateTime = DateTime.Now;
            int currentTimestamp = (int)new DateTimeOffset(currentDateTime).ToUnixTimeSeconds();

            cmd.CommandText = "UPDATE homeworks SET retrieveDate = @retrieveDate WHERE homeworkId = @id";
            cmd.Parameters.AddWithValue("retrieveDate", currentTimestamp);
            cmd.Parameters.AddWithValue("id", rowDetails.ID);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            rowDetails.RetrieveDate = currentDateTime.ToString("g", GlobalVariable.culture);
            rowDetails.ButtonEnabled = false;
        }
    }
    public class HomeworkDetails : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string retrieveDate;
        private string statusColor;
        private string foregroundColor;
        private bool buttonEnabled;
        public int ID { get; set; }
        public string CreationDate { get; set; }
        public string EndDate { get; set; }
        public string RetrieveDate 
        { 
            get { return retrieveDate; }
            set
            {
                retrieveDate = value;
                OnPropertyChanged();
            }
        }
        public string Description { get; set; }
        public string StatusColor 
        {
            get { return statusColor; }
            set
            {
                statusColor = value;
                OnPropertyChanged();
            }
        }
        public string ForegroundColor
        { 
            get { return foregroundColor; } 
            set
            {
                foregroundColor = value;
                OnPropertyChanged();
            }
        }
        public bool ButtonEnabled 
        { 
            get { return buttonEnabled; }
            set
            {
                buttonEnabled = value;
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
