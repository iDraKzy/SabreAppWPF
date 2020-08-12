using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Application.Current.Properties["studentsPage"] = new studentsPage();
        }

        private void Main_Load(object sender, RoutedEventArgs e)
        {
            using SQLiteConnection connection = new SQLiteConnection("Data Source=" + GlobalVariable.path);
            connection.Open();
            using SQLiteCommand cmd = new SQLiteCommand(connection)
            {
                CommandText = @"CREATE TABLE IF NOT EXISTS
                                students(studentId INTEGER PRIMARY KEY, classroomId INTEGER, lastname TEXT, surname TEXT, gender BOOLEAN, board INTEGER, interrogation BOOLEAN);

                                CREATE TABLE IF NOT EXISTS
                                homeworks(homeworkId INTEGER PRIMARY KEY, studentId INTEGER, creationDate INTEGER, endDate INTEGER, retrieveDate INTEGER, description TEXT);

                                CREATE TABLE IF NOT EXISTS
                                punishments(punishmentId INTEGER PRIMARY KEY, studentId INTEGER,
                                creationDate INTEGER, endDate INTEGER, retrieveDate INTEGER, description TEXT);

                                CREATE TABLE IF NOT EXISTS
                                notes(noteId INTEGER PRIMARY KEY, studentId INTEGER, creationDate INTEGER, content TEXT);
                                
                                CREATE TABLE IF NOT EXISTS
                                grades(gradeId INTEGER PRIMARY KEY, studentId INTEGER, grade FLOAT, coeff INTEGER, creationDate INTEGER);

                                CREATE TABLE IF NOT EXISTS
                                votes(voteId INTEGER PRIMARY KEY, studentId INTEGER, upvotes BOOLEAN, description TEXT, creationDate INTEGER);

                                CREATE TABLE IF NOT EXISTS
                                rooms(roomId INTEGER PRIMARY KEY, name TEXT, rows INTEGER, columns INTEGER);

                                CREATE TABLE IF NOT EXISTS
                                classrooms(classroomId INTEGER PRIMARY KEY, planId INTEGER, name TEXT);

                                CREATE TABLE IF NOT EXISTS
                                schedules(scheduleId INTEGER PRIMARY KEY, classroomId INTEGER, roomId INTEGER);

                                CREATE TABLE IF NOT EXISTS
                                classTimes(classTimeId INTEGER PRIMARY KEY, scheduleId INTEGER, weekDay INTEGER, timeOfDay INTEGER);

                                CREATE TABLE IF NOT EXISTS
                                plans(planId INTEGER PRIMARY KEY, scheduleId INTEGER);

                                CREATE TABLE IF NOT EXISTS
                                places(placeId INTEGER PRIMARY KEY, planId INTEGER, studentId INTEGER, row INTEGER, column INTEGER);

                                CREATE TABLE IF NOT EXISTS
                                reminders(reminderId INTEGER PRIMARY KEY, creationDate INTEGER, reminderDate INTEGER, description TEXT);"
            };
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO classrooms(planId, name) VALUES(@planId, @name)";
            cmd.Parameters.AddWithValue("planId", 1);
            cmd.Parameters.AddWithValue("name", "103");
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.CommandText = $"INSERT INTO punishments(studentId, creationDate, endDate, retrieveDate, description) VALUES(1, 1000000, 10000022, 0, 'Test5')";
            cmd.ExecuteNonQuery();

            for (int i = 0; i < 2; i++)
            {
                cmd.CommandText = "INSERT INTO students(classroomId, lastname, surname, gender, board, interrogation) VALUES(@classroomId, @lastname, @surname, @gender, @board, @interrogation)";
                cmd.Parameters.AddWithValue("classroomId", 1);
                if (i % 2 == 0)
                {
                    cmd.Parameters.AddWithValue("lastname", "Clas");
                    cmd.Parameters.AddWithValue("surname", "Emeric");

                } else
                {
                    cmd.Parameters.AddWithValue("lastname", "Collard");
                    cmd.Parameters.AddWithValue("surname", "Youlan");
                }
                cmd.Parameters.AddWithValue("board", 0);
                cmd.Parameters.AddWithValue("gender", true);
                cmd.Parameters.AddWithValue("interrogation", false);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }

            cmd.CommandText = "INSERT INTO notes(studentId, creationDate, content) VALUES(1, 1596385214, 'Bon élève mais bruyant')";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO homeworks(studentId, creationDate, endDate, retrieveDate, description) VALUES(1, 1596385213, 1597000000, 0, 'Test 2')";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO homeworks(studentId, creationDate, endDate, retrieveDate, description) VALUES(1, 1596385216, 1595000000, 0, 'Test 1')";
            cmd.ExecuteNonQuery();
            connection.Close();

            //cmd.CommandText = "INSERT INTO homeworks(studentId, creationDate, endDate, retrieveDate, description) VALUES(1, 1596385214, ";
        }

        private void Students_Button_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(Application.Current.Properties["studentsPage"]);
            _addFrame.Navigate(new AddTemplate("student"));
        }

        private void Main_Button_Click(object sender, RoutedEventArgs e)
        {
            //_mainFrame.Navigate(new studentsPage());
        }
    }
}
