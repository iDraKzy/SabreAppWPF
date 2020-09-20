using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using Windows.Devices.AllJoyn;
using System.Threading.Tasks;

namespace SabreAppWPF.Database.Get
{
    public static class LinkStudentToClassroom
    {
        public static List<LinkStudentClassroomInfo> All(string type)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "SELECT * FROM linkStudentToClassroom";
            using SQLiteDataReader rdr = cmd.ExecuteReader();
            List<LinkStudentClassroomInfo> links = new List<LinkStudentClassroomInfo>();

            while (rdr.Read())
            {
                LinkStudentClassroomInfo link = new LinkStudentClassroomInfo()
                {
                    StudentId = rdr.GetInt32(1),
                    ClassroomId = rdr.GetInt32(2)
                };
                links.Add(link);
            }
            return links;
        }
    }
}
