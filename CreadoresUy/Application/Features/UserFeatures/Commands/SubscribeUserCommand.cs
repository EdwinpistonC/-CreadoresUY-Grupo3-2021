using Application.Interface;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Share.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserFeatures.Commands
{
    public class SubscribeUserCommand : IRequest<int>
    {
        public int IdCreator { get; set; }
        public int IdPlan { get; set; }

        public int IdUser {  get; set; }


        public class SubscribeUserCommandHandler : IRequestHandler<SubscribeUserCommand, int>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public SubscribeUserCommandHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<int> Handle(SubscribeUserCommand command, CancellationToken cancellationToken)
            {
                var user = await _context.Users.Where(x => x.Id == command.IdUser ).FirstOrDefaultAsync();
                DateTime now = DateTime.Now;
                var userPlan = new UserPlan(command.IdPlan, command.IdUser, now);
                user.UserPlans.Add(userPlan);


                await _context.SaveChangesAsync();
                return user.Id;
            }
        }

    }

}
