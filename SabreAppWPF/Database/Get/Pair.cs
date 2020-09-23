using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace SabreAppWPF.Database.Get
{
    public static class Pair
    {
        public static List<PairInfo> GetAllPairsFromClassroomId(int classroomId)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "SELECT * FROM pairs WHERE classroomId = @classroomId";
            cmd.Parameters.AddWithValue("classroomId", classroomId);
            List<PairInfo> pairs = new List<PairInfo>();

            cmd.Prepare();
            using SQLiteDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                PairInfo pair = new PairInfo()
                {
                    PairId = rdr.GetInt32(0),
                    StudentId1 = rdr.GetInt32(1),
                    StudentId2 = rdr.GetInt32(2),
                    ClassroomId = rdr.GetInt32(3)
                };
                pairs.Add(pair);
            }
            return pairs;
        }
    }
}
