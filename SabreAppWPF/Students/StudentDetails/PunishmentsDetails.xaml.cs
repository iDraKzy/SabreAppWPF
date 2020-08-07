using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Data.SQLite;

namespace SabreAppWPF.Students.StudentDetails
{
    /// <summary>
    /// Logique d'interaction pour PunishmentsDetails.xaml
    /// </summary>
    public partial class PunishmentsDetails : Page
    {
        public List<PunishmentDetails> punishmentsList;
        public PunishmentsDetails(int studentId)
        {
            InitializeComponent();
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = $"SELECT * FROM punishments WHERE studentId = {studentId}";

            punishmentsList = new List<PunishmentDetails>();

            using SQLiteDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                //EndDate with DateTime format use in multiple automation later on
                DateTime endDate = DateTimeOffset.FromUnixTimeSeconds(rdr.GetInt32(3)).LocalDateTime;

                //color string for row color
                string color = "Transparent";

                //buttonenabled bool
                bool buttonEnabled = true;

                //Handle Returned Time which is empty by default
                string returnedTime = "";
                if (rdr.GetInt32(4) != 0)
                {
                    buttonEnabled = false;
                    DateTime returnedDateTime = DateTimeOffset.FromUnixTimeSeconds(rdr.GetInt32(4)).LocalDateTime;
                    returnedTime = returnedDateTime.ToString("G", GlobalVariable.culture);
                    //Handle row color
                    int endTimestamp = Convert.ToInt32(new DateTimeOffset(endDate).ToUnixTimeSeconds());
                    int returnedTimestamp = Convert.ToInt32(new DateTimeOffset(returnedDateTime).ToUnixTimeSeconds());
                    if (returnedTimestamp > endTimestamp) color = "DarkRed";
                }

                PunishmentDetails punishmentDetails = new PunishmentDetails()
                {
                    ID = rdr.GetInt32(0),
                    Date = DateTimeOffset.FromUnixTimeSeconds(rdr.GetInt32(2)).LocalDateTime.ToString("g", GlobalVariable.culture),
                    EndDate = endDate.ToString("d", GlobalVariable.culture),
                    Returned = returnedTime,
                    Description = rdr.GetString(5),
                    StatusColor = color,
                    ButtonEnabled = buttonEnabled
                };
                punishmentsList.Add(punishmentDetails);
            }
            rdr.Close();

            punishmentDataGrid.ItemsSource = punishmentsList;
        }

        private void Retrieved_Click(object sender, RoutedEventArgs e)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            PunishmentDetails rowDetails = (PunishmentDetails)((FrameworkElement)sender).DataContext;

            rowDetails.ButtonEnabled = false;

            int currentTimestamp = Convert.ToInt32(new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds());

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
            if (retrieveTimestamp > endTimestamp)
            {
                rowDetails.StatusColor = "DarkRed";
            }

            punishmentDataGrid.ItemsSource = null;
            punishmentDataGrid.ItemsSource = punishmentsList;
        }

    }

    public class PunishmentDetails
    {
        public int ID { get; set; }
        public string Date { get; set; }
        public string EndDate { get; set; }
        public string Returned { get; set; }
        public string Description { get; set; }
        public string StatusColor { get; set; }
        public bool ButtonEnabled { get; set; }
    }
}
