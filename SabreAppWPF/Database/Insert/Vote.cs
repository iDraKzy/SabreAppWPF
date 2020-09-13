using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace SabreAppWPF.Database.Insert
{
    public static class Vote
    {
        public static int One(int studentId, bool upvote, string description)
        {
            int currentTimestamp = GlobalFunction.ReturnTimestamp(DateTime.Now);

            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "INSERT INTO votes(studentId, upvotes, description, creationDate, active) VALUES(@studentId, @upvotes, @description, @creationDate, true)";
            cmd.Parameters.AddWithValue("studentId", studentId);
            cmd.Parameters.AddWithValue("upvotes", upvote);
            cmd.Parameters.AddWithValue("description", description);
            cmd.Parameters.AddWithValue("creationDate", currentTimestamp);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT last_insert_rowid()";
            long voteId = (long)cmd.ExecuteScalar();

            return (int)voteId;
        }

        public static void One(int studentId, bool upvote, string description, int creationDate, bool active, int voteId)
        {

            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "INSERT INTO votes(voteId, studentId, upvotes, description, creationDate, active) VALUES(@voteId, @studentId, @upvotes, @description, @creationDate, @active)";
            cmd.Parameters.AddWithValue("voteId", voteId);
            cmd.Parameters.AddWithValue("studentId", studentId);
            cmd.Parameters.AddWithValue("upvotes", upvote);
            cmd.Parameters.AddWithValue("description", description);
            cmd.Parameters.AddWithValue("creationDate", creationDate);
            cmd.Parameters.AddWithValue("active", active);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
    }
}
