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
        public PlanViewPage(int planId)
        {
            PlanInfo plan = Database.Get.Plan.FromId(planId);
            RoomInfo room = Database.Get.Room.FromID(plan.roomId);

            string[] separationStringArray = plan.spacing.Split(",");
            int[] seperationArray = new int[separationStringArray.Length];
            for (int i = 0; i < separationStringArray.Length; i++)
            {
                seperationArray[i] = int.Parse(separationStringArray[i]);
            }

            int initItemHeight = 40;
            int initItemWidth = 40;

            List<PlaceInfo> placeList = Database.Get.Place.AllFromPlanId(planId);

            //placeList.Find(x => x.Row == 1 && x.Column == 2)

            rowNumber = room.Rows;
            columnNumber = room.Columns;

            int currentColumn = 0;

            for (int i = 0; i < rowNumber; i++)
            {
                studentPlanViewList.Add(new ObservableCollection<StudentPlanViewDisplay>());

                for (int j = 0; j < columnNumber + seperationArray.Length; j++)
                {
                    if (seperationArray.Contains(i))
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
                            Thickness = 0
                        };
                        studentPlanViewList[i].Add(placeDisplay);
                    }
                    else
                    {
                        PlaceInfo place = placeList.Find(x => x.Row == i && x.Column == currentColumn);
                        string[] name = Database.Get.Student.NameFromID(place.StudentId);
                        string parsedName = name[1] + " " + name[0];
                        StudentPlanViewDisplay placeDisplay = new StudentPlanViewDisplay()
                        {
                            ItemHeight = initItemHeight,
                            ItemWidth = initItemWidth,
                            AbsoluteWidth = 0,
                            Name = parsedName,
                            Thickness = 1,
                            StudentId = 0,
                            BoardCheck = GlobalVariable.specialCharacter["CheckMark"],
                            InterrogationCheck = GlobalVariable.specialCharacter["CheckMark"],
                            ButtonVisible = true
                        };
                        studentPlanViewList[i].Add(placeDisplay);
                        currentColumn++;
                    }
                }
                InitializeComponent();
            }
        }

        private void PlanViewPage_Load(object sender,  RoutedEventArgs e)
        {
            HandleResize();
        }

        private void PlanViewPage_Resize(object sender, RoutedEventArgs e)
        {
            HandleResize();
        }

        private void HandleResize()
        {

            //int rowCount = _testGrid.RowDefinitions.Count;
            //int columnCount = _testGrid.ColumnDefinitions.Count;
            int columnCount = 0;
            int columnSkip = 0;
            for (int i = 0; i < columnNumber; i++)
            {
                if (studentPlanViewList[0][i].AbsoluteWidth == 0)
                {
                    columnCount++;
                }
                else columnSkip++;
            }

            double columnWidth = _mainGrid.ColumnDefinitions[1].ActualWidth;
            double columnCalculation = ((columnWidth - (columnSkipSize * columnSkip)) / columnCount);

            for (int i = 0; i < studentPlanViewList.Count; i++)
            {
                for (int j = 0; j < studentPlanViewList[i].Count; j++)
                {
                    //double columnCalculation = columnWidth / columnCount;
                    double itemWidth = studentPlanViewList[i][j].AbsoluteWidth == 0 ? columnCalculation : studentPlanViewList[i][j].AbsoluteWidth;
                    double itemHeight = _mainGrid.RowDefinitions[1].ActualHeight / rowNumber;

                    studentPlanViewList[i][j].ItemHeight = (int)itemHeight;
                    studentPlanViewList[i][j].ItemWidth = (int)itemWidth;

                    //_testGrid.RowDefinitions[i].Height = new GridLength(itemHeight);
                    //_testGrid.ColumnDefinitions[j].Width = new GridLength(itemWidth);

                }
            }
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

            protected void OnPropertyChanged([CallerMemberName] string name = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
