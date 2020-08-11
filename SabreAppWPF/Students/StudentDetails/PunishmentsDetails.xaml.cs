using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Data.SQLite;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SabreAppWPF.Students.StudentDetails
{
    /// <summary>
    /// Logique d'interaction pour PunishmentsDetails.xaml
    /// </summary>
    public partial class PunishmentsDetails : Page
    {
        public static ObservableCollection<PunishmentDetails> punishmentsList = new ObservableCollection<PunishmentDetails>();
        public int studentId;
        public PunishmentsDetails(int studentId)
        {
            InitializeComponent();
            this.studentId = studentId;
            punishmentsList = new ObservableCollection<PunishmentDetails>();
        }
        /// <summary>
        /// Populates punishments list and handles the logic for the initialization of the datagrid
        /// </summary>
        /// <param name="sender"/>
        /// <param name="e"/>
        private void Punishments_Load(object sender, RoutedEventArgs e)
        {
            punishmentDataGrid.ItemsSource = punishmentsList;
            //Get all punishments of this student from the database
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = $"SELECT * FROM punishments WHERE studentId = {studentId}";

            using SQLiteDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                //EndDate and ReturnedDate with DateTime format use in multiple automation later on
                DateTime endDate = DateTimeOffset.FromUnixTimeSeconds(rdr.GetInt32(3)).LocalDateTime;
                DateTime returnedDateTime = DateTimeOffset.FromUnixTimeSeconds(rdr.GetInt32(4)).LocalDateTime;

                //All DateTime to timestamp for later automation
                int currentTimestamp = (int)new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
                int endTimestamp = (int)new DateTimeOffset(endDate).ToUnixTimeSeconds();
                int returnedTimestamp = (int)new DateTimeOffset(returnedDateTime).ToUnixTimeSeconds();

                //Tempenddate for checking if the date is past
                DateTime tempEndDate = endDate.AddDays(1);
                int tempEndTimestamp = (int)new DateTimeOffset(tempEndDate).ToUnixTimeSeconds();

                //background and foreground color string for row color
                string backgroundColor = "Transparent";
                string foregroundColor = "Back";

                //buttonenabled bool
                bool buttonEnabled = true;

                //Handle Returned Time which is empty by default
                string returnedTime = "";
                if (rdr.GetInt32(4) != 0) //Raw int from Returned Date (index 4)
                {
                    buttonEnabled = false;
                    returnedTime = returnedDateTime.ToString("G", GlobalVariable.culture);
                    //Handle row color
                    if (returnedTimestamp > tempEndTimestamp)
                    {
                        backgroundColor = "DarkRed";
                        foregroundColor = "White";
                    }
                }
                else if (currentTimestamp > tempEndTimestamp)
                {
                    backgroundColor = "DarkRed";
                    foregroundColor = "White";
                }

                PunishmentDetails punishmentDetails = new PunishmentDetails()
                {
                    ID = rdr.GetInt32(0),
                    Date = DateTimeOffset.FromUnixTimeSeconds(rdr.GetInt32(2)).LocalDateTime.ToString("g", GlobalVariable.culture),
                    EndDate = endDate.ToString("d", GlobalVariable.culture),
                    Returned = returnedTime,
                    Description = rdr.GetString(5),
                    StatusColor = backgroundColor,
                    ForegroundColor = foregroundColor,
                    ButtonEnabled = buttonEnabled
                };
                punishmentsList.Add(punishmentDetails);
            }
            rdr.Close();

        }

        private void Retrieved_Click(object sender, RoutedEventArgs e)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            PunishmentDetails rowDetails = (PunishmentDetails)((FrameworkElement)sender).DataContext;

            rowDetails.ButtonEnabled = false;

            int currentTimestamp = Convert.ToInt32(new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds());

            cmd.CommandText = $"UPDATE punishments SET retrieveDate = @retrieveDate WHERE punishmentId = @id";
            cmd.Parameters.AddWithValue("retrieveDate", currentTimestamp);
            cmd.Parameters.AddWithValue("id", rowDetails.ID);
            cmd.Prepare();
            cmd.ExecuteNonQuery();


            rowDetails.Returned = DateTime.Now.ToString("G", GlobalVariable.culture);


            DateTime tempEndDate = DateTime.Parse(rowDetails.EndDate);
            tempEndDate = tempEndDate.AddDays(1);
            int endTimestamp = Convert.ToInt32(new DateTimeOffset(tempEndDate).ToUnixTimeSeconds());
            int retrieveTimestamp = Convert.ToInt32(new DateTimeOffset(DateTime.Parse(rowDetails.Returned)).ToUnixTimeSeconds());
        }

    }

    public class PunishmentDetails : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string returned;
        private string statusColor;
        private string foregroundColor;
        private bool buttonEnabled;
        public int ID { get; set; }
        public string Date { get; set; }
        public string EndDate { get; set; }
        public string Returned 
        {
            get { return returned; }
            set
            {
                returned = value;
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
