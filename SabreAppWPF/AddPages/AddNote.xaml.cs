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
using SabreAppWPF.Students.StudentDetails;

namespace SabreAppWPF.AddPages
{
    /// <summary>
    /// Logique d'interaction pour AddNote.xaml
    /// </summary>
    public partial class AddNote : Page
    {
        public AddNote()
        {
            InitializeComponent();
        }

        public AddNote(int studentId)
        {
            InitializeComponent();
            string[] nameArray = Database.Get.Student.NameFromID(studentId);
            _lastnameTextBox.Text = nameArray[0];
            _surnameTextBox.Text = nameArray[1];
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string lastname = _lastnameTextBox.Text;
            string surname = _surnameTextBox.Text;
            string content = _contentTextBox.Text;

            string[] validation = DataValidation.Note(lastname, surname, content);
            if (validation[0] != "valid")
            {
                error.Content = validation[0];
                error.Foreground = new SolidColorBrush(Colors.Red);
            }

            DateTime currentDateTime = DateTime.Now;
            int currentTimestamp = (int)new DateTimeOffset(currentDateTime).ToUnixTimeSeconds();

            int studentId = int.Parse(validation[1]);
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "INSERT INTO notes(studentId, creationDate, content) VALUES(@studentId, @creationDate, @content)";
            cmd.Parameters.AddWithValue("studentId", studentId);
            cmd.Parameters.AddWithValue("creationDate", currentTimestamp);
            cmd.Parameters.AddWithValue("content", content);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT last_insert_rowid()";
            long noteId = (long)cmd.ExecuteScalar();

            NoteDetails noteDetails = new NoteDetails()
            {
                ID = (int)noteId,
                CreationDate = currentDateTime.ToString("g", GlobalVariable.culture),
                Content = content
            };
            NotesDetails.noteCollection.Add(noteDetails);
        }
    }
}
