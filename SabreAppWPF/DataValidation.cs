using System;
using System.Collections.Generic;
using System.Text;

namespace SabreAppWPF
{
    static public class DataValidation
    {
        static public string Student(string surname, string lastname)
        {
            if (surname == null) return "Le champ prénom est obligatoire";
            if (lastname == null) return "Le champ nom est obligatoire";
            return "valid";
        }
        static public string Punishment(string surname, string lastname, DateTime? endDate)
        {
            int currentTimeStamp = Convert.ToInt32(new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds());
            if (surname == null) return "Le champ prénom est obligatoire";
            if (lastname == null) return "Le champ nom est obligatoire";
            if (endDate == null) { return "Le champ Echéance est obligatoire"; }
            else
            {
                int endDateTimestamp = Convert.ToInt32(new DateTimeOffset((DateTime)endDate).ToUnixTimeSeconds());
                if (endDateTimestamp < currentTimeStamp) return "L'échéance ne peut pas être déjà passé";
            }
            return "valid";
        }

        static public string Room(string name, string rows, string columns)
        {
            if (name == null) return "Le champ nom est obligatoire";

            int rowsNumber = int.Parse(rows);
            if (rowsNumber <= 0) return "Le champ ligne ne peut pas être 0, vide ou négatif";
            int columnsNumber = int.Parse(columns);
            if (columnsNumber <= 0) return "Le champ colonnes ne peut pas être 0, vide ou négatif";

            return "valid";
        }
    }
}
