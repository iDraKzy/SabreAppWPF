using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace SabreAppWPF.Database.Get
{
    public static class Schedule
    {

        /// <summary>
        /// Get the schedule from its ID
        /// </summary>
        /// <param name="scheduleId">The id of the schedule</param>
        /// <returns>ScheduleInfo object</returns>
        public static ScheduleInfo FromId(int scheduleId)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "SELECT * FROM schedules WHERE scheduleId = @scheduleId";
            cmd.Parameters.AddWithValue("scheduleId", scheduleId);
            cmd.Prepare();
            using SQLiteDataReader rdr = cmd.ExecuteReader();
            ScheduleInfo scheduleInfo = new ScheduleInfo();
            while (rdr.Read())
            {
                scheduleInfo.scheduleId = rdr.GetInt32(0);
                scheduleInfo.classroomId = rdr.GetInt32(1);
                scheduleInfo.roomId = rdr.GetInt32(2);
                scheduleInfo.repetitivity = rdr.GetInt32(3);
                scheduleInfo.nextDate = rdr.GetInt32(4);
                scheduleInfo.duration = rdr.GetInt32(5);
            }
            return scheduleInfo;

        }

        public static List<ScheduleInfo> AllFromClassroomId(int classroomId)
        {
            List<ScheduleInfo> scheduleList = new List<ScheduleInfo>();

            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "SELECT * FROM schedules WHERE classroomId = @classroomId";
            cmd.Parameters.AddWithValue("classroomId", classroomId);
            cmd.Prepare();
            using SQLiteDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                ScheduleInfo schedule = new ScheduleInfo()
                {
                    scheduleId = rdr.GetInt32(0),
                    classroomId = rdr.GetInt32(1),
                    roomId = rdr.GetInt32(2),
                    repetitivity = rdr.GetInt32(3),
                    nextDate = rdr.GetInt32(4),
                    duration = rdr.GetInt32(5)
                };
                scheduleList.Add(schedule);
            }
            return scheduleList;
        }


        /// <summary>
        /// Returns all schedules from the database
        /// </summary>
        /// <returns>List of ScheduleInfo</returns>
        public static List<ScheduleInfo> All()
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "SELECT * FROM schedules";

            List<ScheduleInfo> scheduleInfoList = new List<ScheduleInfo>();

            using SQLiteDataReader rdr = cmd.ExecuteReader();
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
                scheduleInfoList.Add(scheduleInfo);
            }
            return scheduleInfoList;
        }
    }
}
