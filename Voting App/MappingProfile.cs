﻿using AutoMapper;
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
            CreateMap<Vote, ShowVoteDto>(); //converting from VoteDto to Vote
            CreateMap<AnswerDto, Answer>();

            CreateMap<Answer, ShowAnswerDto>().ForMember( //convert Answer to ShowAnswerDto. Fields name dont match. Custom mapping made
                dest => dest.VoteCount,
                opt => opt.MapFrom(src => src.VoteCounter)
            );
        }
    }
}
