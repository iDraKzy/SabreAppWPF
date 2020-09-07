using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace SabreAppWPF.Database.Insert
{
    public static class Room
    {
        public static int One(string name, int rows, int columns)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "INSERT INTO rooms(name, rows, columns) VALUES(@name, @rows, @columns)";
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("rows", rows);
            cmd.Parameters.AddWithValue("columns", columns);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT last_insert_rowid()";
            long roomId = (long)cmd.ExecuteScalar();

            return (int)roomId;
        }
    }
}
