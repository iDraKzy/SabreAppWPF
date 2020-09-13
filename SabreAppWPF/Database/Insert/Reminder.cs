using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace SabreAppWPF.Database.Insert
{
    public static class Reminder
    {
        public static int One(DateTime dateSelected, string description)
        {
            int currentTimestamp = GlobalFunction.ReturnTimestamp(DateTime.Now);
            int dateSelectedTimestamp = GlobalFunction.ReturnTimestamp(dateSelected);

            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            //reminders(reminderId INTEGER PRIMARY KEY, creationDate INTEGER, reminderDate INTEGER, description TEXT); ";
            cmd.CommandText = "INSERT INTO reminders(creationDate, reminderDate, description, active) VALUES(@creationDate, @reminderDate, @description, true)";
            cmd.Parameters.AddWithValue("creationDate", currentTimestamp);
            cmd.Parameters.AddWithValue("reminderDate", dateSelectedTimestamp);
            cmd.Parameters.AddWithValue("description", description);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT last_insert_rowid()";
            long reminderId = (long)cmd.ExecuteScalar();

            return (int)reminderId;
        }

        public static void One(int date, string description, int creationDate, bool active, int reminderId)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            //reminders(reminderId INTEGER PRIMARY KEY, creationDate INTEGER, reminderDate INTEGER, description TEXT); ";
            cmd.CommandText = "INSERT INTO reminders(reminderId, creationDate, reminderDate, description, active) VALUES(@reminderId, @creationDate, @reminderDate, @description, @active)";
            cmd.Parameters.AddWithValue("reminderId", reminderId);
            cmd.Parameters.AddWithValue("creationDate", creationDate);
            cmd.Parameters.AddWithValue("reminderDate", date);
            cmd.Parameters.AddWithValue("description", description);
            cmd.Parameters.AddWithValue("active", active);

            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
    }
}
