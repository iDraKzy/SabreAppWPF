using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace SabreAppWPF.Database.Insert
{
    public static class Grade
    {
        public static int One(int studentId, float grade, int coeff)
        {
            int currentTimestamp = GlobalFunction.ReturnTimestamp(DateTime.Now);

            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "INSERT INTO grades(studentId, grade, coeff, creationDate) VALUES(@studentId, @grade, @coeff, @creationDate)";
            cmd.Parameters.AddWithValue("studentId", studentId);
            cmd.Parameters.AddWithValue("grade", grade);
            cmd.Parameters.AddWithValue("coeff", coeff);
            cmd.Parameters.AddWithValue("creationDate", currentTimestamp);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT last_insert_rowid()";
            long gradeId = (long)cmd.ExecuteScalar();

            return (int)gradeId;
        }
    }
}
