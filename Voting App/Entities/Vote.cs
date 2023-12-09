namespace Voting_App.Entities
{
    public class Vote
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int YesVotes { get; set; }
        public int NoVotes { get; set; }

        public virtual User CreatedBy { get; set; }
    }
}
