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
using Windows.UI.Xaml.Automation.Peers;
using System.Data.SQLite;

namespace SabreAppWPF.AddPages
{
    /// <summary>
    /// Logique d'interaction pour AddHomeworkStudent.xaml
    /// </summary>
    public partial class AddHomeworkStudent : Page
    {
        public AddHomeworkStudent(int studentId)
        {
            InitializeComponent();

            string[] name = Database.Get.Student.NameFromID(studentId);
            _lastnameTextBox.Text = name[0];
            _surnameTextBox.Text = name[1];
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            int currentTimestamp = (int)new DateTimeOffset(currentTime).ToUnixTimeSeconds();
            string lastname = _lastnameTextBox.Text;
            //TODO: Datavalidation cleaner
            if (lastname == null)
            {
                error.Foreground = new SolidColorBrush(Colors.Red);
                error.Content = "Nom obligatoire";
                return;
            }
            string surname = _surnameTextBox.Text;
            if (surname == null)
            {
                error.Foreground = new SolidColorBrush(Colors.Red);
                error.Content = "Prénom obligatoire";
                return;
            }
            int studentId = Database.Get.Student.IdFromName(lastname, surname);
            if (studentId == 0)
            {
                error.Foreground = new SolidColorBrush(Colors.Red);
                error.Content = "Etudiant(e) introuvable";
                return;
            }
            string description = _titleTextBox.Text;
            if (description == null)
            {
                error.Foreground = new SolidColorBrush(Colors.Red);
                error.Content = "Intitulé obligatoire";
                return;
            }
            if (_datePicker.SelectedDate == null)
            {
                error.Foreground = new SolidColorBrush(Colors.Red);
                error.Content = "Date obligaoire";
                return;
            }
            DateTime endDateTime = (DateTime)_datePicker.SelectedDate;
            int endDateTimestamp = (int)new DateTimeOffset(endDateTime).ToUnixTimeSeconds();
            //homeworks(homeworkId INTEGER PRIMARY KEY, studentId INTEGER, creationDate INTEGER, endDate INTEGER, retrieveDate INTEGER, description TEXT);
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "INSERT INTO homeworks(studentId, creationDate, endDate, retrieveDate, description) VALUES(@studentId, @creationDate, @endDate, 0, @description)";
            cmd.Parameters.AddWithValue("studentId", studentId);
            cmd.Parameters.AddWithValue("creationDate", currentTimestamp);
            cmd.Parameters.AddWithValue("endDate", endDateTimestamp);
            cmd.Parameters.AddWithValue("description", description);
        }
    }
}
