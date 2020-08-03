namespace SabreAppWPF
{
    public class StudentInfo
    {
        public int studentId, classroomId, board;
        public string name;
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
}
