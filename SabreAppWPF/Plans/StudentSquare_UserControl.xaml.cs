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

namespace SabreAppWPF.Plans
{
    /// <summary>
    /// Logique d'interaction pour StudentSquare_UserControl.xaml
    /// </summary>
    public partial class StudentSquare_UserControl : UserControl
    {
        public StudentSquare_UserControl()
        {
            InitializeComponent();

            this.DataContext = this;
        }
    }
}
