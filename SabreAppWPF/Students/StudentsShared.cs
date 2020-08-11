using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using SabreAppWPF.Students.StudentDetails;
using System.Windows.Media;

namespace SabreAppWPF.Students
{
    public static class StudentsShared
    {
        private static HomeworkInfo GetLastHomework(List<HomeworkInfo> homeworksList)
        {
            HomeworkInfo lastHomework = new HomeworkInfo();
            foreach (HomeworkInfo homework in homeworksList)
            {
                if (lastHomework.creationDate < homework.creationDate)
                {
                    lastHomework = homework;
                }
            }
            return lastHomework;
        }

        //private static void SwitchHomeworkStatus(student newStudent, HomeworkInfo lastHomework, string wantedStatus)
        //{
        //    switch(wantedStatus)
        //    {
        //        case "✓":
        //            newStudent.lastHomeworkStatus.Content = "✓";
        //            newStudent.lastHomeworkStatus.Foreground = new SolidColorBrush(Colors.Green);
        //            newStudent.lastHomeworkStatus.Margin = new Thickness(5, 0, 0, 0);
        //            newStudent.lastHomeworkStatus.FontWeight = FontWeights.Bold;
        //            break;
        //        case "❌":
        //            newStudent.lastHomeworkStatus.Content = "❌";
        //            newStudent.lastHomeworkStatus.Foreground = new SolidColorBrush(Colors.Red);
        //            newStudent.lastHomeworkStatus.Margin = new Thickness(0);
        //            newStudent.lastHomeworkStatus.FontWeight = FontWeights.Normal;
        //            break;
        //    }
        //}
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
            cmd.Parameters.AddWithValue("creationDate", Convert.ToInt32(new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()));
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public static void AddStudentToUI(studentsPage page, int studentId, string name, string classroom, string lastNote, int upvotes, int downvotes)
        {
            //using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            //cmd.CommandText = $"SELECT * FROM homeworks WHERE studentId = {studentId}";
            //using SQLiteDataReader rdr = cmd.ExecuteReader();

            //List<HomeworkInfo> homeworksList = new List<HomeworkInfo>();

            //while (rdr.Read())
            //{
            //    HomeworkInfo homeworkInfo = new HomeworkInfo()
            //    {
            //        homeworkId = rdr.GetInt32(0),
            //        studentId = rdr.GetInt32(1),
            //        creationDate = rdr.GetInt32(2),
            //        endDate = rdr.GetInt32(3),
            //        retrieveDate = rdr.GetInt32(4),
            //        description = rdr.GetString(5)
            //    };
            //    homeworksList.Add(homeworkInfo);
            //}

            //HomeworkInfo lastHomework = GetLastHomework(homeworksList);

            ////Create newStudent
            //student newStudent = new student();

            ////Handle texts
            //newStudent.studentName.Content = name;
            //newStudent.studentClassroom.Content = classroom ?? "Classe";
            //newStudent.studentNote.Text = lastNote ?? "Note par défaut";

            ////Handle lasthomework
            //if (lastHomework.retrieveDate != 0 || lastHomework.homeworkId == 0)
            //{
            //    SwitchHomeworkStatus(newStudent, lastHomework, "✓");
            //    newStudent.lastHomeworkButton.IsEnabled = false;
            //} else
            //{
            //    SwitchHomeworkStatus(newStudent, lastHomework, "❌");
            //}

            ////Handle votesText
            //newStudent.studentUpvote.Content = upvotes.ToString();
            //newStudent.studentDownvote.Content = downvotes.ToString();

            ////Handle upvoteButton
            //newStudent.upvoteButton.Tag = studentId;
            //newStudent.upvoteButton.Click += (object s, RoutedEventArgs e) =>
            //{
            //    int studentId = (int)((Button)s).Tag;
            //    int score = int.Parse((string)newStudent.studentUpvote.Content);
            //    score++;
            //    newStudent.studentUpvote.Content = score.ToString();
            //    AddVotesToDb(studentId, true, "Upvote rapide");
            //};

            ////Handle downvoteButton
            //newStudent.downvoteButton.Tag = studentId;
            //newStudent.downvoteButton.Click += (s, e) =>
            //{
            //    int studentId = (int)((Button)s).Tag;
            //    int score = int.Parse((string)newStudent.studentDownvote.Content);
            //    score++;
            //    newStudent.studentDownvote.Content = score.ToString();
            //    AddVotesToDb(studentId, false, "Downvote rapide");
            //};

            //newStudent.punishButton.Tag = studentId;
            //newStudent.punishButton.Click += (s, e) =>
            //{
            //    int studentId = (int)((Button)s).Tag;
            //    MainWindow window = GlobalFunction.GetMainWindow();
            //    window._addFrame.Navigate(new AddTemplate("punishment", studentId));
            //    //window._mainFrame.Navigate(new StudentDetail(studentId));
            //};

            //newStudent.detailButton.Tag = studentId;
            //newStudent.detailButton.Click += (s, e) =>
            //{
            //    int studentId = (int)((Button)s).Tag;
            //    MainWindow window = GlobalFunction.GetMainWindow();
            //    window._mainFrame.Navigate(new StudentDetailsPage(studentId));
            //};

            //newStudent.lastHomeworkButton.Tag = lastHomework.homeworkId;
            //newStudent.lastHomeworkButton.Click += (s, e) =>
            //{
            //    using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            //    int currentTimestamp = (int)new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
            //    int lastHomeworkId = (int)((Button)s).Tag;
            //    cmd.CommandText = $"UPDATE homeworks SET retrieveDate = {currentTimestamp} WHERE homeworkId = {lastHomeworkId}";
            //    cmd.ExecuteNonQuery();
            //    //SwitchHomeworkStatus(newStudent, lastHomework, "✓");
            //};

            ////page.studentListPanel.Children.Add(newStudent);
        }
        /// <summary>
        /// Returns the name and lastname of a student based on his id
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns>string[lastname, surname]</returns>
        public static string[] GetStudentNameFromID(int studentId)
        {
            string[] nameArray = new string[2];
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = $"SELECT lastname, surname FROM students WHERE studentId = {studentId}";
            using SQLiteDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                nameArray[0] = rdr.GetString(0);
                nameArray[1] = rdr.GetString(1);
            }

            return nameArray;
        }
    }
}
