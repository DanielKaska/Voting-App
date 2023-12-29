using System.ComponentModel.DataAnnotations;
using Voting_App.Entities;

namespace Voting_App.Models
{
    public class VoteDto
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public List<AnswerDto> Answers { get; set; }

    }
}
