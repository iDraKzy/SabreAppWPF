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
using System.Data.SQLite;
using SabreAppWPF.Classrooms;

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

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (_nameTextBox.Text == null)
            {
                error.Foreground = new SolidColorBrush(Colors.Red);
                error.Content = "Nom obligatoire";
                return;
            }

            string name = _nameTextBox.Text;
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "INSERT INTO classrooms(name) VALUES(@name)";
            cmd.Parameters.AddWithValue("name", name);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT last_insert_rowid()";

            long classroomId = (long)cmd.ExecuteScalar();

            ClassroomDisplay classroomDisplay = new ClassroomDisplay()
            {
                ClassroomName = name,
                ID = (int)classroomId,
                NextSession = "Aucun horraire défini",
                StudentsNumber = "0 étudiant(e)s"
            };
            ClassroomsPage.classroomCollection.Add(classroomDisplay);
        }
    }
}
