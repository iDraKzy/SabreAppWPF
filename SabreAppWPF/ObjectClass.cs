using Windows.Phone.Management.Deployment;

namespace SabreAppWPF
{
    public class StudentInfo
    {
        public int studentId, classroomId, board;
        public string lastname, surname;
        public bool gender, interrogation;
    }

    public class VotesInfo
    {
        public int voteId, studentId, creationDate;
        public bool upvotes;
        public string description;
    }

    public class NoteInfo
    {
        public int noteId, studentId, creationDate;
        public string content;
    }

    public class HomeworkInfo
    {
        public int homeworkId, studentId, creationDate, endDate, retrieveDate;
        public string description;
    }

    public class ScheduleInfo
    {
        public int? scheduleId, classroomId, roomId, nextDate, repetitivity, duration;
    }

    public class PlanInfo
    {
        public int planId, scheduleId, roomId;
        public string spacing;
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
    }
}
