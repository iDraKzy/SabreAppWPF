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

namespace SabreAppWPF
{
    /// <summary>
    /// Logique d'interaction pour AddTemplate.xaml
    /// </summary>
    public partial class AddTemplate : Page
    {
        private string type;
        private int studentId;
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
        /// Creates a ComboBox at the specified row
        /// </summary>
        /// <param name="row">Row to place the element in the grid</param>
        /// <returns>The created combobox</returns>
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
        /// <summary>
        /// Creates a DataPicker at the specified row
        /// </summary>
        /// <param name="row">The specified row</param>
        /// <returns>The created DataPicker</returns>
        private DatePicker CreateDatePicker(int row)
        {
            DatePicker tempDatePicker = new DatePicker()
            {
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 10, 0),
            };
            Grid.SetRow(tempDatePicker, row);
            Grid.SetColumn(tempDatePicker, 1);
            _addGrid.Children.Add(tempDatePicker);
            return tempDatePicker;
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
                case "student":
                    InitializeStudentCreation();
                    break;
                case "punishment":
                    InitializePunishmentCreation(studentId);
                    break;
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
                case "notes":
                    InitializeNoteCreation(studentId);
                    break;

            }
        }
        /// <summary>
        /// Initialize the add page to add students
        /// </summary>
        private void InitializeStudentCreation()
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
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
            AddPromptsLabel(promptsArray);

            //Confirmation Button
            confirmButton.Click += (s, e) =>
            {
                //Log to db
                using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();

                //Retrieve forms value
                string surname = surnameTextBox.Text;
                string lastname = lastNameTextBox.Text;
                string name = surname + " " + lastname;
                int gender = genderComboBox.SelectedIndex;
                bool trueGender = gender == 0;
                int classroomId = classroomComboBox.SelectedIndex + 1;
                string classroomName = (string)classroomComboBox.SelectedValue;

                //Validate the data
                string validation = DataValidation.Student(surname, lastname);
                if (validation != "valid")
                {
                    error.Foreground = new SolidColorBrush(Colors.Red);
                    error.Content = validation;
                    return;
                }

                //Insert to db
                cmd.CommandText = "INSERT INTO students(classroomId, lastname, surname, gender, board, interrogation) VALUES(@classroomId, @lastname, @surname, @gender, 1, false)";
                cmd.Parameters.AddWithValue("classroomId", classroomId);
                cmd.Parameters.AddWithValue("lastname", lastname);
                cmd.Parameters.AddWithValue("surname", surname);
                cmd.Parameters.AddWithValue("gender", trueGender);
                cmd.Prepare();
                int studentId = cmd.ExecuteNonQuery();
                error.Foreground = new SolidColorBrush(Colors.Green);
                if (trueGender)
                {
                    error.Content = "Etudiant ajouté avec succès";
                } else
                {
                    error.Content = "Etudiante ajoutée avec succès";
                }
                studentsPage studentsPage = (studentsPage)Application.Current.Properties["studentsPage"];

                StudentsShared.AddStudentToUI(studentsPage, studentId, name, classroomName, "Note par défaut", 0, 0);
            };
        }
        /// <summary>
        /// Initialize the add page to add punishment, auto fills the name of the student
        /// </summary>
        /// <param name="studentId">The id of the student for autofilling purposes</param>
        private void InitializePunishmentCreation(int studentId)
        {
            title.Content = "Ajouter une punition";
            string[] nameArray = StudentsShared.GetStudentNameFromID(studentId);

            TextBox surnameTextBox = CreateTextBox(1);
            surnameTextBox.Text = nameArray[1];

            TextBox lastnameTextBox = CreateTextBox(2);
            lastnameTextBox.Text = nameArray[0];

            DatePicker endDatePicker = CreateDatePicker(3);

            TextBox descriptionTextBox = CreateTextBox(4);
            descriptionTextBox.TextWrapping = TextWrapping.Wrap;
            string[] promptsArray = new string[] { "Prénom", "Nom", "Echéance", "Description" };
            AddPromptsLabel(promptsArray);

            confirmButton.Click += (s, e) =>
            {
                using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();

                //Extract data from the from
                int currentTimestamp = Convert.ToInt32(new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds());
                string surname = surnameTextBox.Text;
                string lastname = lastnameTextBox.Text;
                DateTime? endDate = endDatePicker.SelectedDate;
                string description = descriptionTextBox.Text;

                //Validate the data
                string validation = DataValidation.Punishment(surname, lastname, endDate);
                if (validation != "valid")
                {
                    error.Foreground = new SolidColorBrush(Colors.Red);
                    error.Content = validation;
                    return;
                }

                //Date is valid add punishment to the database and return success
                string name = surname + " " + lastname;
                long endTimestamp = Convert.ToInt32(new DateTimeOffset((DateTime)endDatePicker.SelectedDate).ToUnixTimeSeconds());

                cmd.CommandText = $"SELECT studentId FROM students WHERE lastname = '{lastname}' AND surname = '{surname}'";
                long? enteredStudentId = (long)cmd.ExecuteScalar();
                if (enteredStudentId == null)
                {
                    error.Foreground = new SolidColorBrush(Colors.Red);
                    error.Content = "Cet étudiant n'existe pas, vérifié l'orthographe";
                    return;
                }

                cmd.CommandText = @"INSERT INTO punishments(studentId, creationDate, endDate, retrieveDate, description)
                                    VALUES(@studentId, @creationDate, @endDate, @retrieveDate, @description)";
                cmd.Parameters.AddWithValue("studentId", enteredStudentId);
                cmd.Parameters.AddWithValue("creationDate", currentTimestamp);
                cmd.Parameters.AddWithValue("endDate", endTimestamp);
                cmd.Parameters.AddWithValue("retrieveDate", 0);
                cmd.Parameters.AddWithValue("description", description);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                error.Foreground = new SolidColorBrush(Colors.Green);
                error.Content = "Punition ajoutée avec succès!";

            };
        }

        private void InitializeNoteCreation(int studentId)
        {
            string[] nameArray = StudentsShared.GetStudentNameFromID(studentId);

            TextBox surnameTextBox = CreateTextBox(1);
            surnameTextBox.Text = nameArray[1];

            TextBox lastnameTextBox = CreateTextBox(2);
            lastnameTextBox.Text = nameArray[2];

            TextBox noteTextBox = CreateTextBox(3);

            string[] promptsLabelArray = new string[] { "Prénom", "Nom", "Remarque" };
            AddPromptsLabel(promptsLabelArray);

            confirmButton.Click += (s, e) =>
            {

            };
        }

        private void InitializeRoomCreation()
        {
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
