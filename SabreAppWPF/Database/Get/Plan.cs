using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace SabreAppWPF.Database.Get
{
    public static class Plan
    {
        public static PlanInfo FromId(int planId)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "SELECT * FROM plans WHERE planId = @planId";
            cmd.Parameters.AddWithValue("planId", planId);
            cmd.Prepare();
            using SQLiteDataReader rdr = cmd.ExecuteReader();
            PlanInfo plan = new PlanInfo();

            while (rdr.Read())
            {
                plan.planId = rdr.GetInt32(0);
                plan.scheduleId = rdr.GetInt32(1);
                plan.roomId = rdr.GetInt32(2);
                plan.spacing = rdr.GetString(3);
                plan.name = rdr.GetString(4);
            };
            return plan;
        }
        public static List<PlanInfo> FromScheduleId(int scheduleId)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "SELECT * FROM plans WHERE scheduleId = @scheduleId";
            cmd.Parameters.AddWithValue("scheduleId", scheduleId);
            cmd.Prepare();
            using SQLiteDataReader rdr = cmd.ExecuteReader();
            List<PlanInfo> planInfos = new List<PlanInfo>();
            while (rdr.Read())
            {
                PlanInfo plan = new PlanInfo()
                {
                    planId = rdr.GetInt32(0),
                    scheduleId = rdr.GetInt32(1),
                    roomId = rdr.GetInt32(2),
                    spacing = rdr.GetString(3),
                    name = rdr.GetString(4)
                };
                planInfos.Add(plan);
            }
            return planInfos;

        }
    }

}
