using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Data.SQLite;

namespace SabreAppWPF.Classrooms
{
    public static class ClassroomsShared
    {
        public static int GetNumberOfDaysToNextSession(ScheduleInfo schedule)
        {
            if (schedule.repetitivity == null || schedule.nextDate == null) return 0;
            DateTime currentDay = DateTime.Today;
            DateTime scheduleNextDayDateTime = DateTimeOffset.FromUnixTimeSeconds((long)schedule.nextDate).LocalDateTime;
            TimeSpan timeSpan = scheduleNextDayDateTime.Subtract(currentDay);
            int daysToNextSession = (7 * ((int)schedule.repetitivity + 1)) - timeSpan.Days;
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
                    repetitivity = rdr.GetInt32(3),
                    nextDate = rdr.GetInt32(4),
                    duration = rdr.GetInt32(5)
                };
                scheduleList.Add(scheduleInfo);
            }
            rdr.Close();
            return scheduleList;
        }

    }
}
