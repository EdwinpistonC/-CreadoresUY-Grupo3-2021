using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Share;
using Share.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserFeatures.Queries
{
    public class GetAllUsersQuery : IRequest<IEnumerable<User>>
    {
        public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<User>>
        {
            private readonly ICreadoresUyDbContext _context;
            public GetAllUsersQueryHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }
            public async Task<IEnumerable<User>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
            {
                var userList = await _context.Users.ToListAsync();
                if (userList == null)
                {
                    return null;
                }
                return userList.AsReadOnly();
            }
        }
    }
}

