using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace SabreAppWPF.Database.Insert
{
    public static class Pair
    {
        public static int One(int studentId1, int studentId2, int classroomId)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "INSERT INTO pairs(studentId1, studentId2, classroomId), VALUES(@studentId1, @studentId2, @classroomId)";
            cmd.Parameters.AddWithValue("studentId1", studentId1);
            cmd.Parameters.AddWithValue("studentId2", studentId2);
            cmd.Parameters.AddWithValue("classroomId", classroomId);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT last_insert_rowid()";
            long pairId = (long)cmd.ExecuteScalar();
            return (int)pairId;

        }
    }
}
