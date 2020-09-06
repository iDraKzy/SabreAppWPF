using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SabreAppWPF.Students.StudentDetails
{
    /// <summary>
    /// Logique d'interaction pour GradesDetails.xaml
    /// </summary>
    public partial class GradesDetails : Page
    {
        private int studentId;
        public static ObservableCollection<GradeDetails> gradeCollection = new ObservableCollection<GradeDetails>();
        public GradesDetails(int studentId)
        {
            InitializeComponent();
            gradeCollection = new ObservableCollection<GradeDetails>();
            this.studentId = studentId;
        }

        private void GradesDetails_Load(object sender, RoutedEventArgs e)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = $"SELECT * FROM grades WHERE studentId = {studentId}";
            using SQLiteDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                int creationDateTimestamp = rdr.GetInt32(4);
                DateTime creationDateTime = DateTimeOffset.FromUnixTimeSeconds(creationDateTimestamp).LocalDateTime;
                GradeDetails gradeDetail = new GradeDetails()
                {
                    CreationDate = creationDateTime.ToString("g", GlobalVariable.culture),
                    Grade = rdr.GetFloat(2).ToString(),
                    Coefficient = rdr.GetInt32(3).ToString(),
                    Title = rdr.GetString(4)
                };
                gradeCollection.Add(gradeDetail);
            }
        }

        public class GradeDetails : INotifyPropertyChanged
        {
            private string _creationDate;
            private string _title;
            private string _grade;
            private string _coeff;
            public event PropertyChangedEventHandler PropertyChanged;

            public string Title
            {
                get { return _title; }
                set
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
            public string CreationDate
            {
                get { return _creationDate; }
                set
                {
                    _creationDate = value;
                    OnPropertyChanged();
                }
            }
            public string Grade
            { 
                get { return _grade; }
                set
                {
                    _grade = value;
                    OnPropertyChanged();
                }
            }
            public string Coefficient 
            {
                get { return _coeff; }
                set
                {
                    _coeff = value;
                    OnPropertyChanged();
                }
            }

            protected void OnPropertyChanged([CallerMemberName] string name = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
