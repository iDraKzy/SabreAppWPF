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
using System.Collections.ObjectModel;
using SabreAppWPF.Students.StudentDetails;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SabreAppWPF.AddPages;
using Windows.UI.Xaml.Automation.Peers;
using System.Security.Policy;
using Windows.UI.WebUI;

namespace SabreAppWPF
{
    /// <summary>
    /// Logique d'interaction pour studentsPage.xaml
    /// </summary>
    public partial class studentsPage : Page
    {
        public static ObservableCollection<StudentDisplay> studentsCollection = new ObservableCollection<StudentDisplay>();
        //private readonly int int32id; 
        public studentsPage()
        {
            InitializeComponent();
            studentsCollection = new ObservableCollection<StudentDisplay>();
            studentList.ItemsSource = studentsCollection;
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "SELECT studentId FROM students";
            using SQLiteDataReader rdr = cmd.ExecuteReader();
            List<int> studentIdList = new List<int>();
            while(rdr.Read())
            {
                studentIdList.Add((int)rdr.GetInt64(0));
            }
            ReadStudentsData(studentIdList);
        }

        public studentsPage(int classroomId)
        {
            InitializeComponent();
            studentsCollection = new ObservableCollection<StudentDisplay>();
            studentList.ItemsSource = studentsCollection;
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "SELECT studentId FROM linkStudentToClassroom WHERE classroomId = @classroomId";
            cmd.Parameters.AddWithValue("classroomId", classroomId);
            cmd.Prepare();

            List<int> studentIdList = new List<int>();

            using SQLiteDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                studentIdList.Add(rdr.GetInt32(0));
            }

            ReadStudentsData(studentIdList);
        }

        private void ReadStudentsData(List<int> studentIdList)
        {
            foreach (int studentId in studentIdList)
            {

                using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
                //Handle classroom name
                List<int> classroomIdList = Database.Get.Classroom.AllIDFromStudentID(studentId);
                List<string> classroomNameList = new List<string>();
                foreach (int classroomId in classroomIdList)
                {
                    classroomNameList.Add(Database.Get.Classroom.NameFromID(classroomId));
                }

                string classroomName = String.Join(", ", classroomNameList.ToArray());

                string[] studentName = Database.Get.Student.NameFromID(studentId);

                //Handle homework
                List<HomeworkInfo> homeworkList = GetAllHomeworks(studentId);
                HomeworkInfo lastHomework = GetLastHomework(homeworkList);

                bool lastHomeworkButtonEnabled = false;
                string lastHomeworkStatus = GlobalVariable.specialCharacter["CheckMark"];
                string lastHomeworkColor = "Green";
                if (lastHomework.retrieveDate == 0 && lastHomework.creationDate != 0)
                {
                    lastHomeworkButtonEnabled = true;
                    lastHomeworkStatus = GlobalVariable.specialCharacter["Cross"];
                    lastHomeworkColor = "Red";
                }

                //Handle note
                List<NoteInfo> notesList = GetAllNotes(studentId);
                NoteInfo lastNotes = GetLastNote(notesList);

                //Handle votes
                List<VotesInfo> upvotesList = Database.Get.Vote.AllFromStudentId(studentId, true);
                List<VotesInfo> downvotesList = Database.Get.Vote.AllFromStudentId(studentId, false);

                string average = "";

                List<GradeInfo> gradesList = Database.Get.Grade.AllFromStudentId(studentId);
                if (gradesList.Count == 0)
                {
                    average = "20/20";
                } 
                else
                {
                    float[] gradesArray = new float[gradesList.Count];
                    int[] coeffArray = new int[gradesList.Count];
                    for (int i = 0; i < gradesList.Count; i++)
                    {
                        gradesArray[i] = gradesList[i].Grade;
                        coeffArray[i] = gradesList[i].Coeff;
                    }

                    int coeffSum = coeffArray.Sum();
                    float[] gradesCoeffMultiply = new float[gradesList.Count];

                    for (int i = 0; i < gradesList.Count; i++)
                    {
                        gradesCoeffMultiply[i] = gradesArray[i] * coeffArray[i];
                    }

                    float gradesCoefMultiplySum = gradesCoeffMultiply.Sum();
                    float averageFloat = gradesCoefMultiplySum / coeffSum;
                    average = averageFloat.ToString() + "/20";
                }

                StudentDisplay studentDisplay = new StudentDisplay()
                {
                    ID = studentId,
                    Name = studentName[1] + " " + studentName[0],
                    ClassroomName = classroomName,
                    HomeworkButtonEnabled = lastHomeworkButtonEnabled,
                    LastHomeworkStatusText = lastHomeworkStatus,
                    LastHomeworkStatusColor = lastHomeworkColor,
                    LastHomeWorkId = lastHomework.homeworkId,
                    Note = lastNotes.content ?? "Aucune note",
                    Average = average,
                    UpvotesCount = upvotesList.Count.ToString(),
                    DownvotesCount = downvotesList.Count.ToString()
                };
                studentsCollection.Add(studentDisplay);
            }
        }
        /// <summary>
        /// Get all the notes from a given student
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public static List<NoteInfo> GetAllNotes(int studentId)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = $"SELECT * FROM notes WHERE studentId = {studentId}";
            using SQLiteDataReader rdr = cmd.ExecuteReader();
            List<NoteInfo> notesList = new List<NoteInfo>();

