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
using SabreAppWPF.AddPages;
using SabreAppWPF.LightDark;
using SabreAppWPF.Classrooms;
using SabreAppWPF.MainMenu;
using SabreAppWPF.Plans;
using Windows.Storage;
using System.IO;
using Windows.UI.Xaml.Automation.Peers;
using System.Threading;
using Windows.ApplicationModel.Contacts;

namespace SabreAppWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Task.Run(() => CreateDb());
            InitializeComponent();
            //TODO: Eleve peuvent être dans deux groupes
#if DEBUG
            //cmd.CommandText = "INSERT INTO classrooms(name) VALUES(@name)";
            //cmd.Parameters.AddWithValue("name", "103");
            //cmd.Prepare();
            //cmd.ExecuteNonQuery();
            //cmd.CommandText = $"INSERT INTO punishments(studentId, creationDate, endDate, retrieveDate, description) VALUES(1, 1000000, 10000022, 0, 'Test5')";
            //cmd.ExecuteNonQuery();

            //for (int i = 0; i < 2; i++)
            //{
            //    cmd.CommandText = "INSERT INTO students(classroomId, lastname, surname, gender, board, interrogation) VALUES(@classroomId, @lastname, @surname, @gender, @board, @interrogation)";
            //    cmd.Parameters.AddWithValue("classroomId", 1);
            //    if (i % 2 == 0)
            //    {
            //        cmd.Parameters.AddWithValue("lastname", "Clas");
            //        cmd.Parameters.AddWithValue("surname", "Emeric");

            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("lastname", "Collard");
            //        cmd.Parameters.AddWithValue("surname", "Youlan");
            //    }
            //    cmd.Parameters.AddWithValue("board", 0);
            //    cmd.Parameters.AddWithValue("gender", true);
            //    cmd.Parameters.AddWithValue("interrogation", false);
            //    cmd.Prepare();
            //    cmd.ExecuteNonQuery();
            //}

            //cmd.CommandText = "INSERT INTO notes(studentId, creationDate, content) VALUES(1, 1596385214, 'Bon élève mais bruyant')";
            //cmd.ExecuteNonQuery();

            //cmd.CommandText = "INSERT INTO homeworks(studentId, creationDate, endDate, retrieveDate, description) VALUES(1, 1596385213, 1597000000, 0, 'Test 2')";
            //cmd.ExecuteNonQuery();

            //cmd.CommandText = "INSERT INTO homeworks(studentId, creationDate, endDate, retrieveDate, description) VALUES(1, 1596385216, 1595000000, 0, 'Test 1')";
            //cmd.ExecuteNonQuery();
#endif
            //cmd.CommandText = "INSERT INTO homeworks(studentId, creationDate, endDate, retrieveDate, description) VALUES(1, 1596385214, ";

            //Application.Current.Properties["studentsPage"] = new studentsPage();

            try
            {
                var v = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", "1");
                if (v != null && v.ToString() == "0")
                    GlobalVariable.isLightMode = false;
            }
            catch {}

            //GlobalVariable.isLightMode = true;
            if (!GlobalVariable.isLightMode)
            {
                AppTheme.BackgroundColor = "#202020";
                //AppTheme.TextColor = Colors.White;
                AppTheme.BackgroundGridRowColor = "#333333";
                AppTheme.TextColor = "#f2f2f2";
                AppTheme.ButtonHoverColor = "#404040";
                AppTheme.ButtonClickColor = "#007acc";
                AppTheme.BorderColor = "#404040";
                AppTheme.ButtonTextDisabledColor = "#878787";
            }
            else
            {
                AppTheme.BackgroundColor = "#FFFFFF";
                AppTheme.BackgroundGridRowColor = "#FFFFFF";
                AppTheme.TextColor = "#000000";
                AppTheme.ButtonHoverColor = "#DCDCDC";
                AppTheme.ButtonClickColor = "#c9c9c9";
                AppTheme.ButtonTextDisabledColor = "#878787";
                AppTheme.BorderColor = "#000000";
            }
        }
        //f
        private async void CreateDb()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync(GlobalVariable.currentDbName, CreationCollisionOption.OpenIfExists);
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS
                                students(studentId INTEGER PRIMARY KEY, lastname TEXT, surname TEXT, gender BOOLEAN, board BOOLEAN, interrogation BOOLEAN, mask INTEGER); --Mask new in 1.1.0.0

                                CREATE TABLE IF NOT EXISTS
                                linkStudentToClassroom(linkId INTERGER PRIMARY KEY, studentId INTEGER, classroomId INTEGER);

                                CREATE TABLE IF NOT EXISTS
                                homeworks(homeworkId INTEGER PRIMARY KEY, studentId INTEGER, creationDate INTEGER, endDate INTEGER, retrieveDate INTEGER, description TEXT, active BOOLEAN);

                                CREATE TABLE IF NOT EXISTS
                                punishments(punishmentId INTEGER PRIMARY KEY, studentId INTEGER,
                                creationDate INTEGER, endDate INTEGER, retrieveDate INTEGER, description TEXT, active BOOLEAN);

                                CREATE TABLE IF NOT EXISTS
                                notes(noteId INTEGER PRIMARY KEY, studentId INTEGER, creationDate INTEGER, content TEXT, active BOOLEAN);
                                
                                CREATE TABLE IF NOT EXISTS
                                grades(gradeId INTEGER PRIMARY KEY, studentId INTEGER, grade FLOAT, coeff INTEGER, creationDate INTEGER, title TEXT, active BOOLEAN);

                                CREATE TABLE IF NOT EXISTS
                                votes(voteId INTEGER PRIMARY KEY, studentId INTEGER, upvotes BOOLEAN, description TEXT, creationDate INTEGER, active BOOLEAN);

                                CREATE TABLE IF NOT EXISTS
                                rooms(roomId INTEGER PRIMARY KEY, name TEXT, rows INTEGER, columns INTEGER);

                                CREATE TABLE IF NOT EXISTS
                                classrooms(classroomId INTEGER PRIMARY KEY, name TEXT);

                                CREATE TABLE IF NOT EXISTS
                                schedules(scheduleId INTEGER PRIMARY KEY, classroomId INTEGER, roomId INTEGER, repetitivity INTEGER, nextDate INTEGER, duration INTEGER, active BOOLEAN);

                                CREATE TABLE IF NOT EXISTS
                                plans(planId INTEGER PRIMARY KEY, scheduleId INTEGER, roomId INTEGER, spacing TEXT, name TEXT, active BOOLEAN);

                                CREATE TABLE IF NOT EXISTS
                                places(placeId INTEGER PRIMARY KEY, planId INTEGER, studentId INTEGER, row INTEGER, column INTEGER);

                                CREATE TABLE IF NOT EXISTS
                                pairs(pairId INTEGER PRIMARY KEY, studentId1 INTEGER, studentId2 INTEGER, classroomId INTEGER);

                                CREATE TABLE IF NOT EXISTS
                                reminders(reminderId INTEGER PRIMARY KEY, creationDate INTEGER, reminderDate INTEGER, description TEXT, active BOOLEAN);"; //Spacing in plans is a string of comma seperated int
            cmd.ExecuteNonQuery();

