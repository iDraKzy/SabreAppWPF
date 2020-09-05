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
using Windows.Globalization;

namespace SabreAppWPF.AddPages
{
    /// <summary>
    /// Logique d'interaction pour AddVote.xaml
    /// </summary>
    public partial class AddVote : Page
    {
        //TODO: Quick description
        private bool upvote;
        private StudentDisplay studentDisplay;
        public AddVote(bool upvote, StudentDisplay studentDisplay)
        {
            InitializeComponent();
            this.upvote = upvote;
            this.studentDisplay = studentDisplay;
            _titleLabel.Content = upvote ? "Ajouter un upvote" : "Ajouter un downvote";
            string[] studentName = Database.Get.Student.NameFromID(studentDisplay.ID);
            _lastnameTextBox.Text = studentName[0];
            _surnameTextBox.Text = studentName[1];
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            //votes(voteId INTEGER PRIMARY KEY, studentId INTEGER, upvotes BOOLEAN, description TEXT, creationDate INTEGER)
            string lastname = _lastnameTextBox.Text;
            string surname = _surnameTextBox.Text;
            int studentId = Database.Get.Student.IdFromName(lastname, surname);
            string description = _descriptionTextBox.Text;
            DateTime currentDateTime = DateTime.Now;
            int currentTimestamp = (int)new DateTimeOffset(currentDateTime).ToUnixTimeSeconds();

            string valid = DataValidation.Vote(lastname, surname, description);
            if (valid != "valid")
            {
                error.Foreground = new SolidColorBrush(Colors.Red);
                error.Content = valid;
                return;
            }

            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "INSERT INTO votes(studentId, upvotes, description, creationDate) VALUES(@studentId, @upvotes, @description, @creationDate)";
            cmd.Parameters.AddWithValue("studentId", studentId);
            cmd.Parameters.AddWithValue("upvotes", upvote);
            cmd.Parameters.AddWithValue("description", description);
            cmd.Parameters.AddWithValue("creationDate", currentTimestamp);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            error.Foreground = new SolidColorBrush(Colors.Green);
            string upvoteType = upvote ? "Upvote" : "Downvote";
            error.Content = $"{upvoteType} ajouté avec succès à {currentDateTime:hh:mm:ss}";
            if (upvote)
            {
                int currentUpvote = int.Parse(studentDisplay.UpvotesCount);
                currentUpvote++;
                studentDisplay.UpvotesCount = currentUpvote.ToString();
            } 
            else
            {
                int currentDownvote = int.Parse(studentDisplay.DownvotesCount);
                currentDownvote++;
                studentDisplay.DownvotesCount = currentDownvote.ToString();
            }
        }
    }
}
