using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace SabreAppWPF.Database.Insert
{
    public static class Punishment
    {
        public static int One(int studentId, DateTime endDate, string description)
        {
            int currentTimestamp = GlobalFunction.ReturnTimestamp(DateTime.Now);
            int endDateTimestamp = GlobalFunction.ReturnTimestamp(endDate);

            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "INSERT INTO punishments(studentId, creationDate, endDate, retrieveDate, description, active) VALUES(@studentId, @creationDate, @endDate, 0, @description, true)";
            cmd.Parameters.AddWithValue("studentId", studentId);
            cmd.Parameters.AddWithValue("creationDate", currentTimestamp);
            cmd.Parameters.AddWithValue("endDate", endDateTimestamp);
            cmd.Parameters.AddWithValue("description", description);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT last_insert_rowid()";
            long punishmentId = (long)cmd.ExecuteScalar();

            return (int)punishmentId;
        }
    }
}
