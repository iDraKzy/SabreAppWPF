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

namespace SabreAppWPF.Classrooms
{
    /// <summary>
    /// Logique d'interaction pour ClassroomsOptions.xaml
    /// </summary>
    public partial class ClassroomsOptions : Page
    {
        private int classroomId;
        public ClassroomsOptions(int classroomId)
        {
            InitializeComponent();
            this.classroomId = classroomId;
        }
    }
}
