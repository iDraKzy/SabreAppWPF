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
using Windows.UI.Xaml.Automation.Peers;

namespace SabreAppWPF
{
    /// <summary>
    /// Logique d'interaction pour Test.xaml
    /// </summary>
    public partial class Test : Page
    {
        private ObservableCollection<ObservableCollection<PlaceDisplay>> studentPlanList;
        private int rowNumber = 3;
        private int columnNumber = 6;
        private int columnSkipSize = 40;
        public Test()
        {
            studentPlanList = new ObservableCollection<ObservableCollection<PlaceDisplay>>();

            //double itemWidth = _mainGrid.ColumnDefinitions[1].ActualWidth / columnNumber;
            //double itemHeight = _mainGrid.RowDefinitions[0].ActualHeight / rowNumber;
            double initItemHeight = 60;
            double initItemWidth = 60;

            for (int i = 0; i < rowNumber; i++)
            {
                studentPlanList.Add(new ObservableCollection<PlaceDisplay>());

                for (int j = 0; j < columnNumber; j++)
                {
                    if (j == 1)
                    {
                        PlaceDisplay placeDisplay = new PlaceDisplay()
                        {
                            ItemHeight = (int)initItemHeight,
                            ItemWidth = (int)initItemWidth,
                            AbsoluteWidth = columnSkipSize,
                            Content = "",
                            Drop = false,
                            Thickness = 0
                        };
                        studentPlanList[i].Add(placeDisplay);
                    }
                    else if (j % 2 == 0)
                    {
                        PlaceDisplay placeDisplay = new PlaceDisplay()
                        {
                            ItemHeight = (int)initItemHeight,
                            ItemWidth = (int)initItemWidth,
                            AbsoluteWidth = 0,
                            Content = "Youlan",
                            Drop = true,
                            Thickness = 1
                        };
                        studentPlanList[i].Add(placeDisplay);
                    } 
                    else
                    {
                        PlaceDisplay placeDisplay = new PlaceDisplay()
                        {
                            ItemHeight = (int)initItemHeight,
                            ItemWidth = (int)initItemWidth,
                            AbsoluteWidth = 0,
                            Content = "Emeric",
                            Drop = true,
                            Thickness = 1
                        };
                        studentPlanList[i].Add(placeDisplay);
                    }
                }
            }


            InitializeComponent();

            lst.ItemsSource = studentPlanList;
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

            for (int i = 0; i < studentPlanList.Count; i++)
            {
                for (int j = 0; j < studentPlanList[i].Count; j++)
                {
                    double columnWidth = _mainGrid.ColumnDefinitions[1].ActualWidth;
                    //double columnCalculation = columnWidth / columnCount;
                    double columnCalculation = ((columnWidth - (columnSkipSize * columnSkip)) / columnCount);
                    double itemWidth = studentPlanList[i][j].AbsoluteWidth == 0 ? columnCalculation : studentPlanList[i][j].AbsoluteWidth;
                    double itemHeight = _mainGrid.RowDefinitions[0].ActualHeight / rowNumber;

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

        public class PlaceDisplay : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            private int _itemHeight;
            private int _itemWidth;
            private int _thickness;
            private string _content;
            private bool _drop;
            private int _absoluteWidth;

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

            protected void OnPropertyChanged([CallerMemberName] string name = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
