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
using SabreAppWPF.Classrooms.Options;

namespace SabreAppWPF.AddPages
{
    /// <summary>
    /// Logique d'interaction pour AddSchedules.xaml
    /// </summary>
    public partial class AddSchedules : System.Windows.Controls.Page
    {
        List<ClassroomEntry> classromsList = new List<ClassroomEntry>();
        List<RoomEntry> roomsList = new List<RoomEntry>();
        public AddSchedules()
        {
            InitializeComponent();
            //Handle classrooms
            List<ClassroomInfo> allClassroomsList = Database.Get.Classroom.All();
            foreach (ClassroomInfo classroom in allClassroomsList)
            {
                ClassroomEntry classroomEntry = new ClassroomEntry()
                {
                    ID = classroom.ClassroomId,
                    Name = classroom.Name
                };
                classromsList.Add(classroomEntry);
            }
            _classroomComboBox.ItemsSource = classromsList;
            _classroomComboBox.SelectedIndex = 0;

            //Handle Rooms
            List<RoomInfo> allRoomsList = Database.Get.Room.All();
            foreach (RoomInfo room in allRoomsList)
            {
                RoomEntry roomEntry = new RoomEntry()
                {
                    ID = room.RoomId,
                    Name = room.Name
                };
                roomsList.Add(roomEntry);
            }
            _roomNameComboBox.ItemsSource = roomsList;
            _roomNameComboBox.SelectedIndex = 0;

            //Handle Repetitivity
            List<string> allRepetitivity = new List<string>() { "Une fois par semaine", "Une semaine sur deux" };
            _regularityComboBox.ItemsSource = allRepetitivity;
            _regularityComboBox.SelectedIndex = 0;

        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            error.Content = "";
            ClassroomEntry selectedClassroom = (ClassroomEntry)_classroomComboBox.SelectedItem;
            RoomEntry selectedRoom = (RoomEntry)_roomNameComboBox.SelectedItem;
            int repetitivity = _regularityComboBox.SelectedIndex;

            WindowsXamlHost windowsHost = _timePicker;
            TimePicker timePickerHour = (TimePicker)windowsHost.Child;

            WindowsXamlHost windowsHost2 = _durationTimePicker;
            TimePicker durationTimePicker = (TimePicker)windowsHost2.Child;
            TimeSpan? durationTimeSelected = durationTimePicker.SelectedTime;
            if (durationTimeSelected == null)
            {
                error.Foreground = new SolidColorBrush(Colors.Red);
                error.Content = "Spécifier une durée";
                return;
            }
            TimeSpan? hourTimeSelected = timePickerHour.SelectedTime;
            if (hourTimeSelected == null)
            {
                error.Foreground = new SolidColorBrush(Colors.Red);
                error.Content = "Spécifier une heure.";
                return;
            }
            DateTime? dateSelected = _firstDatePciker.SelectedDate; 
            if (dateSelected == null)
            {
                error.Foreground = new SolidColorBrush(Colors.Red);
                error.Content = "Spécifier une date de première séance";
                return;
            }

            DateTime dateSelectedNotNull = (DateTime)dateSelected;
            dateSelectedNotNull = dateSelectedNotNull.AddHours((double)hourTimeSelected?.Hours).AddMinutes((double)hourTimeSelected?.Minutes);

            int scheduleId = Database.Insert.Schedule.One(selectedClassroom.ID, selectedRoom.ID, repetitivity, dateSelectedNotNull, (TimeSpan)hourTimeSelected);

            ScheduleOption.ScheduleOptionDisplay scheduleDisplay = new ScheduleOption.ScheduleOptionDisplay()
            {
                ID = scheduleId,
                ClassroomId = selectedClassroom.ID,
                NextDate = dateSelectedNotNull.ToString("g", GlobalVariable.culture),
                Duration = durationTimeSelected?.ToString(@"hh\:mm"),
                Repetitivity = repetitivity == 0 ? "Une fois par semaine" : "Une semaine sur deux",
                Room = Database.Get.Room.NameFromID(selectedRoom.ID)
            };

            ScheduleOption.scheduleDisplayCollection.Add(scheduleDisplay);
            error.Foreground = new SolidColorBrush(Colors.Green);
            error.Content = "Horraire ajouté avec succès";
        }

        public class RoomEntry
        {
           public int ID { get; set; }
           public string Name { get; set; }
        }

        public class ClassroomEntry
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }
    }
}
