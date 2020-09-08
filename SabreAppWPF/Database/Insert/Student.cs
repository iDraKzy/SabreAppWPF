using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Text;
using System.Data.SQLite;

namespace SabreAppWPF.Database.Insert
{
    public static class Student
    {
        public static int One(int classroomId, string lastname, string surname, bool trueGender)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "INSERT INTO students(lastname, surname, gender, board, interrogation) VALUES(@lastname, @surname, @gender, 0, false)";
            cmd.Parameters.AddWithValue("lastname", lastname);
            cmd.Parameters.AddWithValue("surname", surname);
            cmd.Parameters.AddWithValue("gender", trueGender);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT last_insert_rowid()";
            long studentId = (long)cmd.ExecuteScalar();

            Insert.LinkStudentToClassroom.One((int)studentId, classroomId);

            return (int)studentId;
        }
    }
}
