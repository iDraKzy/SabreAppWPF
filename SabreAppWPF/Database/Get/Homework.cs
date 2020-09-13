using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace SabreAppWPF.Database.Get
{
    public static class Homework
    {
        public static List<HomeworkInfo> All(string type)
        {
            string source = (type == "old") ? Update.oldPath : GlobalVariable.path;
            using SQLiteConnection connection = new SQLiteConnection("Data Source=" + source);
            connection.Open();
            using SQLiteCommand cmd = new SQLiteCommand(connection);
            List<HomeworkInfo> homeworks = new List<HomeworkInfo>();

            cmd.CommandText = "SELECT * FROM homeworks";
            using SQLiteDataReader rdr = cmd.ExecuteReader();
            //homeworks(homeworkId INTEGER PRIMARY KEY, studentId INTEGER, creationDate INTEGER, endDate INTEGER, retrieveDate INTEGER, description TEXT, active BOOLEAN);
            while (rdr.Read())
            {
                HomeworkInfo homework = new HomeworkInfo()
                {
                    homeworkId = rdr.GetInt32(0),
                    studentId = rdr.GetInt32(1),
                    creationDate = rdr.GetInt32(2),
                    endDate = rdr.GetInt32(3),
                    retrieveDate = rdr.GetInt32(4),
                    description = rdr.GetString(5),
                    active = rdr.GetBoolean(6)
                };
                homeworks.Add(homework);
            }
            return homeworks;
        }
    }
}
