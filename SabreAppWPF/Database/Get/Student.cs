using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using Windows.Media.Protection.PlayReady;

namespace SabreAppWPF.Database.Get
{
    public static class Student
    {
        public static List<StudentInfo> All()
        {
            List<StudentInfo> studentList = new List<StudentInfo>();
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "SELECT * FROM students";
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                StudentInfo student = new StudentInfo()
                {
                    studentId = rdr.GetInt32(0),
                    lastname = rdr.GetString(1),
                    surname = rdr.GetString(2),
                    gender = rdr.GetBoolean(3),
                    board = rdr.GetBoolean(4),
                    interrogation = rdr.GetBoolean(5)
                };
                studentList.Add(student);
            }
            return studentList;
        }

        /// <summary>
        /// Return the student from its id
        /// </summary>
        /// <param name="studentId">Id of the student</param>
        /// <returns>StudentInfo</returns>
        public static StudentInfo FromId(int studentId)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "SELECT * FROM students WHERE studentId = @studentId";
            cmd.Parameters.AddWithValue("studentId", studentId);
            cmd.Prepare();
            using SQLiteDataReader rdr = cmd.ExecuteReader();
            StudentInfo student = new StudentInfo();
            while (rdr.Read())
            {
                student.studentId = rdr.GetInt32(0);
                student.lastname = rdr.GetString(1);
                student.surname = rdr.GetString(2);
                student.gender = rdr.GetBoolean(3);
                student.board = rdr.GetBoolean(4);
                student.interrogation = rdr.GetBoolean(5);
            }
            return student;
        }
        /// <summary>
        /// Return the id of a student based on his name
        /// </summary>
        /// <param name="lastname"></param>
        /// <param name="surname"></param>
        /// <returns>studentId</returns>
        public static int IdFromName(string lastname, string surname)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "SELECT studentId FROM students WHERE lastname = @lastname AND surname = @surname";
            cmd.Parameters.AddWithValue("lastname", lastname);
            cmd.Parameters.AddWithValue("surname", surname);
            cmd.Prepare();
            long studentId = (long)cmd.ExecuteScalar();

            return (int)studentId;
        }

        /// <summary>
        /// Get the student name from his id
        /// </summary>
        /// <param name="studentId">The id of the student</param>
        /// <returns>string[lastname, surname]</returns>
        public static string[] NameFromID(int studentId)
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

        public static List<StudentInfo> AllFromClassroomId(int classroomId)
        {
            List<StudentInfo> studentList = new List<StudentInfo>();
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "SELECT * FROM linkStudentToClassroom WHERE classroomId = @classroomId";
            cmd.Parameters.AddWithValue("classroomId", classroomId);
            cmd.Prepare();
            using SQLiteDataReader rdr = cmd.ExecuteReader();
            List<int> studentIdList = new List<int>();

            while (rdr.Read())
            {
                studentIdList.Add(rdr.GetInt32(1));
            }

            foreach (int studentId in studentIdList)
            {
                StudentInfo student = FromId(studentId);
                studentList.Add(student);
            }

            return studentList;
        }
    }
}
