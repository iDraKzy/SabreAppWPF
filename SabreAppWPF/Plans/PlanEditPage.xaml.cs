using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Pkcs;
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
using System.Linq;

namespace SabreAppWPF.Plans
{
    /// <summary>
    /// Logique d'interaction pour Test.xaml
    /// </summary>
    public partial class PlanEditPage : Page
    {
        private ObservableCollection<ObservableCollection<PlaceDisplay>> studentPlanList;
        private List<Button> buttonList;
        private int rowNumber;
        private int columnNumber;
        private int columnSkipSize = 40;
        private List<int> spacing = new List<int>();
        private int? planId = null;
        public PlanEditPage(int scheduleId, int classroomId)
        {
            studentPlanList = new ObservableCollection<ObservableCollection<PlaceDisplay>>();
            ScheduleInfo schedule = Getter.GetScheduleFromId(scheduleId);
            if (schedule.scheduleId == null)
            {
                MessageBox.Show("Un probème est surnvenu");
                return;
            }
            RoomInfo roomInfo = Getter.GetRoomFromID((int)schedule.roomId);
            if (roomInfo == null)
            {
                MessageBox.Show("Salle inexistante");
                return;
            }
            rowNumber = roomInfo.Rows;
            columnNumber = roomInfo.Columns;

            //double itemWidth = _mainGrid.ColumnDefinitions[1].ActualWidth / columnNumber;
            //double itemHeight = _mainGrid.RowDefinitions[0].ActualHeight / rowNumber;
            double initItemHeight = 60;
            double initItemWidth = 60;

            for (int i = 0; i < rowNumber; i++)
            {
                studentPlanList.Add(new ObservableCollection<PlaceDisplay>());

                for (int j = 0; j < columnNumber; j++)
                {
                    //if (j == 1)
                    //{
                    //    PlaceDisplay placeDisplay = new PlaceDisplay()
                    //    {
                    //        ItemHeight = (int)initItemHeight,
                    //        ItemWidth = (int)initItemWidth,
                    //        AbsoluteWidth = columnSkipSize,
                    //        Content = "",
                    //        Drop = false,
                    //        Thickness = 0
                    //    };
                    //    studentPlanList[i].Add(placeDisplay);
                    //} else
                    //{
                        PlaceDisplay placeDisplay = new PlaceDisplay()
                        {
                            Row = i,
                            Column = j,
                            ItemHeight = (int)initItemHeight,
                            ItemWidth = (int)initItemWidth,
                            AbsoluteWidth = 0,
                            Content = $"Glisser déposer ici pour\nplacer l'élève {i} {j}",
                            Drop = true,
                            Thickness = 1
                        };
                        studentPlanList[i].Add(placeDisplay);
                    //}
                }
            }


            InitializeComponent();
            buttonList = new List<Button>();
            List<string> spacingList = new List<string>();
            if (planId != null)
            {
                PlanInfo plan = Getter.GetPlanFromScheduleId(scheduleId);
                spacingList = plan.spacing.Split(",").ToList();
                planId = plan.planId;

                
            }
            for (int i = 0; i < columnNumber - 1; i++)
            {
                Button btn = new Button();
                btn.Content = "|";
                btn.Height = 20;
                btn.Width = 20;

                btn.HorizontalAlignment = HorizontalAlignment.Left;
                btn.Tag = i;
                btn.Click += SpacingButton_Click;
                //btn.Margin = new Thickness(80 * (i + 1), 0, 0, 0);
                _mainGrid.Children.Add(btn);
                Grid.SetRow(btn, 0);
                Grid.SetColumn(btn, 1);
                string found = spacingList.Find(x => x == i.ToString());
                if (found != null)
                {
                    btn.IsEnabled = false;
                }
                buttonList.Add(btn);
            }

            ObservableCollection<NameDisplay> studentCollection = new ObservableCollection<NameDisplay>();
            List<StudentInfo> studentList = Getter.GetAllStudentsFromClassroomId(classroomId);
            foreach (StudentInfo student in studentList)
            {
                NameDisplay nameDisplay = new NameDisplay()
                {
                    ID = student.studentId,
                    Name = String.Join(" ", new string[] { student.surname, student.lastname })
                };
                studentCollection.Add(nameDisplay);
            }

            _studentList.ItemsSource = studentCollection;
            _lst.ItemsSource = studentPlanList;
        }

        private void SpacingButton_Click(object sender, RoutedEventArgs e)
        {
            int index = (int)((Button)sender).Tag;
            spacing.Add(index);
            ((Button)sender).IsEnabled = false;
        }

