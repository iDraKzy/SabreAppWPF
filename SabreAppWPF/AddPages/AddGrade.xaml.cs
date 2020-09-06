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
using SabreAppWPF.Students.StudentDetails;

namespace SabreAppWPF.AddPages
{
    /// <summary>
    /// Logique d'interaction pour AddGrade.xaml
    /// </summary>
    public partial class AddGrade : Page
    {
        public AddGrade(int studentId)
        {
            InitializeComponent();

            string[] name = Database.Get.Student.NameFromID(studentId);
            _lastnameTextBox.Text = name[0];
            _surnameTextBox.Text = name[1];
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string title = _titleTextBox.Text;
            string lastname = _lastnameTextBox.Text;
            string surname = _lastnameTextBox.Text;
            string grade = _gradeTextBox.Text;
            string coeff = _coeffTextBox.Text;

            DateTime currentTime = DateTime.Now;
            int currentTimestamp = (int)new DateTimeOffset(currentTime).ToUnixTimeSeconds();

            string valid = DataValidation.Grade(title, lastname, surname, grade, coeff);
            if (valid != "valid")
            {
                error.Foreground = new SolidColorBrush(Colors.Red);
                error.Content = valid;
                return;
            }

            float gradeFloat = float.Parse(grade, GlobalVariable.culture);
            int coeffInt = int.Parse(coeff);
            int studentId = Database.Get.Student.IdFromName(lastname, surname);

            //grades(gradeId INTEGER PRIMARY KEY, studentId INTEGER, grade FLOAT, coeff INTEGER, creationDate INTEGER);

            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "INSERT INTO grades(studentId, grade, coeff, creationDate) VALUES(@studentId, @grade, @coeff, @creationDate)";
            cmd.Parameters.AddWithValue("studentId", studentId);
            cmd.Parameters.AddWithValue("grade", gradeFloat);
            cmd.Parameters.AddWithValue("coeff", coeffInt);
            cmd.Parameters.AddWithValue("creationDate", currentTimestamp);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT last_insert_rowid()";
            long gradeId = (long)cmd.ExecuteScalar();



            GradesDetails.GradeDetails gradeDetail = new GradesDetails.GradeDetails()
            {
                Title = title,
                Coefficient = coeff,
                Grade = grade,
                CreationDate = currentTime.ToString("g", GlobalVariable.culture)
            };
            GradesDetails.gradeCollection.Add(gradeDetail);

            error.Foreground = new SolidColorBrush(Colors.Green);
            error.Content = "Note ajoutée avec succès";

        }
    }
}
