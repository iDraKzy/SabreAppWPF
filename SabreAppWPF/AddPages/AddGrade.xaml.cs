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

            int gradeId = Database.Insert.Grade.One(studentId, gradeFloat, coeffInt);

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
