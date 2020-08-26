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
using SabreAppWPF.Students.StudentDetails;

namespace SabreAppWPF
{
    /// <summary>
    /// Logique d'interaction pour AddTemplate.xaml
    /// </summary>
    public partial class AddTemplate : Page
    {
        private string type;
        private int studentId;

        //TODO: Refactor this shit
        public AddTemplate(string type, int studentId = 0)
        {
            InitializeComponent();
            this.type = type;
            this.studentId = studentId;
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
                case "classroom":
                    //InitializeClassroomCreation();
                    break;
                case "room":
                    //InitializeRoomCreation();
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
            }
        }
    }

}
