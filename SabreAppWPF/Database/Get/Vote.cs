using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace SabreAppWPF.Database.Get
{
    public static class Vote
    {
        public static List<VotesInfo> All(SQLiteCommand cmd)
        {
            cmd.CommandText = "SELECT * FROM votes";
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            List<VotesInfo> votes = new List<VotesInfo>();

            while (rdr.Read())
            {
                VotesInfo vote = new VotesInfo()
                {
                    voteId = rdr.GetInt32(0),
                    studentId = rdr.GetInt32(1),
                    upvotes = rdr.GetBoolean(2),
                    description = rdr.GetString(3),
                    creationDate = rdr.GetInt32(4),
                    active = rdr.GetBoolean(5)
                };
                votes.Add(vote);
            }
            return votes;
        }

        /// <summary>
        /// Get all votes of the specified student of the given type (upvote = true, downvote = false)
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="type">type of votes (upvote = true, downvote = false)</param>
        /// <returns></returns>
        public static List<VotesInfo> AllFromStudentId(int studentId, bool type)
        {
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = $"SELECT * FROM votes WHERE studentId = {studentId} AND upvotes = {type}";
            using SQLiteDataReader rdr = cmd.ExecuteReader();
            List<VotesInfo> votesList = new List<VotesInfo>();

            while (rdr.Read())
            {
                VotesInfo voteInfo = new VotesInfo()
                {
                    voteId = rdr.GetInt32(0),
                    studentId = rdr.GetInt32(1),
                    upvotes = rdr.GetBoolean(2),
                    description = rdr.GetString(3),
                    creationDate = rdr.GetInt32(4),
                    active = rdr.GetBoolean(5)
                };
                votesList.Add(voteInfo);
            }
            rdr.Close();
            return votesList;
        }
    }
}
