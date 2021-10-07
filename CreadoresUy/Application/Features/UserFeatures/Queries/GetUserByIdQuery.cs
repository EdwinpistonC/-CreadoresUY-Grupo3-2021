using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Share;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserFeatures.Queries
{

    public class GetUserById : IRequest<User>
    {
        public int Id { get; set; }
        public class GetUserByIdHandler : IRequestHandler<GetUserById, User>
        {
            private readonly ICreadoresUyDbContext _context;
            public GetUserByIdHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }
            public async Task<User> Handle(GetUserById query, CancellationToken cancellationToken)
            {
                var user = await _context.Users.Where(a => a.Id == query.Id).FirstOrDefaultAsync();
                if (user.CreatorId != null)
                {
                    user.Creator = _context.Creators.Where(a => a.Id == user.CreatorId).FirstOrDefault();
                }

                if (user == null) return null;
                return user;
            }
        }
    }
}
