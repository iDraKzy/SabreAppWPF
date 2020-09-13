using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace SabreAppWPF.Database.Get
{
    public static class Punishment
    {
        public static List<PunishmentInfo> All(SQLiteCommand cmd)
        {
            List<PunishmentInfo> punishments = new List<PunishmentInfo>();

            cmd.CommandText = "SELECT * FROM punishments";
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                PunishmentInfo punishment = new PunishmentInfo()
                {
                    PunishmentId = (int)rdr.GetInt64(0),
                    StudentId = rdr.GetInt32(1),
                    CreationDate = rdr.GetInt32(2),
                    EndDate = rdr.GetInt32(3),
                    RetrieveDate = rdr.GetInt32(4),
                    Description = rdr.GetString(5),
                    Active = rdr.GetBoolean(6)
                };
                punishments.Add(punishment);
            }
            return punishments;
        }
    }
}
