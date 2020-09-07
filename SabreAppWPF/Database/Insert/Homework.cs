using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data;
using Windows.UI.Composition;

namespace SabreAppWPF.Database.Insert
{
    public static class Homework
    {
        private static string insertString = "INSERT INTO homeworks(studentId, creationDate, endDate, retrieveDate, description, active) VALUES(@studentId, @creationDate, @endDate, 0, @description, true)";
        /// <summary>
        /// Insert an homework into the database
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="endDateTime"></param>
        /// <param name="description"></param>
        /// <returns>The id of the created homework</returns>
        public static int One(int studentId, DateTime endDateTime, string description)
        {
            int currentTimestamp = GlobalFunction.ReturnTimestamp(DateTime.Now);
            int endDateTimestamp = GlobalFunction.ReturnTimestamp(endDateTime);

            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = insertString;
            cmd.Parameters.AddWithValue("studentId", studentId);
            cmd.Parameters.AddWithValue("creationDate", currentTimestamp);
            cmd.Parameters.AddWithValue("endDate", endDateTimestamp);
            cmd.Parameters.AddWithValue("description", description);

            cmd.CommandText = "SELECT last_insert_rowid()";
            long homeworkId = (long)cmd.ExecuteScalar();

            return (int)homeworkId;
        }
        /// <summary>
        /// Create multiple homeworks in the database
        /// </summary>
        /// <param name="homeworkList"></param>
        /// <returns>The number of generated homeworks</returns>
        public static int Multiple(List<HomeworkInfo> homeworkList)
        {
            int currentTimestamp = GlobalFunction.ReturnTimestamp(DateTime.Now);

            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = insertString;

            int homeworkAddedCount = 0;

            foreach (HomeworkInfo homework in homeworkList)
            {
                cmd.Parameters.AddWithValue("studentId", homework.studentId);
                cmd.Parameters.AddWithValue("creationDate", currentTimestamp);
                cmd.Parameters.AddWithValue("endDate", homework.endDate);
                cmd.Parameters.AddWithValue("description", homework.description);
                cmd.Prepare();
                int rowImpacted = cmd.ExecuteNonQuery();
                homeworkAddedCount += rowImpacted;
                cmd.Parameters.Clear();

            }

            return homeworkAddedCount;
        }
    }
}
