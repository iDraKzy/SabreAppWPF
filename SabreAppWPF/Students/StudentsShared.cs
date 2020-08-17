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
        /// <summary>
        /// Add a vote to the db
        /// </summary>
        /// <param name="studentId">Id of the student</param>
        /// <param name="vote">true = upvote, false = downvote</param>
        /// <param name="description">Description of the vote</param>
        public static void AddVotesToDb(int studentId, bool vote, string description)
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

    }
}
