using System.Net;

namespace Voting_App.Entities
{
    public class Vote
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int CreatedBy { get; set; }

        public int AnswerId { get; set; }
        public virtual List<Answer> Answers { get; set; }

       
    }
}
