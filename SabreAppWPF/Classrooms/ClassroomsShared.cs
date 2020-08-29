using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Data.SQLite;

namespace SabreAppWPF.Classrooms
{
    public static class ClassroomsShared
    {
        public static int? GetNextSessionTimestamp(ScheduleInfo schedule)
        {
            if (schedule.weekDay == null || schedule.hour == null || schedule.minute == null || schedule.repetitivity == null || schedule.lastDate == null) return null;
            int repetitivity = (int)schedule.repetitivity;
            DateTime currentDay = DateTime.Today;
            int daysToNextSession = 0;
            DateTime nextDateTime = DateTimeOffset.FromUnixTimeSeconds((long)schedule.lastDate).LocalDateTime;
            daysToNextSession = GetNumberOfDaysToNextSession(nextDateTime, repetitivity);

            currentDay.AddDays(daysToNextSession);
            currentDay.AddHours((double)schedule.hour);
            currentDay.AddMinutes((double)schedule.minute);
            int nextSessionTimestamp = (int)new DateTimeOffset(currentDay).ToUnixTimeSeconds();
            return nextSessionTimestamp;
        }

        public static int GetNumberOfDaysToNextSession(DateTime nextDate, int repetitivity)
        {
            DateTime currentDay = DateTime.Today;
            TimeSpan timeSpan = nextDate.Subtract(currentDay);
            int daysToNextSession = (7 * (repetitivity + 1)) - timeSpan.Days;
            return daysToNextSession;
        }

        public static List<ScheduleInfo> GetSchedulesFromClassroomId(int classroomId)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "SELECT * FROM schedules WHERE classroomId = @classroomId";
            cmd.Parameters.AddWithValue("classroomId", classroomId);
            cmd.Prepare();
            using SQLiteDataReader rdr = cmd.ExecuteReader();
            List<ScheduleInfo> scheduleList = new List<ScheduleInfo>();
            while (rdr.Read())
            {
                ScheduleInfo scheduleInfo = new ScheduleInfo()
                {
                    scheduleId = rdr.GetInt32(0),
                    classroomId = rdr.GetInt32(1),
                    roomId = rdr.GetInt32(2),
                    weekDay = rdr.GetInt32(3),
                    hour = rdr.GetInt32(4),
                    minute = rdr.GetInt32(5),
                    repetitivity = rdr.GetInt32(6),
                    lastDate = rdr.GetInt32(7)
                };
                scheduleList.Add(scheduleInfo);
            }
            rdr.Close();
            return scheduleList;
        }

    }
}
