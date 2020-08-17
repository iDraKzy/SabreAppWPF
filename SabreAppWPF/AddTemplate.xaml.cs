using System;
using System.Collections.Generic;
using System.Drawing.Printing;
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
using SabreAppWPF.Students;
using System.Xml;
using System.Diagnostics.Contracts;
using SabreAppWPF.Students.StudentDetails;

namespace SabreAppWPF
{
    /// <summary>
    /// Logique d'interaction pour AddTemplate.xaml
    /// </summary>
    public partial class AddTemplate : Page
    {
        private string type;
        private int studentId;

        //TODO: Refactor this shit
        public AddTemplate(string type, int studentId = 0)
        {
            InitializeComponent();
            this.type = type;
            this.studentId = studentId;
        }

        /// <summary>
        /// Add labels recursively based on the length of the array of string
        /// </summary>
        /// <param name="prompts"></param>
        private void AddPromptsLabel(string[] prompts)
        {
            for (int i = 0; i < prompts.Length; i++)
            {
                Label tempLabel = new Label
                {
                    Content = prompts[i],
                    FontFamily = new FontFamily("Segoe UI Semibold"),
                    FontSize = 12,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(10, 0, 0, 0),
                    Width = Double.NaN
                };
                Grid.SetRow(tempLabel, i + 1);
                Grid.SetColumn(tempLabel, 0);
                _addGrid.Children.Add(tempLabel);
            }
        }
        /// <summary>
        /// Create Textbox at the specified row
        /// </summary>
        /// <param name="row">The row number</param>
        /// <returns>The created textbox</returns>
        private TextBox CreateTextBox(int row)
        {
            TextBox tempTextBox = new TextBox()
            {
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 10, 0),
            };
            Grid.SetRow(tempTextBox, row);
            Grid.SetColumn(tempTextBox, 1);
            _addGrid.Children.Add(tempTextBox);
            return tempTextBox;
        }

        /// <summary>
        /// Call the right initalizing function for the Add page
        /// </summary>
        /// <param name="sender"/>
        /// <param name="e"/>
        private void AddTemplate_Load(object sender, RoutedEventArgs e)
        {
            switch(this.type)
            {
                case "classroom":
                    //InitializeClassroomCreation();
                    break;
                case "room":
                    InitializeRoomCreation();
                    break;
                case "upvote":
                    //InitializeVoteCreation(true);
                    break;
                case "downvote":
                    //InitializeVoteCreation(false);
                    break;
                case "homework":
                    //InitializeHomeworkCreation();
                    break;
                case "grades":
                    //InitializeGradeCreation(studentId);
                    break;
            }
        }
        private void InitializeRoomCreation()
        {
            title.Content = "Ajouter une salle";
            TextBox nameTextBox = CreateTextBox(1);

            TextBox rowTextBox = CreateTextBox(2);

            TextBox columnTextBox = CreateTextBox(3);
            string[] promptsArray = new string[] { "Nom", "Nombre de lignes", "Nombre de colonnes" };
            AddPromptsLabel(promptsArray);

            confirmButton.Click += (s, e) =>
            {
                using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();

                string name = nameTextBox.Text;

                string rows = rowTextBox.Text;

                string columns = columnTextBox.Text;

                string validation = DataValidation.Room(name, rows, columns);
                if (validation != "valid")
                {
                    error.Foreground = new SolidColorBrush(Colors.Red);
                    error.Content = validation;
                    return;
                }
                int rowNumbers = int.Parse(rows);

                int columnNumbers = int.Parse(columns);

                cmd.CommandText = "INSERT INTO rooms(name, rows, columns) VALUES(@name, @rows, @columns";
                cmd.Parameters.AddWithValue("name", name);
                cmd.Parameters.AddWithValue("rows", rowNumbers);
                cmd.Parameters.AddWithValue("columns", columnNumbers);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                error.Foreground = new SolidColorBrush(Colors.Green);
                error.Content = $"Salle {name} ajoutée avec succès";
            };
        }
    }

}
