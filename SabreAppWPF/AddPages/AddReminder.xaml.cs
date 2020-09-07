using Microsoft.Toolkit.Wpf.UI.XamlHost;
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
using Windows.UI.Xaml.Controls;
using System.Data.SQLite;
//using static SabreAppWPF.MainMenu.MainMenuPage;
using SabreAppWPF.MainMenu;

namespace SabreAppWPF.AddPages
{
    /// <summary>
    /// Logique d'interaction pour AddReminder.xaml
    /// </summary>
    public partial class AddReminder : System.Windows.Controls.Page
    {
        public AddReminder()
        {
            InitializeComponent();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            
            string description = _descriptionTextBox.Text;
            DateTime? dateSelected = _DatePicker.SelectedDate;

            WindowsXamlHost windowsHost = _timePicker;
            TimePicker timePickerHour = (TimePicker)windowsHost.Child;
            TimeSpan? timeSelected = timePickerHour.SelectedTime;

            string valid = DataValidation.Reminder(description, dateSelected, timeSelected);
            if (valid != "valid")
            {
                error.Foreground = new SolidColorBrush(Colors.Red);
                error.Content = valid;
                return;
            }

            DateTime dateSelectedVerified = (DateTime)dateSelected;
            TimeSpan timeSelectedVerified = (TimeSpan)timeSelected;
            dateSelectedVerified = dateSelectedVerified.Add(timeSelectedVerified);
            DateTime currentTime = DateTime.Now;

            int reminderId = Database.Insert.Reminder.One(dateSelectedVerified, description);

            error.Foreground = new SolidColorBrush(Colors.Green);
            error.Content = "Rappel ajouté avec succès";

            MainMenuPage.ReminderGridDisplay reminder = new MainMenuPage.ReminderGridDisplay()
            {
                Content = description,
                ID = reminderId,
                CreationDate = currentTime.ToString("g", GlobalVariable.culture),
                ReminderDate = dateSelectedVerified.ToString("g", GlobalVariable.culture)
            };
            MainMenuPage.reminderGridDisplayCollection.Add(reminder);
        }
    }
}