            while (rdr.Read())
            {
                NoteInfo noteInfo = new NoteInfo()
                {
                    noteId = rdr.GetInt32(0),
                    creationDate = rdr.GetInt32(2),
                    content = rdr.GetString(3)
                };
                notesList.Add(noteInfo);
            }
            rdr.Close();
            return notesList;
        }

        /// <summary>
        /// Get the latest created note from a given list
        /// </summary>
        /// <param name="notesList"></param>
        /// <returns></returns>
        public static NoteInfo GetLastNote(List<NoteInfo> notesList)
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
        /// <summary>
        /// Get all homeworks from a specific studentId
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        private static List<HomeworkInfo> GetAllHomeworks(int studentId)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = $"SELECT * FROM homeworks WHERE studentId = {studentId}";
            using SQLiteDataReader rdr = cmd.ExecuteReader();
            List<HomeworkInfo> homeworksList = new List<HomeworkInfo>();
            while (rdr.Read())
            {
                HomeworkInfo homeworkInfo = new HomeworkInfo()
                {
                    homeworkId = rdr.GetInt32(0),
                    creationDate = rdr.GetInt32(2),
                    endDate = rdr.GetInt32(3),
                    retrieveDate = rdr.GetInt32(4),
                    description = rdr.GetString(5)
                };
                homeworksList.Add(homeworkInfo);
            }
            rdr.Close();
            return homeworksList;
        }
        /// <summary>
        /// Get the last created homework from a given list
        /// </summary>
        /// <param name="homeworksList"></param>
        /// <returns></returns>
        private static HomeworkInfo GetLastHomework(List<HomeworkInfo> homeworksList)
        {
            HomeworkInfo lastHomework = new HomeworkInfo();
            foreach (HomeworkInfo homework in homeworksList)
            {
                if (lastHomework.creationDate < homework.creationDate)
                {
                    lastHomework = homework;
                }
            }
            return lastHomework;
        }

        /// <summary>
        /// Get the current datacontext of a button (effectively returning the StudentDisplay of this student)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private StudentDisplay GetCurrentStudent(object s)
        {
            return (StudentDisplay)((FrameworkElement)s).DataContext;
        }
        /// <summary>
        /// On load function adding recursively all students to the studentCollection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Students_Load(object sender, RoutedEventArgs e)
        {
        }

        private void RetrieveLastHomeworkButton_Click(object sender, RoutedEventArgs e)
        {
            StudentDisplay currentStudent = (StudentDisplay)((FrameworkElement)sender).DataContext;
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "UPDATE homeworks SET retrieveDate = @retrieveDate WHERE homeworkId = @id";
            cmd.Parameters.AddWithValue("id", currentStudent.LastHomeWorkId);

            int currentTimestamp = (int)new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
            cmd.Parameters.AddWithValue("retrieveDate", currentTimestamp);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            currentStudent.LastHomeworkStatusColor = "Green";
            currentStudent.LastHomeworkStatusText = GlobalVariable.specialCharacter["CheckMark"];
            currentStudent.HomeworkButtonEnabled = false;
        }

        private void PunishButton_Click(object sender, RoutedEventArgs e)
        {
            StudentDisplay currentStudent = GetCurrentStudent(sender);
            MainWindow window = GlobalFunction.GetMainWindow();
            window._addFrame.Navigate(new AddPunishment(currentStudent.ID));
        }
        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            StudentDisplay currentStudent = GetCurrentStudent(sender);
            MainWindow window = GlobalFunction.GetMainWindow();
            window._mainFrame.Navigate(new StudentDetailsPage(currentStudent.ID));

        }

        private void UpvotesButton_Click(object sender, RoutedEventArgs e)
        {
            StudentDisplay currentStudent = GetCurrentStudent(sender);
            MainWindow window = GlobalFunction.GetMainWindow();
            window._addFrame.Navigate(new AddVote(true, currentStudent));
            //StudentsShared.AddVotesToDb(currentStudent.ID, true, "Upvote rapide");
            //int currentUpvoteCount = int.Parse(currentStudent.UpvotesCount);
            //currentUpvoteCount++;
            //currentStudent.UpvotesCount = currentUpvoteCount.ToString();
        }

        private void DownvotesButton_Click(object sender, RoutedEventArgs e)
        {
            StudentDisplay currentStudent = GetCurrentStudent(sender);
            MainWindow window = GlobalFunction.GetMainWindow();
            window._addFrame.Navigate(new AddVote(false, currentStudent));
            //StudentsShared.AddVotesToDb(currentStudent.ID, false, "Downvote rapide");
            //int currentDownvoteCount = int.Parse(currentStudent.DownvotesCount);
            //currentDownvoteCount++;
            //currentStudent.DownvotesCount = currentDownvoteCount.ToString();
        }
    }

    public class StudentDisplay : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool homeWorkButtonEnabled;
        private string lastHomeWorkStatusText;
        private string lastHomeworkStatusColor;
        private string note;
        private string average;
        private string upvotesCount;
        private string downvotesCount;

        public int ID { get; set; }
        public string Name { get; set; }
        public string ClassroomName { get; set; }
        public bool HomeworkButtonEnabled
        {
            get { return homeWorkButtonEnabled; }
            set 
            {
                homeWorkButtonEnabled = value;
                OnPropertyChanged();
            }
        }
        public string LastHomeworkStatusText 
        { 
            get { return lastHomeWorkStatusText; }
            set 
            {
                lastHomeWorkStatusText = value;
                OnPropertyChanged();
            }
        }
        public string LastHomeworkStatusColor 
        {
            get { return lastHomeworkStatusColor; } 
            set
            {
                lastHomeworkStatusColor = value;
                OnPropertyChanged();
            }
        }
        public int LastHomeWorkId { get; set; }
        public string Note
        { 
            get { return note; } 
            set
            {
                note = value;
                OnPropertyChanged();
            }
        }
        public string Average
        { 
            get { return average; }
            set
            {
                average = value;
                OnPropertyChanged();
            }
        }
        public string UpvotesCount 
        {
            get { return upvotesCount; }
            set
            {
                upvotesCount = value;
                OnPropertyChanged();
            } 
        }
        public string DownvotesCount
        {
            get { return downvotesCount; } 
            set
            {
                downvotesCount = value;
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
