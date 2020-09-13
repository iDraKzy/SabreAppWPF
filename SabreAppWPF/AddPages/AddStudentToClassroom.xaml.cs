using System;
using System.Collections.Generic;
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

namespace SabreAppWPF.AddPages
{
    /// <summary>
    /// Logique d'interaction pour AddStudentToClassroom.xaml
    /// </summary>
    public partial class AddStudentToClassroom : Page
    {
        public AddStudentToClassroom(int? classroomId = null)
        {
            InitializeComponent();

            List<Entry> studentEntries = new List<Entry>();
            List<Entry> classroomEntries = new List<Entry>();

            List<StudentInfo> studentList = Database.Get.Student.All("new");
            List<ClassroomInfo> classroomList = Database.Get.Classroom.All("new");

            foreach (StudentInfo student in studentList)
            {
                Entry studentEntry = new Entry()
                {
                    ID = student.studentId,
                    Name = student.surname + " " + student.lastname
                };
                studentEntries.Add(studentEntry);
            }

            foreach (ClassroomInfo classroom in classroomList)
            {
                Entry classroomEntry = new Entry()
                {
                    ID = classroom.ClassroomId,
                    Name = classroom.Name
                };
                classroomEntries.Add(classroomEntry);
            }

            _studentComboBox.ItemsSource = studentEntries;
            _studentComboBox.SelectedIndex = 0;
            _classroomComboBox.ItemsSource = classroomEntries;
            if (classroomId != null)
            {
                _classroomComboBox.SelectedValue = classroomId;
            }
            else _classroomComboBox.SelectedIndex = 0;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            int studentId = (int)_studentComboBox.SelectedValue;
            int classroomId = (int)_classroomComboBox.SelectedValue;

            int linkId = Database.Insert.LinkStudentToClassroom.One(studentId, classroomId);
            if (linkId != 0)
            {
                error.Foreground = new SolidColorBrush(Colors.Green);
                error.Content = "Etudiant ajouté dans la classe avec succès";
            }
            else
            {
                error.Foreground = new SolidColorBrush(Colors.Red);
                error.Content = "Une erreur innatendue est survenue";
            }
        }

        public class Entry : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            private string _name;
            private int _id;

            public int ID
            {
                get { return _id; }
                set
                {
                    _id = value;
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

            protected void OnPropertyChanged([CallerMemberName] string name = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
