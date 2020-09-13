using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using Windows.Devices.AllJoyn;

namespace SabreAppWPF.Database.Get
{
    public static class LinkStudentToClassroom
    {
        public static List<LinkStudentClassroomInfo> All(SQLiteCommand cmd)
        {
            cmd.CommandText = "SELECT * FROM linkStudentToClassroom";
            using SQLiteDataReader rdr = cmd.ExecuteReader();
            List<LinkStudentClassroomInfo> links = new List<LinkStudentClassroomInfo>();

            while (rdr.Read())
            {
                LinkStudentClassroomInfo link = new LinkStudentClassroomInfo()
                {
                    StudentId = rdr.GetInt32(0),
                    ClassroomId = rdr.GetInt32(1)
                };
                links.Add(link);
            }
            return links;
        }
    }
}
