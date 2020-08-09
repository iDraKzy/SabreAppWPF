using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using SabreAppWPF.Students;

namespace SabreAppWPF
{
    /// <summary>
    /// Logique d'interaction pour studentsPage.xaml
    /// </summary>
    public partial class studentsPage : Page
    {
        //private readonly int int32id; 
        public studentsPage()
        {
            InitializeComponent();
            //this.int32id = int.Parse(id);
        }

        private NoteInfo GetLastNote(List<NoteInfo> notesList)
        {
            NoteInfo lastNote = new NoteInfo();
            foreach (NoteInfo note in notesList)
            {
                if (lastNote.creationDate < note.creationDate)
                {
                    lastNote = note;
                }
            }
            return lastNote;
        }

        private List<StudentInfo> ReadStudents(SQLiteCommand cmd)
        {
            cmd.CommandText = "SELECT * FROM students";
            List<StudentInfo> studentList = new List<StudentInfo>();

            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                StudentInfo studentInfo = new StudentInfo()
                {
                    studentId = rdr.GetInt32(0),
                    classroomId = rdr.GetInt32(1),
                    lastname = rdr.GetString(2),
                    surname = rdr.GetString(3),
                    gender = rdr.GetBoolean(4),
                    board = rdr.GetInt32(5),
                    interrogation = rdr.GetBoolean(6)
                };
                studentList.Add(studentInfo);
            }

            rdr.Close();
            return studentList;
        }

        private void Students_Load(object sender, RoutedEventArgs e)
        {
            using SQLiteConnection connection = new SQLiteConnection("Data Source=" + GlobalVariable.path);
            connection.Open();
            using SQLiteCommand cmd = new SQLiteCommand(connection);
            List<StudentInfo> studentList = ReadStudents(cmd);



            for (int i = 0; i < studentList.Count; i++)
            {
                //Get classroom name
                cmd.CommandText = $"SELECT name FROM classrooms WHERE classroomId = {studentList[i].classroomId}";
                string classroomName = (string)cmd.ExecuteScalar();

                //Get all votes from the student
                cmd.CommandText = $"SELECT * FROM votes WHERE studentId = {studentList[i].studentId}";
                using SQLiteDataReader rdrVotes = cmd.ExecuteReader();

                List<bool> votesList = new List<bool>();

                while (rdrVotes.Read())
                {
                    votesList.Add(rdrVotes.GetBoolean(2));
                }
                rdrVotes.Close();

                //Count the downvotes and upvotes
                int upvotes = 0;
                int downvotes = 0;

                foreach(bool vote in votesList)
                {
                    if(vote)
                    {
                        upvotes++;
                    }
                    else
                    {
                        downvotes++;
                    }
                }

                cmd.CommandText = $"SELECT * FROM notes WHERE studentId = {studentList[i].studentId}";
                using SQLiteDataReader rdrNotes = cmd.ExecuteReader();

                List<NoteInfo> notesList = new List<NoteInfo>();

                while (rdrNotes.Read())
                {
                    NoteInfo noteInfo = new NoteInfo
                    {
                        noteId = rdrNotes.GetInt32(0),
                        creationDate = rdrNotes.GetInt32(2),
                        content = rdrNotes.GetString(3)
                    };
                    notesList.Add(noteInfo);
                }

                NoteInfo lastNote = GetLastNote(notesList);

                string name = studentList[i].surname + " " + studentList[i].lastname;

                StudentsShared.AddStudentToUI(this, studentList[i].studentId, name, classroomName, lastNote.content ?? "Note par défaut", upvotes, downvotes);
            }
        }
    }
}
