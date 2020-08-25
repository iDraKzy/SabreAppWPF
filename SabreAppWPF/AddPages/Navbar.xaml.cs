using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

namespace SabreAppWPF.AddPages.Navbar_Control
{
    /// <summary>
    /// Logique d'interaction pour Navbar.xaml
    /// </summary>
    public partial class Navbar : UserControl
    {
        public MainWindow window = GlobalFunction.GetMainWindow();
        public Navbar()
        {
            InitializeComponent();
        }

        private void StudentButton_Click(object sender, RoutedEventArgs e)
        {
            window._addFrame.Navigate(new AddStudent());
        }

        private void RoomButton_Click(object sender, RoutedEventArgs e)
        {
            window._addFrame.Navigate(new AddRoom());
        }

        private void ScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            window._addFrame.Navigate(new AddSchedules());
        }

        private void ClassroomButton_Click(object sender, RoutedEventArgs e)
        {
            window._addFrame.Navigate(new AddClassroom());
        }
    }
}
