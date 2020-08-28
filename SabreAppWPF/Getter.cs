using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace SabreAppWPF
{
    public static class Getter
    {
        /// <summary>
        /// Returns all grades of a student
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns>List of GradeInfo</returns>
        public static List<GradeInfo> GetAllGrades(int studentId)
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

        /// <summary>
        /// Get all votes of the specified student of the given type (upvote = true, downvote = false)
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="type">type of votes (upvote = true, downvote = false)</param>
        /// <returns></returns>
        public static List<VotesInfo> GetAllVotes(int studentId, bool type)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = $"SELECT * FROM votes WHERE studentId = {studentId} AND upvotes = {type}";
            using SQLiteDataReader rdr = cmd.ExecuteReader();
            List<VotesInfo> votesList = new List<VotesInfo>();

            while (rdr.Read())
            {
                VotesInfo voteInfo = new VotesInfo()
                {
                    voteId = rdr.GetInt32(0),
                    studentId = rdr.GetInt32(1),
                    upvotes = rdr.GetBoolean(2),
                    description = rdr.GetString(3),
                    creationDate = rdr.GetInt32(4)
                };
                votesList.Add(voteInfo);
            }
            rdr.Close();
            return votesList;
        }
        /// <summary>
        /// Returns all rooms from the database
        /// </summary>
        /// <returns>List of RoomInfo</returns>
        public static List<RoomInfo> GetAllRooms()
        {
            List<RoomInfo> roomsList = new List<RoomInfo>();
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "SELECT * FROM rooms";
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                RoomInfo roomInfo = new RoomInfo()
                {
                    RoomId = rdr.GetInt32(0),
                    Name = rdr.GetString(1),
                    Rows = rdr.GetInt32(2),
                    Columns = rdr.GetInt32(3)
                };
                roomsList.Add(roomInfo);
            }
            return roomsList;
        }
        /// <summary>
        /// Gets all the classrooms from the database
        /// </summary>
        /// <returns>List of ClassroomInfo</returns>
        public static List<ClassroomInfo> GetAllClassrooms()
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
        /// <summary>
        /// Get the classroom name from its id
        /// </summary>
        /// <param name="classroomId">The id of the classroom</param>
        /// <returns>Name of the classroom</returns>
        public static string GetClassrommNameFromID(int classroomId)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "SELECT name FROM classrooms WHERE classroomId = @classroomId";
            cmd.Parameters.AddWithValue("classroomId", classroomId);
            cmd.Prepare();
            string classroomName = (string)cmd.ExecuteScalar();
            return classroomName;
        }
        /// <summary>
        /// Get the classroomid of a student from the id of the student
        /// </summary>
        /// <param name="studentId">The id of the student</param>
        /// <returns>Id of the classroom</returns>
        public static int GetClassroomIDFromStudentID(int studentId)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "SELECT classroomId FROM students WHERE studentId = @studentId";
            cmd.Parameters.AddWithValue("studentId", studentId);
            cmd.Prepare();
            int classroomId = (int)cmd.ExecuteScalar();
            return classroomId;
        }
        /// <summary>
        /// Get the student name from his id
        /// </summary>
        /// <param name="studentId">The id of the student</param>
        /// <returns>string[lastname, surname]</returns>
        public static string[] GetStudentNameFromID(int studentId)
        {
            string[] nameArray = new string[2];
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = $"SELECT lastname, surname FROM students WHERE studentId = {studentId}";
            using SQLiteDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                nameArray[0] = rdr.GetString(0);
                nameArray[1] = rdr.GetString(1);
            }

            return nameArray;
        }
        /// <summary>
        /// Return the id of a student based on his name
        /// </summary>
        /// <param name="lastname"></param>
        /// <param name="surname"></param>
        /// <returns>studentId</returns>
        public static int GetStudentIdFromName(string lastname, string surname)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "SELECT studentId FROM students WHERE lastname = @lastname AND surname = @surname";
            cmd.Parameters.AddWithValue("lastname", lastname);
            cmd.Parameters.AddWithValue("surname", surname);
            cmd.Prepare();
            long studentId = (long)cmd.ExecuteScalar();

            return (int)studentId;
        }
    }
}
