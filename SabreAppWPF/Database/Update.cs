using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;
using Windows.Storage;

namespace SabreAppWPF.Database
{
    public static class Update
    {
        public static string oldPath = System.IO.Path.Combine(ApplicationData.Current.LocalFolder.Path, "Sabre.db");
        public static void UpdateDb()
        {
            //Only for when it's Sabre.db
            //TODO: Refactor next version

            //Connection to old database
            using SQLiteConnection connection = new SQLiteConnection("Data Source=" + oldPath);
            using SQLiteCommand cmd = new SQLiteCommand(connection);

            UpdateLinkStudentClassroom(Get.LinkStudentToClassroom.All(cmd));
            UpdateStudent(Get.Student.All(cmd));
            UpdateHomeworks(Get.Homework.All(cmd));
            UpdatePunishments(Get.Punishment.All(cmd));
            UpdateNotes(Get.Note.All(cmd));
            UpdateGrades(Get.Grade.All(cmd));
            UpdateVotes(Get.Vote.All(cmd));
            UpdateRooms(Get.Room.All(cmd));
            UpdateClassrooms(Get.Classroom.All(cmd));
            UpdateSchedules(Get.Schedule.All(cmd));
            UpdatePlans(Get.Plan.All(cmd));
            UpdatePlaces(Get.Place.All(cmd));
            UpdateReminder(Get.Reminder.All(cmd));
            File.Delete(oldPath);
        }

        private static void UpdateLinkStudentClassroom(List<LinkStudentClassroomInfo> oldLinks)
        {
            foreach (LinkStudentClassroomInfo link in oldLinks)
            {
                Insert.LinkStudentToClassroom.One(link.StudentId, link.ClassroomId);
            }
        }

        private static void UpdateStudent(List<StudentInfo> oldStudents)
        {
            foreach (StudentInfo student in oldStudents)
            {
                Insert.Student.One(student.lastname, student.surname, student.gender, student.board, student.interrogation, student.mask, student.studentId);
            }
        }

        private static void UpdateHomeworks(List<HomeworkInfo> oldHomeworks)
        {
            foreach (HomeworkInfo homework in oldHomeworks)
            {
                Insert.Homework.One(homework.studentId, homework.creationDate, homework.endDate, homework.retrieveDate, homework.description, homework.homeworkId);
            }
        }

        private static void UpdatePunishments(List<PunishmentInfo> oldPunishments)
        {
            foreach (PunishmentInfo punishment in oldPunishments)
            {
                Insert.Punishment.One(punishment.StudentId, punishment.CreationDate, punishment.EndDate, punishment.RetrieveDate, punishment.Description, punishment.Active, punishment.PunishmentId);
            }
        }

        private static void UpdateNotes(List<NoteInfo> oldNotes)
        {
            foreach (NoteInfo note in oldNotes)
            {
                Insert.Note.One(note.studentId, note.content, note.creationDate, note.active, note.noteId);
            }
        }

        private static void UpdateGrades(List<GradeInfo> oldGrades)
        {
            foreach (GradeInfo grade in oldGrades)
            {
                Insert.Grade.One(grade.StudentId, grade.Grade, grade.Coeff, grade.CreationDate, grade.GradeId);
            }
        }

        private static void UpdateVotes(List<VotesInfo> oldVotes)
        {
            foreach (VotesInfo vote in oldVotes)
            {
                Insert.Vote.One(vote.studentId, vote.upvotes, vote.description, vote.creationDate, vote.active, vote.voteId);
            }
        }

        private static void UpdateRooms(List<RoomInfo> oldRooms)
        {
            foreach (RoomInfo room in oldRooms)
            {
                Insert.Room.One(room.Name, room.Rows, room.Columns, room.RoomId);
            }
        }

        private static void UpdateClassrooms(List<ClassroomInfo> oldClassrooms)
        {
            foreach (ClassroomInfo classroom in oldClassrooms)
            {
                Insert.Classroom.One(classroom.Name, classroom.ClassroomId);
            }
        }

        private static void UpdateSchedules(List<ScheduleInfo> oldSchedules)
        {
            foreach (ScheduleInfo schedule in oldSchedules)
            {
                Insert.Schedule.One((int)schedule.classroomId, (int)schedule.roomId, (int)schedule.repetitivity, (int)schedule.nextDate, (int)schedule.duration, (int)schedule.scheduleId);
            }
        }

        private static void UpdatePlans(List<PlanInfo> oldPlans)
        {
            foreach (PlanInfo plan in oldPlans)
            {
                Insert.Plan.One(plan.scheduleId, plan.roomId, plan.spacing, plan.name, plan.planId);
            }
        }

        private static void UpdatePlaces(List<PlaceInfo> oldPlaces)
        {
            foreach (PlaceInfo place in oldPlaces)
            {
                Insert.Place.One(place.PlanId, place.StudentId, place.Row, place.Column, place.PlaceId);
            }
        }

        private static void UpdateReminder(List<ReminderInfo> oldReminders)
        {
            foreach (ReminderInfo reminder in oldReminders)
            {
                Insert.Reminder.One(reminder.ReminderDate, reminder.Description, reminder.CreationDate, reminder.Active, reminder.ReminderId);
            }
        }
    }
}
