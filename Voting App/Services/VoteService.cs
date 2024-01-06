using AutoMapper;
using AutoMapper.Configuration.Conventions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Voting_App.Entities;
using Voting_App.Exceptions;
using Voting_App.Models;

namespace Voting_App.Services 
{
    public class VoteService
    {
        private readonly VotingDbContext context;
        private readonly IMapper mapper;

        public VoteService(VotingDbContext _context, IMapper _mapper)
        {
            context = _context;
            mapper = _mapper;
        }

        public Vote GetVoteById(int voteId)
        {
            var vote = context.votes.FirstOrDefault(x => x.Id == voteId);

            if (vote is null)
            {
                throw new NotFoundException("vote not found");
            }

            var answers = context.answers.Where(answers => answers.VoteId == voteId);

            vote.Answers = answers.ToList();

            return vote;
        }

        public Vote Create(VoteDto dto, int userId)
        {
            var answers = new List<Answer>();

            foreach (var answer in dto.Answers) //add answers to list
            {
                answers.Add(mapper.Map<Answer>(answer));
            }

            var vote = new Vote()
            {
                Name = dto.Name,
                Description = dto.Description,
                Answers = answers,
                CreatedBy = userId,
            };

            return vote;
        }

        public Answer GetAnswerFromVote(Vote v, string answerName)
        {
            var a = v.Answers.FirstOrDefault(a => a.Name == answerName);

            if (a is null)
                throw new NotFoundException("answer not found");

            return a;
        }

    }
}