#if DEBUG
            cmd.CommandText = "DELETE FROM schedules WHERE scheduleId = 1";
            cmd.ExecuteNonQuery();
#endif

            List<string> columnList = ReadColumnName("students");

            if (columnList.Contains("classroomId"))
            {
                cmd.CommandText = "SELECT * FROM students";
                using SQLiteDataReader rdr = cmd.ExecuteReader();
                List<StudentInfo> studentList = new List<StudentInfo>();
                if (!columnList.Contains("mask"))
                {
                    while (rdr.Read())
                    {
                        StudentInfo student = new StudentInfo()
                        {
                            studentId = rdr.GetInt32(0),
                            lastname = rdr.GetString(2),
                            surname = rdr.GetString(3),
                            gender = rdr.GetBoolean(4),
                            board = rdr.GetBoolean(5),
                            interrogation = rdr.GetBoolean(6),
                            mask = 0
                        };
                        studentList.Add(student);
                    }
                    rdr.Close();

                } else
                {
                    while (rdr.Read())
                    {
                        StudentInfo student = new StudentInfo()
                        {
                            studentId = rdr.GetInt32(0),
                            lastname = rdr.GetString(2),
                            surname = rdr.GetString(3),
                            gender = rdr.GetBoolean(4),
                            interrogation = rdr.GetBoolean(5),
                            mask = rdr.GetInt32(6)
                        };
                        studentList.Add(student);
                    }
                    rdr.Close();
                }
                cmd.CommandText = "DROP TABLE students";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE TABLE students(studentId INTEGER PRIMARY KEY, lastname TEXT, surname TEXT, gender BOOLEAN, board BOOLEAN, interrogation BOOLEAN, mask INTEGER);";
                cmd.ExecuteNonQuery();
                foreach (StudentInfo student in studentList)
                {
                    cmd.CommandText = "INSERT INTO students(studentId, lastname, surname, gender, board, interrogation, mask) VALUES(@studentId, @lastname, @surname, @gender, @board, @interrogation, @mask)";
                    cmd.Parameters.AddWithValue("studentId", student.studentId);
                    cmd.Parameters.AddWithValue("lastname", student.lastname);
                    cmd.Parameters.AddWithValue("surname", student.surname);
                    cmd.Parameters.AddWithValue("gender", student.gender);
                    cmd.Parameters.AddWithValue("board", student.board);
                    cmd.Parameters.AddWithValue("interrogation", student.interrogation);
                    cmd.Parameters.AddWithValue("mask", student.mask);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
            }
            columnList = ReadColumnName("students");
            if (!columnList.Contains("mask"))
            {
                cmd.CommandText = "ALTER TABLE students ADD mask INTEGER";
                cmd.ExecuteNonQuery();
            }

            columnList = ReadColumnName("votes");
            if (!columnList.Contains("active"))
            {
                cmd.CommandText = "ALTER TABLE votes ADD active BOOLEAN";
                cmd.ExecuteNonQuery();
            }
                                
        }

        private List<string> ReadColumnName(string table)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = $"PRAGMA table_info({table})";
            using SQLiteDataReader rdr = cmd.ExecuteReader();
            List<string> columnList = new List<string>();
            while (rdr.Read())
            {
                columnList.Add(rdr.GetString(1));
            }
            return columnList;
        }

        private void Main_Load(object sender, RoutedEventArgs e)
        {
            //_addFrame.Navigate(new AddVote());
            _mainFrame.Navigate(new MainMenuPage());
            //_mainFrame.Navigate(new PlanViewPage(2));
            //_mainFrame.Navigate(new PlanEditPage(1, 1));
            ClassroomsPage _ = new ClassroomsPage();
        }
            

        private void Students_Button_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new studentsPage());
            _addFrame.Navigate(new AddStudent());
        }

        private void Main_Button_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new MainMenuPage());
        }

        private void Classrooms_Button_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new ClassroomsPage());
            _addFrame.Navigate(new AddClassroom());
        }

    }
}
