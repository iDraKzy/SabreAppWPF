using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Data.SQLite;
using System.Linq;
using SabreAppWPF.AddPages;
using Windows.UI.Xaml.Automation.Peers;

namespace SabreAppWPF.Plans
{
    /// <summary>
    /// Logique d'interaction pour PlanViewPage.xaml
    /// </summary>
    public partial class PlanViewPage : Page
    {
        private ObservableCollection<ObservableCollection<StudentPlanViewDisplay>> studentPlanViewList = new ObservableCollection<ObservableCollection<StudentPlanViewDisplay>>();
        private int columnSkipSize = 40;
        private int columnNumber;
        private int rowNumber;
        private int columnSkip;
        private int columnCount;
        private int classroomId;
        private MainWindow window = GlobalFunction.GetMainWindow();
        public PlanViewPage(int planId)
        {
            PlanInfo plan = Database.Get.Plan.FromId(planId);
            RoomInfo room = Database.Get.Room.FromID(plan.roomId);
            ScheduleInfo schedule = Database.Get.Schedule.FromId(plan.scheduleId);
            classroomId = (int)schedule.classroomId;

            List<int> seperationList = new List<int>();
            List<string> separationStringList = new List<string>();
            if (plan.spacing != "")
            {
                separationStringList = plan.spacing.Split(",").ToList();
                for (int i = 0; i < separationStringList.Count; i++)
                {
                    seperationList.Add(int.Parse(separationStringList[i]));
                }
            }
            int initItemHeight = 40;
            int initItemWidth = 40;

            List<PlaceInfo> placeList = Database.Get.Place.AllFromPlanId(planId);

            //placeList.Find(x => x.Row == 1 && x.Column == 2)

            rowNumber = room.Rows;
            columnNumber = room.Columns + seperationList.Count;
            columnCount = room.Columns;
            columnSkip = seperationList.Count;

            int currentColumn = 0;

            for (int i = 0; i < rowNumber; i++)
            {
                studentPlanViewList.Add(new ObservableCollection<StudentPlanViewDisplay>());
                currentColumn = 0;

                for (int j = 0; j < columnNumber; j++)
                {
                    if (seperationList.Contains(j))
                    {
                        StudentPlanViewDisplay placeDisplay = new StudentPlanViewDisplay()
                        {
                            ItemHeight = initItemHeight,
                            ItemWidth = initItemWidth,
                            AbsoluteWidth = columnSkipSize,
                            Name = "",
                            BoardCheck = "",
                            InterrogationCheck = "",
                            ButtonVisible = false,
                            Thickness = 0,
                            Enabled = false
                        };
                        studentPlanViewList[i].Add(placeDisplay);
                    }
                    else
                    {
                        PlaceInfo place = placeList.Find(x => x.Row == i && x.Column == currentColumn);
                        if (place == null)
                        {
                            StudentPlanViewDisplay placeDisplayNull = new StudentPlanViewDisplay()
                            {
                                ItemHeight = initItemHeight,
                                ItemWidth = initItemHeight,
                                AbsoluteWidth = 0,
                                Name = "",
                                Thickness = 0,
                                ButtonVisible = false,
                                BoardCheck = "",
                                InterrogationCheck = "",
                                Enabled = false
                            };
                            studentPlanViewList[i].Add(placeDisplayNull);
                        }
                        else
                        {
                            StudentInfo student = Database.Get.Student.FromId(place.StudentId);
                            string[] name = Database.Get.Student.NameFromID(place.StudentId);
                            string parsedName = name[1] + " " + name[0];

                            StudentPlanViewDisplay placeDisplay = new StudentPlanViewDisplay()
                            {
                                ItemHeight = initItemHeight,
                                ItemWidth = initItemWidth,
                                AbsoluteWidth = 0,
                                Name = parsedName,
                                Thickness = 1,
                                StudentId = place.StudentId,
                                BoardCheck = student.board ? GlobalVariable.specialCharacter["CheckMark"] : GlobalVariable.specialCharacter["Cross"],
                                InterrogationCheck = student.interrogation ? GlobalVariable.specialCharacter["CheckMark"] : GlobalVariable.specialCharacter["Cross"],
                                InterroEnabled = !student.interrogation,
                                BoardEnabled = !student.board,
                                ButtonVisible = false,
                                Enabled = true
                            };
                            studentPlanViewList[i].Add(placeDisplay);
                            currentColumn++;
                        }
                    }
                }
            }
            InitializeComponent();

            _lst.ItemsSource = studentPlanViewList;
        }

        private void PlanViewPage_Load(object sender,  RoutedEventArgs e)
        {
            HandleResize();
        }

        private void PlanViewPage_Resize(object sender, RoutedEventArgs e)
        {
            HandleResize();
        }

        private void Border_MouseLeave(object sender, RoutedEventArgs e)
        {
            StudentPlanViewDisplay student = (StudentPlanViewDisplay)((FrameworkElement)sender).DataContext;
            if (!student.Enabled) return;
            student.ButtonVisible = false;
        }

