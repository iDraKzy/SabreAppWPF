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
            if (schedule.weekDay == null || schedule.hour == null || schedule.minute == null) return null;
            DateTime currentDay = DateTime.Today;
            int daysToNextSession = GetNumberOfDaysToNextSession((int)schedule.weekDay);

            currentDay.AddDays(daysToNextSession);
            currentDay.AddHours((double)schedule.hour);
            currentDay.AddMinutes((double)schedule.minute);
            int nextSessionTimestamp = (int)new DateTimeOffset(currentDay).ToUnixTimeSeconds();
            return nextSessionTimestamp;
        }

        public static int GetNumberOfDaysToNextSession(int weekDay)
        {
            DateTime currentDay = DateTime.Today;
            int currentWeekDay = (int)currentDay.DayOfWeek;
            int daysToNextSession = 0;
            while (currentWeekDay != weekDay)
            {
                currentWeekDay++;
                if (currentWeekDay > 6) currentWeekDay = 0;
                daysToNextSession++;
            }

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
                    minute = rdr.GetInt32(5)
                };
                scheduleList.Add(scheduleInfo);
            }
            rdr.Close();
            return scheduleList;
        }

    }
}
