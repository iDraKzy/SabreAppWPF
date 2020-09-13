using Windows.Phone.Management.Deployment;

namespace SabreAppWPF
{
    public class StudentInfo
    {
        public int studentId, mask;
        public string lastname, surname;
        public bool gender, interrogation, board;
    }

    public class VotesInfo
    {
        public int voteId, studentId, creationDate;
        public bool upvotes, active;
        public string description;
    }

    public class NoteInfo
    {
        public int noteId, studentId, creationDate;
        public string content;
        public bool active;
    }

    public class HomeworkInfo
    {
        public int homeworkId, studentId, creationDate, endDate, retrieveDate;
        public string description;
        public bool active;
    }

    public class ScheduleInfo
    {
        public int? scheduleId, classroomId, roomId, nextDate, repetitivity, duration;
    }

    public class PlanInfo
    {
        public int planId, scheduleId, roomId;
        public string spacing, name;
    }

    public class GradeInfo
    {
        public int GradeId { get; set; }
        public int StudentId { get; set; }
        public int Coeff { get; set; }
        public int CreationDate { get; set; }
        public float Grade { get; set; }
    }

    public class ClassroomInfo
    {
        public int ClassroomId { get; set; }
        public string Name { get; set; }
    }

    public class RoomInfo
    {
        public int RoomId { get; set; }
        public string Name { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
    }
    public class PlaceInfo
    {
        public int PlaceId { get; set; }
        public int PlanId { get; set; }
        public int StudentId { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }
    public class ReminderInfo
    {
        public int ReminderId { get; set; }
        public int CreationDate { get; set; }
        public int ReminderDate { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }

    public class LinkStudentClassroomInfo
    {
        public int StudentId { get; set; }
        public int ClassroomId { get; set; }
    }
//    punishments(punishmentId INTEGER PRIMARY KEY, studentId INTEGER,
//creationDate INTEGER, endDate INTEGER, retrieveDate INTEGER, description TEXT, active BOOLEAN);
    public class PunishmentInfo
    {
        public int PunishmentId { get; set; }
        public int StudentId { get; set; }
        public int CreationDate { get; set; }
        public int EndDate { get; set; }
        public int RetrieveDate { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }
}
