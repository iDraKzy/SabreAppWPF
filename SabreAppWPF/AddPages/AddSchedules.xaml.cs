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
            //Handle week day
            List<string> weekDayList = new List<string>() { "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi", "Dimanche" };
            _weekDayComboBox.ItemsSource = weekDayList;
            _weekDayComboBox.SelectedIndex = 0;
            //Handle classrooms
            List<ClassroomInfo> allClassroomsList = Getter.GetAllClassrooms();
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
            List<RoomInfo> allRoomsList = Getter.GetAllRooms();
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

        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            error.Content = "";
            ClassroomEntry selectedClassroom = (ClassroomEntry)_classroomComboBox.SelectedItem;
            RoomEntry selectedRoom = (RoomEntry)_roomNameComboBox.SelectedItem;
            int weekDay = _weekDayComboBox.SelectedIndex;

            WindowsXamlHost windowsHost = _timePicker;
            TimePicker timePicker = (TimePicker)windowsHost.Child;
            TimeSpan? timeSelected = timePicker.SelectedTime;
            if (timeSelected == null)
            {
                error.Foreground = new SolidColorBrush(Colors.Red);
                error.Content = "Spécifier une heure.";
            }

            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "INSERT INTO schedules(classroomId, roomId, weekDay, hour, minute) VALUES(@classroomId, @roomId, @weekDay, @hour, @minute)";
            cmd.Parameters.AddWithValue("classroomId", selectedClassroom.ID);
            cmd.Parameters.AddWithValue("roomId", selectedRoom.ID);
            cmd.Parameters.AddWithValue("weekDay", weekDay);
            cmd.Parameters.AddWithValue("hour", timeSelected?.Hours);
            cmd.Parameters.AddWithValue("minute", timeSelected?.Minutes);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
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
