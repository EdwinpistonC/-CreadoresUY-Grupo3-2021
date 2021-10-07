using Application.Interface;
using AutoMapper;
using MediatR;
using Share;
using Share.Dtos;
using Share.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserFeatures.Commands
{
    public class CreateUserCommand :IRequest<int>
    {
        public UserDto User {  get; set; }
        public int CreatorId { get; set; }

        public Creator? Creator { get; set; }

        public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public CreateUserCommandHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<int> Handle(CreateUserCommand command, CancellationToken cancellationToken)
            {
                var user = _mapper.Map<User>(command.User);

                if (command.Creator != null)
                {
                    var creator = _context.Creators.Find(command.Creator.Id);
                    if (creator == null)
                    {
                        user.Creator = command.Creator;
                    }
                    else
                    {
                        user.CreatorId = command.Creator.Id;
                    }
                }
                if (command.CreatorId != 0)
                {
                    user.CreatorId = command.CreatorId;
                }
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return user.Id;            }
        }

    }
}
