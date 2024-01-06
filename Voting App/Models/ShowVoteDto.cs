namespace Voting_App.Models
{
    public class ShowVoteDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public List<ShowAnswerDto> Answers { get; set; }
    }
}
