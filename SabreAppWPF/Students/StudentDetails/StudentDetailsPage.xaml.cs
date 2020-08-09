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
using System.Windows.Shapes;
using System.Data.SQLite;

namespace SabreAppWPF.Students.StudentDetails
{
    /// <summary>
    /// Logique d'interaction pour StudentDetailsPage.xaml
    /// </summary>
    public partial class StudentDetailsPage : Page
    {
        public int studentId;
        public MainWindow window = GlobalFunction.GetMainWindow();
        public StudentDetailsPage(int studentId)
        {
            InitializeComponent();
            this.studentId = studentId;

            string studentName = "";
            int classroomId = 1;
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = $"SELECT name, classroomId FROM students WHERE studentId = {studentId}";
            using SQLiteDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                studentName = rdr.GetString(0);
                classroomId = rdr.GetInt32(1);
            }
            rdr.Close();
            cmd.CommandText = $"SELECT name FROM classrooms WHERE classroomId = {classroomId}";
            string classroomName = (string)cmd.ExecuteScalar();

            name.Content = studentName;
            classroom.Content = classroomName;
            _detailsFrame.Navigate(new PunishmentsDetails(studentId));
        }

        private void PunishmentButton_Click(object sender, RoutedEventArgs e)
        {
            _detailsFrame.Navigate(new PunishmentsDetails(studentId));
            window._addFrame.Navigate(new AddTemplate("punishment", studentId));
        }

        private void NotesButton_Click(object sender, RoutedEventArgs e)
        {

            window._addFrame.Navigate(new AddTemplate("note", studentId));
        }
    }
}
