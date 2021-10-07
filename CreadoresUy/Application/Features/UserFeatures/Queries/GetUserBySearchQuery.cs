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
    
    public class GetUserBySearchQuery : IRequest<IEnumerable<User>>
    {
        public string SearchText { get; set; }
        public int SizePage {  get; set; }
        public int Page { get; set; }

        public class GetUserBySearchQueryHandler : IRequestHandler<GetUserBySearchQuery, IEnumerable<User>>
        {
            private readonly ICreadoresUyDbContext _context;
            public GetUserBySearchQueryHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }
            public async Task<IEnumerable<User>> Handle(GetUserBySearchQuery query, CancellationToken cancellationToken)
            {
                
                var userList = await _context.Users.Where(a => (a.Name.Contains(query.SearchText) || a.Description.Contains(query.SearchText)) ).Skip(query.Page * query.SizePage).Take(query.SizePage).ToListAsync();

                if (userList == null) return null;

                return userList.AsReadOnly();
            }
        }
    }
}
