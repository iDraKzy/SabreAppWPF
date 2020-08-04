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

namespace SabreAppWPF
{
    /// <summary>
    /// Logique d'interaction pour AddTemplate.xaml
    /// </summary>
    public partial class AddTemplate : Page
    {
        private string type;
        public AddTemplate(string type)
        {
            InitializeComponent();
            this.type = type;
        }

        private void AddPromptsLabel(int number, string[] prompts)
        {
            for (int i = 0; i < number; i++)
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

        private ComboBox CreateComboBox(int row)
        {
            ComboBox tempComboBox = new ComboBox()
            {
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 10, 0),
                SelectedIndex = 0
            };
            Grid.SetRow(tempComboBox, row);
            Grid.SetColumn(tempComboBox, 1);
            _addGrid.Children.Add(tempComboBox);
            return tempComboBox;
        }

        private void AddTemplate_Load(object sender, RoutedEventArgs e)
        {
            using SQLiteConnection connection = new SQLiteConnection("Data Source=" + GlobalVariable.path);
            connection.Open();
            using SQLiteCommand cmd = new SQLiteCommand(connection);
            switch(this.type)
            {
                case "student":
                    //Change title
                    title.Content = "Ajouter un étudiant";

                    //Add controls
                    //Surname
                    TextBox surnameTextBox = CreateTextBox(1);

                    //Name
                    TextBox lastNameTextBox = CreateTextBox(2);

                    //Gender
                    ComboBox genderComboBox = CreateComboBox(3);
                    string[] genderArray = new string[] { "Homme", "Femme" };
                    genderComboBox.ItemsSource = genderArray;

                    //classroom
                    ComboBox classroomComboBox = CreateComboBox(4);
                    cmd.CommandText = "SELECT * FROM classrooms";
                    SQLiteDataReader rdrClassroom = cmd.ExecuteReader();
                    List<string> classrommsNameList = new List<string>();

                    while (rdrClassroom.Read())
                    {
                        classrommsNameList.Add(rdrClassroom.GetString(2));
                    }
                    rdrClassroom.Close();
                    classroomComboBox.ItemsSource = classrommsNameList;

                    //Add Labels
                    string[] promptsArray = new string[] { "Prénom", "Nom", "Sexe", "Classe" };
                    AddPromptsLabel(4, promptsArray);

                    //Confirmation Button
                    confirmButton.Click += (s, e) =>
                    {
                        //Log to db
                        using SQLiteConnection connection = new SQLiteConnection("Data Source=" + GlobalVariable.path);
                        connection.Open();
                        using SQLiteCommand cmd = new SQLiteCommand(connection);

                        //Retrieve forms value
                        string name = surnameTextBox.Text + " " + lastNameTextBox.Text;
                        int gender = genderComboBox.SelectedIndex;
                        bool trueGender = true;
                        trueGender = gender == 0;
                        int classroomId = classroomComboBox.SelectedIndex + 1;
                        string classroomName = (string)classroomComboBox.SelectedValue;

                        //Insert to db
                        cmd.CommandText = "INSERT INTO students(classroomId, name, gender, board, interrogation) VALUES(@classroomId, @name, @gender, 1, false)";
                        cmd.Parameters.AddWithValue("classroomId", classroomId);
                        cmd.Parameters.AddWithValue("name", name);
                        cmd.Parameters.AddWithValue("gender", trueGender);
                        cmd.Prepare();
                        int studentId = cmd.ExecuteNonQuery();

                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(MainWindow))
                            {
                                (window as MainWindow)._mainFrame.Navigate(new studentsPage());
                            }
                        }
                    };
                    break;
                case "punishment":
                    break;
                case "classroom":
                    break;
                case "room":
                    break;
                case "upvote":
                    break;
                case "downvote":
                    break;
                case "homework":
                    break;
            }
        }
    }

}
