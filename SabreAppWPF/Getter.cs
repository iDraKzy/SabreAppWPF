using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace SabreAppWPF
{
    public static class Getter
    {
        public static string GetClassrommNameFromID(int classroomId)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "SELECT name FROM classrooms WHERE classroomId = @classroomId";
            cmd.Parameters.AddWithValue("classroomId", classroomId);
            cmd.Prepare();
            string classroomName = (string)cmd.ExecuteScalar();
            return classroomName;
        }

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
        /// <summary>
        /// Return the id of a student based on his name
        /// </summary>
        /// <param name="lastname"></param>
        /// <param name="surname"></param>
        /// <returns>studentId</returns>
        public static int GetStudentIdFromName(string lastname, string surname)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "SELECT studentId FROM students WHERE lastname = @lastname AND surname = @surname";
            cmd.Parameters.AddWithValue("lastname", lastname);
            cmd.Parameters.AddWithValue("surname", surname);
            cmd.Prepare();
            long studentId = (long)cmd.ExecuteScalar();

            return (int)studentId;
        }
    }
}
