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
