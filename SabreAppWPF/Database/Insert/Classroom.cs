using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace SabreAppWPF.Database.Insert
{
    public static class Classroom
    {
        static public int One(string name)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "INSERT INTO classrooms(name) VALUES(@name)";
            cmd.Parameters.AddWithValue("name", name);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT last_insert_rowid()";

            long classroomId = (long)cmd.ExecuteScalar();
            return (int)classroomId;
        }

        static public void One(string name, int classroomId)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "INSERT INTO classrooms(classroomId, name) VALUES(@classroomId, @name)";
            cmd.Parameters.AddWithValue("classroomId", classroomId);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
    }
}
