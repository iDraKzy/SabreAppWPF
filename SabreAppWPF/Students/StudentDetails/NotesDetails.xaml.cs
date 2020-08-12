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
using System.Collections.ObjectModel;
using System.Linq;

namespace SabreAppWPF.Students.StudentDetails
{
    /// <summary>
    /// Logique d'interaction pour NotesDetails.xaml some more comment test
    /// </summary>
    public partial class NotesDetails : Page
    {
        public int studentId;
        public static ObservableCollection<NoteDetails> noteCollection = new ObservableCollection<NoteDetails>();
        public NotesDetails(int studentId)
        {
            InitializeComponent();
            this.studentId = studentId;
            noteCollection = new ObservableCollection<NoteDetails>();
        }

        private void NotesDetails_Load(object sender, RoutedEventArgs e)
        {
            noteDataGrid.ItemsSource = noteCollection;
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = $"SELECT * FROM notes WHERE studentId = {studentId}";
            using SQLiteDataReader rdr = cmd.ExecuteReader();



            while (rdr.Read())
            {
                int creationTimestamp = rdr.GetInt32(2);
                DateTime creationDate = DateTimeOffset.FromUnixTimeSeconds(creationTimestamp).LocalDateTime;
                string creationDateString = creationDate.ToString("g", GlobalVariable.culture);
                NoteDetails noteDetail = new NoteDetails()
                {
                    ID = rdr.GetInt32(0),
                    CreationDate = creationDateString,
                    Content = rdr.GetString(3)
                };
                noteCollection.Add(noteDetail);
            }

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            NoteDetails rowDetails = (NoteDetails)((FrameworkElement)sender).DataContext;
            cmd.CommandText = $"DELETE FROM notes WHERE noteId = {rowDetails.ID}";
            cmd.ExecuteNonQuery();

            noteCollection.Remove(noteCollection.Where(i => i.ID == rowDetails.ID).Single());
        }
    }

    public class NoteDetails
    {
        public int ID { get; set; }
        public string CreationDate { get; set; }
        public string Content { get; set; }

    }
}
