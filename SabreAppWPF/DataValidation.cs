using System;
using System.Collections.Generic;
using System.Text;

namespace SabreAppWPF
{
    static public class DataValidation
    {
        static public string Punishment(string surname, string lastname, DateTime? endDate)
        {
            int currentTimeStamp = Convert.ToInt32(new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds());
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
    }
}
