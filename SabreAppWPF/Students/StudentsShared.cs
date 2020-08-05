using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace SabreAppWPF.Students
{
    public static class StudentsShared
    {
        private static void AddVotesToDb(int studentId, bool vote, string description)
        {
            using SQLiteConnection connection = new SQLiteConnection("Data Source=" + GlobalVariable.path);
            connection.Open();
            using SQLiteCommand cmd = new SQLiteCommand(connection)
            {
                CommandText = "INSERT INTO votes(studentId, upvotes, description, creationDate) VALUES(@studentId, @upvotes, @description, @creationDate)"
            };
            cmd.Parameters.AddWithValue("studentId", studentId);
            cmd.Parameters.AddWithValue("upvotes", vote);
            cmd.Parameters.AddWithValue("description", description);
            cmd.Parameters.AddWithValue("creationDate", Convert.ToInt32(new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()));
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public static void AddStudentToUI(studentsPage page, int studentId, string name, string classroom, string lastNote, int upvotes, int downvotes)
        {
            //Create newStudent
            student newStudent = new student();

            //Handle texts
            newStudent.studentName.Content = name;
            newStudent.studentClassroom.Content = classroom ?? "Classe";
            newStudent.studentNote.Text = lastNote ?? "Note par défaut";

            //Handle votesText
            newStudent.studentUpvote.Content = upvotes.ToString();
            newStudent.studentDownvote.Content = downvotes.ToString();

            //Handle upvoteButton
            newStudent.upvoteButton.Tag = studentId;
            newStudent.upvoteButton.Click += (object s, RoutedEventArgs e) =>
            {
                int studentId = (int)((Button)s).Tag;
                int score = int.Parse((string)newStudent.studentUpvote.Content);
                score++;
                newStudent.studentUpvote.Content = score.ToString();
                AddVotesToDb(studentId, true, "Upvote rapide");
            };

            //Handle downvoteButton
            newStudent.downvoteButton.Tag = studentId;
            newStudent.downvoteButton.Click += (s, e) =>
            {
                int studentId = (int)((Button)s).Tag;
                int score = int.Parse((string)newStudent.studentDownvote.Content);
                score++;
                newStudent.studentDownvote.Content = score.ToString();
                AddVotesToDb(studentId, false, "Downvote rapide");
            };

            newStudent.punishButton.Tag = studentId;
            newStudent.punishButton.Click += (s, e) =>
            {
                int studentId = (int)((Button)s).Tag;
                MainWindow window = GlobalFunction.GetMainWindow();
                window._addFrame.Navigate(new AddTemplate("punishment", studentId));
                //window._mainFrame.Navigate(new StudentDetail(studentId));
            };

            page.studentListPanel.Children.Add(newStudent);
        }
    }
}
