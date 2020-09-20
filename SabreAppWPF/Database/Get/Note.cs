using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace SabreAppWPF.Database.Get
{
    public static class Note
    {
        public static List<NoteInfo> All(string type)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            List<NoteInfo> notes = new List<NoteInfo>();
            cmd.CommandText = "SELECT * FROM notes";

            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                NoteInfo note = new NoteInfo()
                {
                    noteId = (int)rdr.GetInt64(0),
                    studentId = rdr.GetInt32(1),
                    creationDate = rdr.GetInt32(2),
                    content = rdr.GetString(3),
                    active = rdr.GetBoolean(4)
                };
                notes.Add(note);
            }
            return notes;
        }

        public static List<NoteInfo> AllFromStudentId(int studentId)
        {
            List<NoteInfo> notes = new List<NoteInfo>();
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = $"SELECT * FROM notes WHERE studentId = {studentId}";
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                NoteInfo note = new NoteInfo()
                {
                    noteId = (int)rdr.GetInt64(0),
                    studentId = rdr.GetInt32(1),
                    creationDate = rdr.GetInt32(2),
                    content = rdr.GetString(3),
                    active = rdr.GetBoolean(4)
                };
                notes.Add(note);
            }
            return notes;
        }
    }
}
