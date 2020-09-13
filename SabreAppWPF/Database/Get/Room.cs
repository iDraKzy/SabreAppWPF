using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace SabreAppWPF.Database.Get
{
    public static class Room
    {
        /// <summary>
        /// Returns a RoomInfo object from the id of the room
        /// </summary>
        /// <param name="roomId">Id of the room</param>
        /// <returns>RoomInfo object</returns>
        public static RoomInfo FromID(int roomId)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "SELECT EXISTS(SELECT * FROM rooms WHERE roomId = @roomId)";
            cmd.Parameters.AddWithValue("roomId", roomId);
            cmd.Prepare();
            int exist = (int)(long)cmd.ExecuteScalar();
            if (exist == 0) return null;
            cmd.CommandText = "SELECT * FROM rooms WHERE roomId = @roomId";
            cmd.Parameters.AddWithValue("roomId", roomId);
            cmd.Prepare();
            using SQLiteDataReader rdr = cmd.ExecuteReader();
            RoomInfo roomInfo = new RoomInfo();
            while (rdr.Read())
            {
                roomInfo.RoomId = rdr.GetInt32(0);
                roomInfo.Name = rdr.GetString(1);
                roomInfo.Rows = rdr.GetInt32(2);
                roomInfo.Columns = rdr.GetInt32(3);
            }
            return roomInfo;
        }

        public static string NameFromID(int roomId)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = "SELECT name FROM rooms WHERE roomId = @roomId";
            cmd.Parameters.AddWithValue("roomId", roomId);
            cmd.Prepare();
            string roomName = (string)cmd.ExecuteScalar();
            return roomName;
        }

        /// <summary>
        /// Returns all rooms from the database
        /// </summary>
        /// <returns>List of RoomInfo</returns>
        public static List<RoomInfo> All(string type)
        {
            string source = (type == "old") ? Update.oldPath : GlobalVariable.path;
            using SQLiteConnection connection = new SQLiteConnection("Data Source=" + source);
            connection.Open();
            using SQLiteCommand cmd = new SQLiteCommand(connection);
            List<RoomInfo> roomsList = new List<RoomInfo>();
            cmd.CommandText = "SELECT * FROM rooms";
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                RoomInfo roomInfo = new RoomInfo()
                {
                    RoomId = rdr.GetInt32(0),
                    Name = rdr.GetString(1),
                    Rows = rdr.GetInt32(2),
                    Columns = rdr.GetInt32(3)
                };
                roomsList.Add(roomInfo);
            }
            return roomsList;
        }
    }
}
