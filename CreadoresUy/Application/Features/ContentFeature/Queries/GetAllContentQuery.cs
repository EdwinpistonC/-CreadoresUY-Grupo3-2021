using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Share;
using Share.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ContentFeature.Queries
{
    public class FeedContentQuery : IRequest<IEnumerable<Content>>
    {
        public int IdUser { get; set; }
        public int Page {  get; set; }
     
        
        
        public class FeedContentQueryHandler : IRequestHandler<FeedContentQuery, IEnumerable<Content>>
        {
            private readonly ICreadoresUyDbContext _context;
            public FeedContentQueryHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }
            public async Task<IEnumerable<Content>> Handle(FeedContentQuery query, CancellationToken cancellationToken)
            {
                
                /*var contentList = await _context.Contents
                    .Include(cp => cp.ContentPlans)
                    .ThenInclude(p =>p.Plan)
                    .ThenInclude(up=>up.UserPlans).ToListAsync();
                */
                if (contentList == null)
                {
                    return null;
                }
                return contentList.AsReadOnly();
            }
        }
    }
}

