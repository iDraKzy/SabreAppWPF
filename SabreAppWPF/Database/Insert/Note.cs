using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using SabreAppWPF.Students.StudentDetails;

namespace SabreAppWPF.Database.Insert
{
    public static class Note
    {
        public static int One(int studentId, string content)
        {
            int currentTimestamp = GlobalFunction.ReturnTimestamp(DateTime.Now);

            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "INSERT INTO notes(studentId, creationDate, content, active) VALUES(@studentId, @creationDate, @content, true)";
            cmd.Parameters.AddWithValue("studentId", studentId);
            cmd.Parameters.AddWithValue("creationDate", currentTimestamp);
            cmd.Parameters.AddWithValue("content", content);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT last_insert_rowid()";
            long noteId = (long)cmd.ExecuteScalar();

            return (int)noteId;
        }

        public static void One(int studentId, string content, int creationDate, bool active, int noteId)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "INSERT INTO notes(noteId, studentId, creationDate, content, active) VALUES(@noteId, @studentId, @creationDate, @content, @active)";
            cmd.Parameters.AddWithValue("noteId", noteId);
            cmd.Parameters.AddWithValue("studentId", studentId);
            cmd.Parameters.AddWithValue("creationDate", creationDate);
            cmd.Parameters.AddWithValue("content", content);
            cmd.Parameters.AddWithValue("active", active);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
    }
}
