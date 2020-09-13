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
using Windows.UI.Xaml.Automation.Peers;
using Microsoft.Win32;
using System.IO;

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

        private void CsvAddButton_Click(object sender, RoutedEventArgs e)
        {
            string[] csvFile;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Fichier CSV (*.csv)|*.csv|Tous les fichiers (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                csvFile = File.ReadAllLines(openFileDialog.FileName);
                ProcessCsvFile(csvFile);
            }
        }

        private void ProcessCsvFile(string[] csvLines)
        {
            List<RowStudent> rowStudents = new List<RowStudent>();
            for (int i = 0; i < csvLines.Length; i++)
            {
                if (i == 0) continue;
                string[] properties = csvLines[i].Split(";");
                RowStudent rowStudent = new RowStudent()
                {
                    Name = properties[0],
                    Surname = properties[1],
                    Gender = properties[2],
                    Classrooms = properties[3]
                };
                rowStudents.Add(rowStudent);
            }
            AddAllStudentsToDbFromCSV(rowStudents);

        }

        private void AddAllStudentsToDbFromCSV(List<RowStudent> rowStudents)
        {
            foreach (RowStudent student in rowStudents)
            {
                student.Gender = student.Gender.ToLower();
                student.Name = student.Name.ToLower();
                student.Name = char.ToUpper(student.Name[0]) + student.Name.Substring(1);
                bool trueGender = student.Gender == "h";
                string[] classrooms = student.Classrooms.Split(",");
                List<ClassroomEntry> classroomEntryList = new List<ClassroomEntry>();
                foreach (string classroomName in classrooms)
                {
                    int? classroomId = Database.Get.Classroom.IDFromName(classroomName);
                    ClassroomEntry classroomEntry = new ClassroomEntry()
                    {
                        Name = classroomName,
                        ID = classroomId
                    };
                    classroomEntryList.Add(classroomEntry);
                }

                foreach (ClassroomEntry classroom in classroomEntryList)
                {
                    if (classroom.ID == null)
                    {
                        classroom.ID = Database.Insert.Classroom.One(classroom.Name);
                    }
                }

                int studentId = Database.Insert.Student.One((int)classroomEntryList[0].ID, student.Name, student.Surname, trueGender);
                if (classroomEntryList.Count > 1)
                {
                    for (int i = 0; i < classroomEntryList.Count; i++)
                    {
                        if (i == 0) continue;
                        Database.Insert.LinkStudentToClassroom.One(studentId, (int)classroomEntryList[i].ID);
                    }
                }

            }

            error.Foreground = new SolidColorBrush(Colors.Green);
            error.Content = $"{rowStudents.Count} étudiant(e)s ajouté(e)s avec succès";
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

            int studentId = Database.Insert.Student.One(classroomId, lastname, surname, trueGender);

            error.Content = trueGender ? "Etudiant ajouté avec succès" : "Etudiante ajoutée avec succès";
            error.Foreground = new SolidColorBrush(Colors.Green);

            StudentDisplay studentDisplay = new StudentDisplay()
            {
                ID = studentId,
                Name = surname + " " + lastname,
                ClassroomName = Database.Get.Classroom.NameFromID(classroomId),
                LastHomeWorkId = 0,
                LastHomeworkStatusText = GlobalVariable.specialCharacter["CheckMark"],
                LastHomeworkStatusColor = "Green",
                HomeworkButtonEnabled = false,
                Average = "20/20",
                DownvotesCount = "0",
                UpvotesCount = "0",
                Note = "Aucune note"
            };
            studentsPage.studentsCollection.Add(studentDisplay);
        }

        public class ClassroomEntry
        {
            public string Name { get; set; }
            public int? ID { get; set; }
        }

        public class RowStudent
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Gender { get; set; }
            public string Classrooms { get; set; }
        }
    }
}
