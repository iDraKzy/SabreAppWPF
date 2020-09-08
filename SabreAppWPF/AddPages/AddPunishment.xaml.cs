using SabreAppWPF.Students;
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
    /// Logique d'interaction pour AddPunishment.xaml
    /// </summary>
    public partial class AddPunishment : Page
    {
        public AddPunishment()
        {
            InitializeComponent();
        }

        public AddPunishment(int studentId)
        {
            InitializeComponent();
            string[] nameArray = Database.Get.Student.NameFromID(studentId);
            _lastnameTextBox.Text = nameArray[0];
            _surnameTextBox.Text = nameArray[1];
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string lastname = _lastnameTextBox.Text;
            string surname = _surnameTextBox.Text;
            string description = _descriptionTextBox.Text;
            DateTime? endDateTime = _endDatePicker.SelectedDate;

            string[] validation = DataValidation.Punishment(surname, lastname, endDateTime);
            if (validation[0] != "valid")
            {
                error.Content = validation[0];
                error.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }

            DateTime currenteDate = DateTime.Now;
            int studentId = int.Parse(validation[1]);

            int punishmentId = Database.Insert.Punishment.One(studentId, (DateTime)endDateTime, description);

            PunishmentDetails punishmentDetail = new PunishmentDetails()
            {
                ID = punishmentId,
                Date = currenteDate.ToString("g", GlobalVariable.culture),
                EndDate = endDateTime?.ToString("d", GlobalVariable.culture),
                Returned = "",
                ButtonEnabled = true,
                ForegroundColor = "Black",
                StatusColor = "Transparent",
                Description = description
            };

            PunishmentsDetails.punishmentsList.Add(punishmentDetail);

            if (punishmentId != 0)
            {
                error.Foreground = new SolidColorBrush(Colors.Green);
                error.Content = "Punition ajoutée avec succès";
            }
        }

    }
}
