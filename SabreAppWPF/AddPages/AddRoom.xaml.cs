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
using Windows.UI.Xaml.Automation.Peers;
using System.Data.SQLite;

namespace SabreAppWPF.AddPages
{
    /// <summary>
    /// Logique d'interaction pour AddRoom.xaml
    /// </summary>
    public partial class AddRoom : Page
    {
        public AddRoom()
        {
            InitializeComponent();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string name = _nameTextBox.Text;
            string row = _rowTextBox.Text;
            string column = _columnTextBox.Text;

            string valid = DataValidation.Room(name, row, column);
            if (valid != "valid")
            {
                error.Content = valid;
                error.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }

            int rows = int.Parse(row);
            int columns = int.Parse(column);

            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "INSERT INTO rooms(name, rows, columns) VALUES(@name, @rows, @colulns)";
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("rows", rows);
            cmd.Parameters.AddWithValue("columns", columns);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            error.Content = "Salle ajoutée avec succès !";
            error.Foreground = new SolidColorBrush(Colors.Green);
        }
    }
}
