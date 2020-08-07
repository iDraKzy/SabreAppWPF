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
using System.Windows.Shapes;

namespace SabreAppWPF.Students.StudentDetails
{
    /// <summary>
    /// Logique d'interaction pour StudentDetailsPage.xaml
    /// </summary>
    public partial class StudentDetailsPage : Page
    {
        public int studentId;
        public StudentDetailsPage(int studentId)
        {
            InitializeComponent();
            this.studentId = studentId;
        }

        private void PunishmentButton_Click(object sender, RoutedEventArgs e)
        {
            _detailsFrame.Navigate(new PunishmentsDetails(studentId));
        }
    }
}
