using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace SabreAppWPF.Database.Insert
{
    public static class Plan
    {
        public static int One(int scheduleId, int roomId, string spacing, string name)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "INSERT INTO plans(scheduleId, roomId, spacing, name) VALUES(@scheduleId, @roomId, @spacing, @name)";
            cmd.Parameters.AddWithValue("scheduleId", scheduleId);
            cmd.Parameters.AddWithValue("roomId", roomId);
            cmd.Parameters.AddWithValue("spacing", spacing);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.CommandText = "SELECT last_insert_rowid()";
            long planId = (long)cmd.ExecuteScalar();
            return (int)planId;
        }

        public static void One(int scheduleId, int roomId, string spacing, string name, int planId)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "INSERT INTO plans(planId, scheduleId, roomId, spacing, name) VALUES(@planId, @scheduleId, @roomId, @spacing, @name)";
            cmd.Parameters.AddWithValue("planId", planId);
            cmd.Parameters.AddWithValue("scheduleId", scheduleId);
            cmd.Parameters.AddWithValue("roomId", roomId);
            cmd.Parameters.AddWithValue("spacing", spacing);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
    }
}