        private void Border_MouseEnter(object sender, RoutedEventArgs e)
        {
            StudentPlanViewDisplay student = (StudentPlanViewDisplay)((FrameworkElement)sender).DataContext;
            if (!student.Enabled) return;
            student.ButtonVisible = true;
        }

        private void AddHomework_Click(object sender, RoutedEventArgs e)
        {

            window._addFrame.Navigate(new AddHomeworkClassroom(classroomId));
        }

        private void Interrogation_Click(object sender, RoutedEventArgs e)
        {
            StudentPlanViewDisplay student = (StudentPlanViewDisplay)((FrameworkElement)sender).DataContext;
            student.InterroEnabled = false;
            student.InterrogationCheck = GlobalVariable.specialCharacter["CheckMark"];

            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "UPDATE students SET interrogation = true WHERE studentId = @studentId";
            cmd.Parameters.AddWithValue("studentId", student.StudentId);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        private void Board_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RandomBoard_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RandomInterro_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleResize()
        {

            //int rowCount = _testGrid.RowDefinitions.Count;
            //int columnCount = _testGrid.ColumnDefinitions.Count;

            double columnWidth = _testGrid.ActualWidth;
            double columnCalculation = ((columnWidth - (columnSkipSize * columnSkip)) / columnCount);

            for (int i = 0; i < studentPlanViewList.Count; i++)
            {
                for (int j = 0; j < studentPlanViewList[i].Count; j++)
                {
                    //double columnCalculation = columnWidth / columnCount;
                    double itemWidth = studentPlanViewList[i][j].AbsoluteWidth == 0 ? columnCalculation : studentPlanViewList[i][j].AbsoluteWidth;
                    double itemHeight = _testGrid.ActualHeight / rowNumber;

                    studentPlanViewList[i][j].ItemHeight = (int)itemHeight;
                    studentPlanViewList[i][j].ItemWidth = (int)itemWidth;

                    //_testGrid.RowDefinitions[i].Height = new GridLength(itemHeight);
                    //_testGrid.ColumnDefinitions[j].Width = new GridLength(itemWidth);

                }
            }
        }

        private void UpvoteButton_Click(object sender, RoutedEventArgs e)
        {
            StudentPlanViewDisplay student = (StudentPlanViewDisplay)((FrameworkElement)sender).DataContext;
            window._addFrame.Navigate(new AddVote(true, student.StudentId));
        }

        private void DownvoteButton_Click(object sender, RoutedEventArgs e)
        {
            StudentPlanViewDisplay student = (StudentPlanViewDisplay)((FrameworkElement)sender).DataContext;
            window._addFrame.Navigate(new AddVote(false, student.StudentId));
        }

        public class StudentPlanViewDisplay : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            private int _studentId;
            private int _thickness;
            private int _absoluteWidth;
            private int _itemHeight;
            private int _itemWidth;
            private string _name;
            private string _interrogationCheck;
            private string _boardCheck;
            private bool _buttonVisible;
            private bool _enabled;
            private bool _interroEnabled;
            private bool _boardEnabled;

            public int StudentId
            {
                get { return _studentId; }
                set
                {
                    _studentId = value;
                    OnPropertyChanged();
                }
            }

            public int Thickness
            {
                get { return _thickness; }
                set
                {
                    _thickness = value;
                    OnPropertyChanged();
                }
            }

            public int AbsoluteWidth
            {
                get { return _absoluteWidth; }
                set
                {
                    _absoluteWidth = value;
                    OnPropertyChanged();
                }
            }

            public int ItemHeight
            {
                get { return _itemHeight; }
                set
                {
                    _itemHeight = value;
                    OnPropertyChanged();
                }
            }

            public int ItemWidth
            {
                get { return _itemWidth; }
                set
                {
                    _itemWidth = value;
                    OnPropertyChanged();
                }
            }

            public string Name
            {
                get { return _name; }
                set
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }

            public string InterrogationCheck
            {
                get { return _interrogationCheck; }
                set
                {
                    _interrogationCheck = value;
                    OnPropertyChanged();
                }
            }

            public string BoardCheck
            {
                get { return _boardCheck; }
                set
                {
                    _boardCheck = value;
                    OnPropertyChanged();
                }
            }

            public bool ButtonVisible
            {
                get { return _buttonVisible; }
                set
                {
                    _buttonVisible = value;
                    OnPropertyChanged();
                }
            }

            public bool Enabled
            {
                get { return _enabled; }
                set
                {
                    _enabled = value;
                    OnPropertyChanged();
                }
            }

            public bool InterroEnabled
            {
                get { return _interroEnabled; }
                set
                {
                    _interroEnabled = value;
                    OnPropertyChanged();
                }
            }

            public bool BoardEnabled
            {
                get { return _boardEnabled; }
                set
                {
                    _boardEnabled = value;
                    OnPropertyChanged();
                }
            }

            protected void OnPropertyChanged([CallerMemberName] string name = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
