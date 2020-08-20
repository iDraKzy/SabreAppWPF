using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

namespace SabreAppWPF.Classrooms
{
    /// <summary>
    /// Logique d'interaction pour ClassroomsPage.xaml
    /// </summary>
    public partial class ClassroomsPage : Page
    {
        public static ObservableCollection<ClassroomDisplay> classroomCollection = new ObservableCollection<ClassroomDisplay>();
        public ClassroomsPage()
        {
            InitializeComponent();
            ClassroomsShared.GetNextSessionTimestamp(0, 0, 0);
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "SELECT * FROM classrooms";

            using SQLiteDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                int classroomId = rdr.GetInt32(0);
                int studentNumber = GetStudentNumber(classroomId);
                string studentNumberString = studentNumber.ToString() + "Etudiant(e)s";
                ClassroomDisplay classroomDisplay = new ClassroomDisplay()
                {
                    ID = classroomId,
                    ClassroomName = rdr.GetString(2),
                    StudentsNumber = studentNumberString,
                    NextSession = "",
                };
                classroomCollection.Add(classroomDisplay);
            }
        }

        private int GetStudentNumber(int classroomId)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "SELECT * FROM students WHERE classroomId = @classroomId";
            cmd.Parameters.AddWithValue("classroomId", classroomList);
            cmd.Prepare();
            using SQLiteDataReader rdr = cmd.ExecuteReader();
            int studentNumber = 0;
            while (rdr.Read())
            {
                studentNumber++;
            }
            rdr.Close();
            return studentNumber;
        }
    }

    public class ClassroomDisplay : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _id;
        private string _classroomName;
        private string _studentsNumber;
        private string _nextSession;
        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }
        public string ClassroomName
        {
            get { return _classroomName; }
            set
            {
                _classroomName = value;
                OnPropertyChanged();
            }
        }
        public string StudentsNumber
        {
            get { return StudentsNumber; }
            set
            {
                _studentsNumber = value;
                OnPropertyChanged();
            }
        }
        public string NextSession
        {
            get { return _nextSession; }
            set
            {
                _nextSession = value;
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
