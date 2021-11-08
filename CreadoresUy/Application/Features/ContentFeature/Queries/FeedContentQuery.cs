using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Share;
using Share.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ContentFeature.Queries
{
    public class GetAllContentQuery : IRequest<IEnumerable<Content>>
    {
        public class GetAllContentQueryHandler : IRequestHandler<GetAllContentQuery, IEnumerable<Content>>
        {
            private readonly ICreadoresUyDbContext _context;
            public GetAllContentQueryHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }
            public async Task<IEnumerable<Content>> Handle(GetAllContentQuery query, CancellationToken cancellationToken)
            {
                var contentList = (from c in _context.Contents
                                   from p in _context.Plans
                                   where 
                                    )


                var contentList = await _context.Contents
                    .Include(cp=> cp.ContentPlans)
                    .ThenInclude(p => p.Plan)
                    .ThenInclude(up=> up.UserPlans)
                    .Include(ct=> ct.ContentTags)
                    .Where(c=> c.ContentPlans.)
                    .ToListAsync();

                var user = await _context.Users.Where(a => a.Id == query.Id).FirstOrDefaultAsync();




                if (contentList == null)
                {
                    return null;
                }
                return contentList.AsReadOnly();
            }
        }
    }
}

