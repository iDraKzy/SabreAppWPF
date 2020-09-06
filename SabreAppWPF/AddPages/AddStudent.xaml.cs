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
using System.Net.NetworkInformation;
using SabreAppWPF.Students;

namespace SabreAppWPF.AddPages
{
    /// <summary>
    /// Logique d'interaction pour AddStudent.xaml
    /// </summary>
    public partial class AddStudent : Page
    {
        public AddStudent()
        {
            InitializeComponent();
        }

        private void AddStudent_Load(object sender, RoutedEventArgs e)
        {
            string[] genderArray = new string[2] { "Homme", "Femme" };
            _genderComboBox.ItemsSource = genderArray;
            _genderComboBox.SelectedIndex = 0;

            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "SELECT classroomId, name FROM classrooms";
            using SQLiteDataReader rdr = cmd.ExecuteReader();
            List<string> classroomList = new List<string>();
            while (rdr.Read())
            {
                classroomList.Add(rdr.GetInt32(0).ToString() + " - " + rdr.GetString(1));
            }
            _classroomComboBox.ItemsSource = classroomList;
            _classroomComboBox.SelectedIndex = 0;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string surname = _surnameTextBox.Text;
            string lastname = _lastnameTextBox.Text;
            int genderIndex = _genderComboBox.SelectedIndex;
            int classroomIndex = _classroomComboBox.SelectedIndex;

            string validation = DataValidation.Student(surname, lastname);
            if (validation != "valid")
            {
                error.Content = validation;
                error.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }

            bool trueGender = genderIndex == 0;
            int classroomId = classroomIndex + 1;

            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "INSERT INTO students(classroomId, lastname, surname, gender, board, interrogation) VALUES(@classroomId, @lastname, @surname, @gender, 0, false)";
            cmd.Parameters.AddWithValue("classroomId", classroomId);
            cmd.Parameters.AddWithValue("lastname", lastname);
            cmd.Parameters.AddWithValue("surname", surname);
            cmd.Parameters.AddWithValue("gender", trueGender);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            error.Content = trueGender ? "Etudiant ajouté avec succès" : "Etudiante ajoutée avec succès";
            error.Foreground = new SolidColorBrush(Colors.Green);

            cmd.CommandText = "SELECT last_insert_rowid()";
            long studentId = (long)cmd.ExecuteScalar();

            StudentDisplay studentDisplay = new StudentDisplay()
            {
                ID = (int)studentId,
                Name = surname + " " + lastname,
                ClassroomName = Database.Get.Classroom.NameFromID(classroomId),
                LastHomeWorkId = 0,
                LastHomeworkStatusText = GlobalVariable.specialCharacter["CheckMark"],
                LastHomeworkStatusColor = "Green",
                HomeworkButtonEnabled = false,
                Average = "17.5/20",
                DownvotesCount = "0",
                UpvotesCount = "0",
                Note = "Aucune note"
            };
            studentsPage.studentsCollection.Add(studentDisplay);
        }
    }
}
