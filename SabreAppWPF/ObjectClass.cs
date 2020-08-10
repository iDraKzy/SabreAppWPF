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
        public int voteId, studentId, creationData;
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
}
