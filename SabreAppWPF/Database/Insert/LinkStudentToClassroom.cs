using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Data.SQLite;

namespace SabreAppWPF.Database.Insert
{
    public static class LinkStudentToClassroom
    {
        public static int One(int studentId, int classroomId)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "INSERT INTO linkStudentToClassroom(studentId, classroomId) VALUES(@studentId, @classroomId)";
            cmd.Parameters.AddWithValue("studentId", studentId);
            cmd.Parameters.AddWithValue("classroomId", classroomId);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT last_insert_rowid()";
            long linkStudentToClassroomId = (long)cmd.ExecuteScalar();
            return (int)linkStudentToClassroomId;
        }
    }
}
