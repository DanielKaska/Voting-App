using AutoMapper;
using Voting_App.Models;
using Voting_App.Entities;

namespace Voting_App
{

    //automapper mapping profile
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<VoteDto, Vote>(); //converting from VoteDto to Vote
            CreateMap<AnswerDto, Answer>();
        }
    }
}
