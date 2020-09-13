using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace SabreAppWPF.Database.Insert
{
    public static class Place
    {
        public static int One(int planId, int studentId, int row, int column)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "INSERT INTO places(planId, studentId, row, column) VALUES(@planId, @studentId, @row, @column)";
            cmd.Parameters.AddWithValue("planId", planId);
            cmd.Parameters.AddWithValue("studentId", studentId);
            cmd.Parameters.AddWithValue("row", row);
            cmd.Parameters.AddWithValue("column", column);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT last_insert_rowid()";
            long placeId = (long)cmd.ExecuteScalar();
            return (int)placeId;
        }

        public static void One(int planId, int studentId, int row, int column, int placeId)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "INSERT INTO places(placeId, planId, studentId, row, column) VALUES(@placeId, @planId, @studentId, @row, @column)";
            cmd.Parameters.AddWithValue("placeId", placeId);
            cmd.Parameters.AddWithValue("planId", planId);
            cmd.Parameters.AddWithValue("studentId", studentId);
            cmd.Parameters.AddWithValue("row", row);
            cmd.Parameters.AddWithValue("column", column);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
    }
}
