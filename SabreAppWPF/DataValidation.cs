using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Schema;

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
        static public string[] Punishment(string surname, string lastname, DateTime? endDate)
        {
            int studentId = 0;
            int currentTimeStamp = Convert.ToInt32(new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds());
            if (surname == null) return new string[] { "Le champ prénom est obligatoire" };
            if (lastname == null) { return new string[] { "Le champ nom est obligatoire" }; }
            else
            {
                studentId = Database.Get.Student.IdFromName(lastname, surname);
                if (studentId == 0) return new string[] { "Etudiant(e) introuvable" };
            }
            if (endDate == null) { return new string[] { "Le champ Echéance est obligatoire" }; }
            else
            {
                int endDateTimestamp = Convert.ToInt32(new DateTimeOffset((DateTime)endDate).ToUnixTimeSeconds());
                if (endDateTimestamp < currentTimeStamp) return new string[] { "L'échéance ne peut pas être déjà passé" };
            }
            return new string[] { "valid", studentId.ToString()};
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

        static public string[] Note(string lastname, string surname, string content)
        {
            int studentId = 0;
            if (lastname == null) return new string[] { "Le champ nom est obligatoire" };
            if (surname == null) { return new string[] { "Le champ prénom est obligatoire" }; }
            else
            {
                studentId = Database.Get.Student.IdFromName(lastname, surname);
                if (studentId == 0) return new string[] { "Etudiant(e) introuvable" };
            }
            if (content == null) return new string[] { "Le champ contenu est obligatoire" };
            return new string[] { "valid", studentId.ToString() };
        }

        static public string Vote(string lastname, string surname, string content)
        {
            int studentId = 0;
            if (lastname == null) return "Le champ nom est obligatoire";
            if (surname == null) { return "Le champ prénom est obligatoire"; }
            else
            {
                studentId = Database.Get.Student.IdFromName(lastname, surname);
                if (studentId == 0) return "Etudiant(e) introuvable";
            }
            if (content == null) return "Le champ déscription est obligatoire";
            return "valid";
        }

        static public string Reminder(string description, DateTime? date, TimeSpan? time)
        {
            if (description == null) return "Le champ déscription est obligatoire";
            if (date == null) return "Le champ date est obligatoire";
            if (time == null) return "Le champ heure est obligatoire";
            return "valid";
        }

        //string title = _titleTextBox.Text;
        //string lastname = _lastnameTextBox.Text;
        //string surname = _lastnameTextBox.Text;
        //string grade = _gradeTextBox.Text;
        //string coeff = _coeffTextBox.Text;

        static public string Grade(string title, string lastname, string surname, string grade, string coeff)
        {
            if (title == null) return "Le titre est obligatoire";
            if (lastname == null) return "Le nom de famille est obligatoire";
            if (surname == null) return "Le prénom est obligatoire";
            int studentId = Database.Get.Student.IdFromName(lastname, surname);
            if (studentId == 0) return "L'étudiant(e) est introuvable";
            NumberStyles style = NumberStyles.AllowDecimalPoint;
            if (!Double.TryParse(grade, style, GlobalVariable.culture, out _)) return "La note est obligatoire et doit être un nombre";
            if (!int.TryParse(coeff, out _)) return "Le coeff est obligatoire et doit être un nombre";
            return "valid";
        }
    }
}
