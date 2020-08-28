using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.UI.Xaml.Automation.Peers;
using System.Data.SQLite;
using System.Linq;

namespace SabreAppWPF.Students.StudentDetails
{
    /// <summary>
    /// Logique d'interaction pour VotesDetails.xaml
    /// </summary>
    public partial class VotesDetails : Page
    {
        public static ObservableCollection<VoteDetails> UpvoteList = new ObservableCollection<VoteDetails>();
        public static ObservableCollection<VoteDetails> DownvoteList = new ObservableCollection<VoteDetails>();
        public VotesDetails(int studentId)
        {
            InitializeComponent();

            List<VotesInfo> upvoteList = Getter.GetAllVotes(studentId, true);
            List<VotesInfo> downvoteList = Getter.GetAllVotes(studentId, false);

            UpvoteList = ParseVoteList(upvoteList);
            DownvoteList = ParseVoteList(downvoteList);

            _upvoteDataGrid.ItemsSource = UpvoteList;
            _downvoteDataGrid.ItemsSource = DownvoteList;
        }

        private ObservableCollection<VoteDetails> ParseVoteList(List<VotesInfo> votesList)
        {
            ObservableCollection<VoteDetails> newListVote = new ObservableCollection<VoteDetails>();
            foreach (VotesInfo vote in votesList)
            {
                int voteTimestamp = vote.creationDate;
                DateTime voteDateTime = DateTimeOffset.FromUnixTimeSeconds(voteTimestamp).LocalDateTime;
                VoteDetails voteDetails = new VoteDetails()
                {
                    ID = vote.voteId,
                    Content = vote.description,
                    Date = voteDateTime.ToString("g", GlobalVariable.culture)
                };
                newListVote.Add(voteDetails);
            }
            return newListVote;
        }

        private void DeleteUpvote_Click(object sender, RoutedEventArgs e)
        {
            VoteDetails vote = (VoteDetails)((FrameworkElement)sender).DataContext;
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = $"DELETE FROM votes WHERE voteId = @voteId";
            cmd.Parameters.AddWithValue("voteId", vote.ID);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            UpvoteList.Remove(UpvoteList.Where(i => i.ID == vote.ID).Single());
        }

        private void DeleteDownvote_Click(object sender, RoutedEventArgs e)
        {
            VoteDetails vote = (VoteDetails)((FrameworkElement)sender).DataContext;
            using SQLiteCommand cmd = GlobalFunction.OpenDbConnection();
            cmd.CommandText = $"DELETE FROM votes WHERE voteId = @voteId";
            cmd.Parameters.AddWithValue("voteId", vote.ID);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            DownvoteList.Remove(DownvoteList.Where(i => i.ID == vote.ID).Single());
        }

        public class VoteDetails : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            private int _id;
            private string _date;
            private string _content;

            public int ID
            {
                get { return _id; }
                set
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }

            public string Date
            {
                get { return _date; }
                set
                {
                    _date = value;
                    OnPropertyChanged();
                }
            }

            public string Content
            {
                get { return _content; }
                set
                {
                    _content = value;
                    OnPropertyChanged();
                }
            }

            protected void OnPropertyChanged([CallerMemberName] string name = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
