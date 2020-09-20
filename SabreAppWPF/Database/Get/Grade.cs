using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace SabreAppWPF.Database.Get
{
    public static class Grade
    {
        public static List<GradeInfo> All(string type)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            List<GradeInfo> grades = new List<GradeInfo>();
            cmd.CommandText = "SELECT * FROM grades";

            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                GradeInfo grade = new GradeInfo()
                {
                    GradeId = rdr.GetInt32(0),
                    StudentId = rdr.GetInt32(1),
                    Grade = rdr.GetFloat(2),
                    Coeff = rdr.GetInt32(3),
                    CreationDate = rdr.GetInt32(4)
                };
                grades.Add(grade);
            }
            return grades;
        }

        /// <summary>
        /// Returns all grades of a student
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns>List of GradeInfo</returns>
        public static List<GradeInfo> AllFromStudentId(int studentId)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "SELECT * FROM grades WHERE studentId = @studentId";
            cmd.Parameters.AddWithValue("studentId", studentId);
            cmd.Prepare();

            List<GradeInfo> gradesList = new List<GradeInfo>();

            using SQLiteDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                GradeInfo gradeInfo = new GradeInfo()
                {
                    GradeId = rdr.GetInt32(0),
                    StudentId = rdr.GetInt32(1),
                    Grade = rdr.GetFloat(2),
                    Coeff = rdr.GetInt32(3),
                    CreationDate = rdr.GetInt32(4)
                };
                gradesList.Add(gradeInfo);
            }
            return gradesList;
        }
    }
}
