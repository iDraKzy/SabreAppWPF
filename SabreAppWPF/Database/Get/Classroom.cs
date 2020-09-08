using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace SabreAppWPF.Database.Get
{
    public static class Classroom
    {
        /// <summary>
        /// Get the classroomid of a student from the id of the student
        /// </summary>
        /// <param name="studentId">The id of the student</param>
        /// <returns>Id of the classroom</returns>
        public static int IDFromStudentID(int studentId)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "SELECT classroomId FROM linkStudentToClassroom WHERE studentId = @studentId";
            cmd.Parameters.AddWithValue("studentId", studentId);
            cmd.Prepare();
            long classroomId = (long)cmd.ExecuteScalar();
            return (int)classroomId;
        }

        /// <summary>
        /// Get the classroom name from its id
        /// </summary>
        /// <param name="classroomId">The id of the classroom</param>
        /// <returns>Name of the classroom</returns>
        public static string NameFromID(int classroomId)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "SELECT name FROM classrooms WHERE classroomId = @classroomId";
            cmd.Parameters.AddWithValue("classroomId", classroomId);
            cmd.Prepare();
            string classroomName = (string)cmd.ExecuteScalar();
            return classroomName;
        }

        /// <summary>
        /// Gets all the classrooms from the database
        /// </summary>
        /// <returns>List of ClassroomInfo</returns>
        public static List<ClassroomInfo> All()
        {
            List<ClassroomInfo> classroomsList = new List<ClassroomInfo>();
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "SELECT * FROM classrooms";
            using SQLiteDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                ClassroomInfo classroomInfo = new ClassroomInfo()
                {
                    ClassroomId = rdr.GetInt32(0),
                    Name = rdr.GetString(1)
                };
                classroomsList.Add(classroomInfo);
            }

            return classroomsList;
        }

    }
}
