using System;
using System.Collections.Generic;
using System.Text;
using Windows.Devices.AllJoyn;
using System.Data.SQLite;

namespace SabreAppWPF.Database.Get
{
    public static class Reminder
    {
        public static List<ReminderInfo> All(string type)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            List<ReminderInfo> reminderList = new List<ReminderInfo>();
            cmd.CommandText = "SELECT * FROM reminders";
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                ReminderInfo reminder = new ReminderInfo()
                {
                    ReminderId = rdr.GetInt32(0),
                    CreationDate = rdr.GetInt32(1),
                    ReminderDate = rdr.GetInt32(2),
                    Description = rdr.GetString(3)
                };
                reminderList.Add(reminder);
            }
            return reminderList;
        }
    }
}