        private ListBox dragSource = null;
        private void Student_Drop(object sender, DragEventArgs e)
        {
            PlaceDisplay placeDisplay = (PlaceDisplay)((FrameworkElement)sender).DataContext;
            NameDisplay data = (NameDisplay)e.Data.GetData(typeof(NameDisplay));
            placeDisplay.Content = data.Name;
            placeDisplay.StudentId = data.ID;
            ((IList)dragSource.ItemsSource).Remove(data);

        }

        private void Listbox_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            dragSource = listBox;
            object data = GetDataFromListBox(dragSource, e.GetPosition(listBox));

            if (data != null)
            {
                DragDrop.DoDragDrop(listBox, data, DragDropEffects.Move);
            }

        }

        private static object GetDataFromListBox(ListBox source, Point point)
        {
            UIElement element = source.InputHitTest(point) as UIElement;
            if (element != null)
            {
                object data = DependencyProperty.UnsetValue;
                while (data == DependencyProperty.UnsetValue)
                {
                    data = source.ItemContainerGenerator.ItemFromContainer(element);

                    if (data == DependencyProperty.UnsetValue)
                    {
                        element = VisualTreeHelper.GetParent(element) as UIElement;
                    }

                    if (element == source)
                    {
                        return null;
                    }
                }

                if (data != DependencyProperty.UnsetValue)
                {
                    return data;
                }
            }

            return null;
        }

        private void Test_Load(object sender, RoutedEventArgs e)
        {
            HandleResize();
        }

        private void Test_SizeChanged(object sender, SizeChangedEventArgs e)
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
                if (studentPlanList[0][i].AbsoluteWidth == 0)
                {
                    columnCount++;
                } 
                else columnSkip++;
            }

            double columnWidth = _mainGrid.ColumnDefinitions[1].ActualWidth;
            double columnCalculation = ((columnWidth - (columnSkipSize * columnSkip)) / columnCount);
            for (int h = 0; h < buttonList.Count; h++)
            {
                buttonList[h].Margin = new Thickness(columnCalculation * (h + 1) - 10, 0, 0, 0);
            }

            for (int i = 0; i < studentPlanList.Count; i++)
            {
                for (int j = 0; j < studentPlanList[i].Count; j++)
                {
                    //double columnCalculation = columnWidth / columnCount;
                    double itemWidth = studentPlanList[i][j].AbsoluteWidth == 0 ? columnCalculation : studentPlanList[i][j].AbsoluteWidth;
                    double itemHeight = _mainGrid.RowDefinitions[1].ActualHeight / rowNumber;

                    studentPlanList[i][j].ItemHeight = (int)itemHeight;
                    studentPlanList[i][j].ItemWidth = (int)itemWidth;

                    //_testGrid.RowDefinitions[i].Height = new GridLength(itemHeight);
                    //_testGrid.ColumnDefinitions[j].Width = new GridLength(itemWidth);

                }
            }


            //foreach (ObservableCollection<PlaceDisplay> placei in studentPlanList)
            //{
            //    double itemWidth = _mainGrid.ColumnDefinitions[1].ActualWidth / columnNumber;
            //    double itemHeight = _mainGrid.RowDefinitions[0].ActualHeight / rowNumber;
            //    foreach (PlaceDisplay placej in placei)
            //    {
            //        placej.ItemHeight = (int)itemHeight;
            //        placej.ItemWidth = (int)itemWidth;
            //    }
            //}

            //foreach (RowDefinition row in _testGrid.RowDefinitions)
            //{
            //    row.Height = new GridLength(itemHeight);
            //}

            //foreach (ColumnDefinition column in _testGrid.ColumnDefinitions)
            //{
            //    column.Width = new GridLength(itemWidth);
            //}
        }

        public class NameDisplay : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            private int _id;
            private string _name;

            public string Name
            {
                get { return _name; }
                set
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }

            public int ID
            {
                get { return _id; }
                set
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
            protected void OnPropertyChanged([CallerMemberName] string name = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        public class PlaceDisplay : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            private int _row;
            private int _column;
            private int _itemHeight;
            private int _itemWidth;
            private int _thickness;
            private int _studentId;
            private string _content;
            private bool _drop;
            private int _absoluteWidth;

            public int Row
            {
                get { return _row; }
                set
                {
                    _row = value;
                    OnPropertyChanged();
                }
            }

            public int Column
            {
                get { return _column; }
                set
                {
                    _column = value;
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

            public int AbsoluteWidth
            {
                get { return _absoluteWidth; }
                set
                {
                    _absoluteWidth = value;
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

            public string Content
            {
                get { return _content; }
                set
                {
                    _content = value;
                    OnPropertyChanged();
                }
            }

            public bool Drop
            {
                get { return _drop; }
                set
                {
                    _drop = value;
                    OnPropertyChanged();
                }
            }

            public int StudentId
            {
                get { return _studentId; }
                set
                {
                    _studentId = value;
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
