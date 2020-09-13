using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using Microsoft.VisualBasic.CompilerServices;

namespace SabreAppWPF.Database.Get
{
    public static class Place
    {
        public static List<PlaceInfo> All(string type)
        {
            string source = (type == "old") ? Update.oldPath : GlobalVariable.path;
            using SQLiteConnection connection = new SQLiteConnection("Data Source=" + source);
            connection.Open();
            using SQLiteCommand cmd = new SQLiteCommand(connection);
            List<PlaceInfo> placeList = new List<PlaceInfo>();
            cmd.CommandText = "SELECT * FROM places";
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                PlaceInfo place = new PlaceInfo()
                {
                    PlaceId = rdr.GetInt32(0),
                    PlanId = rdr.GetInt32(1),
                    StudentId = rdr.GetInt32(2),
                    Row = rdr.GetInt32(3),
                    Column = rdr.GetInt32(4)
                };
                placeList.Add(place);
            }
            return placeList;
        }

        //places(placeId INTEGER PRIMARY KEY, planId INTEGER, studentId INTEGER, row INTEGER, column INTEGER);
        public static List<PlaceInfo> AllFromPlanId(int planId)
        {
            List<PlaceInfo> placeList = new List<PlaceInfo>();
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "SELECT * FROM places WHERE planId = @planId";
            cmd.Parameters.AddWithValue("planId", planId);
            cmd.Prepare();
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                PlaceInfo place = new PlaceInfo()
                {
                    PlaceId = rdr.GetInt32(0),
                    PlanId = rdr.GetInt32(1),
                    StudentId = rdr.GetInt32(2),
                    Row = rdr.GetInt32(3),
                    Column = rdr.GetInt32(4)
                };
                placeList.Add(place);
            }

            return placeList;

        }
    }
}
