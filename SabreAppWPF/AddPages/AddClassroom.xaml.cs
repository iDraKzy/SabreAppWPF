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
using Microsoft.Toolkit.Wpf.UI.XamlHost;
using Windows.UI.Xaml.Controls;

namespace SabreAppWPF.AddPages
{
    /// <summary>
    /// Logique d'interaction pour AddClassroom.xaml
    /// </summary>
    public partial class AddClassroom : System.Windows.Controls.Page
    {
        public AddClassroom()
        {
            InitializeComponent();
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            WindowsXamlHost windowsHost = (WindowsXamlHost)_datePickerTest;
            TimePicker timePicker = (TimePicker)windowsHost.Child;
            TimeSpan? timeSelected = timePicker.SelectedTime;

        }
    }
}
