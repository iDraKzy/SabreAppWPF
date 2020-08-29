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
        public int? scheduleId, classroomId, roomId, weekDay, hour, minute, nextDate, repetitivity;
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
}
