using System.ComponentModel.DataAnnotations;

namespace Voting_App.Entities
{
    public class Answer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int VoteCounter { get; set; }

        public int VoteId { get; set; }
        public virtual Vote Vote { get; set; }
    }
}
