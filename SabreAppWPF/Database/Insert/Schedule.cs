using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace SabreAppWPF.Database.Insert
{
    public static class Schedule
    {
        public static int One(int classroomId, int roomId, int repetitivity, DateTime date, TimeSpan duration)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "INSERT INTO schedules(classroomId, roomId, repetitivity, nextDate, duration) VALUES(@classroomId, @roomId, @repetitivity, @nextDate, @duration)";
            cmd.Parameters.AddWithValue("classroomId", classroomId);
            cmd.Parameters.AddWithValue("roomId", roomId);
            cmd.Parameters.AddWithValue("repetitivity", repetitivity);
            cmd.Parameters.AddWithValue("nextDate", GlobalFunction.ReturnTimestamp(date));
            cmd.Parameters.AddWithValue("duration", (int)duration.TotalSeconds);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT last_insert_rowid()";
            long scheduleId = (long)cmd.ExecuteScalar();

            return (int)scheduleId;
        }

        public static void One(int classroomId, int roomId, int repetitivity, int date, int duration, int scheduleId)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "INSERT INTO schedules(scheduleId, classroomId, roomId, repetitivity, nextDate, duration) VALUES(@scheduleId, @classroomId, @roomId, @repetitivity, @nextDate, @duration)";
            cmd.Parameters.AddWithValue("scheduleId", scheduleId);
            cmd.Parameters.AddWithValue("classroomId", classroomId);
            cmd.Parameters.AddWithValue("roomId", roomId);
            cmd.Parameters.AddWithValue("repetitivity", repetitivity);
            cmd.Parameters.AddWithValue("nextDate", date);
            cmd.Parameters.AddWithValue("duration", duration);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
    }
}
