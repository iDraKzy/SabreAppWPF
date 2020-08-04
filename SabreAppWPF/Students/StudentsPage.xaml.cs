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

        public void AddStudentToUI(int studentId, string name, string classroom, string lastNote, int upvotes, int downvotes)
        {
            //Create newStudent
            student newStudent = new student();

            //Handle texts
            newStudent.studentName.Content = name;
            newStudent.studentClassroom.Content = classroom ?? "Classe";
            newStudent.studentNote.Text = lastNote ?? "Note par défaut";

            //Handle votesText
            newStudent.studentUpvote.Content = upvotes.ToString();
            newStudent.studentDownvote.Content = downvotes.ToString();

            //Handle upvoteButton
            newStudent.upvoteButton.Tag = studentId;
            newStudent.upvoteButton.Click += (object s, RoutedEventArgs e) =>
            {
                int studentId = (int)((Button)s).Tag;
                int score = int.Parse((string)newStudent.studentUpvote.Content);
                score++;
                newStudent.studentUpvote.Content = score.ToString();
                AddVotesToDb(studentId, true, "Upvote rapide");
            };

            //Handle downvoteButton
            newStudent.downvoteButton.Tag = studentId;
            newStudent.downvoteButton.Click += (s, e) =>
            {
                int studentId = (int)((Button)s).Tag;
                int score = int.Parse((string)newStudent.studentDownvote.Content);
                score++;
                newStudent.studentDownvote.Content = score.ToString();
                AddVotesToDb(studentId, false, "Downvote rapide");
            };

            newStudent.punishButton.Tag = studentId;


            studentListPanel.Children.Add(newStudent);
        }

        private void AddVotesToDb(int studentId, bool vote, string description)
        {
            using SQLiteConnection connection = new SQLiteConnection("Data Source=" + GlobalVariable.path);
            connection.Open();
            using SQLiteCommand cmd = new SQLiteCommand(connection)
            {
                CommandText = "INSERT INTO votes(studentId, upvotes, description, creationDate) VALUES(@studentId, @upvotes, @description, @creationDate)"
            };
            cmd.Parameters.AddWithValue("studentId", studentId);
            cmd.Parameters.AddWithValue("upvotes", vote);
            cmd.Parameters.AddWithValue("description", description);
            cmd.Parameters.AddWithValue("creationDate", Convert.ToInt32(new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()));
            cmd.Prepare();
            cmd.ExecuteNonQuery();
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
                    name = rdr.GetString(2),
                    gender = rdr.GetBoolean(3),
                    board = rdr.GetInt32(4),
                    interrogation = rdr.GetBoolean(5)
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

                foreach(bool element in votesList)
                {
                    if(element)
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

                AddStudentToUI(studentList[i].studentId, studentList[i].name, classroomName, lastNote.content ?? "Note par défaut", upvotes, downvotes);
            }
        }
    }
}
