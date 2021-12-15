using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.CreatorFeatures.Queries
{
    public class GetBoolSubscQuery : IRequest<bool> //Mediador
    {
        public int IdUser { get; set; }
        public string Nickname {  get; set; }
        public class GetBoolSubscQueryHandler : IRequestHandler<GetBoolSubscQuery, bool>
        {
            
            private readonly ICreadoresUyDbContext _context;
            public GetBoolSubscQueryHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }
            public async Task<bool> Handle(GetBoolSubscQuery query, CancellationToken cancellationToken)
            {
                if (query.Nickname != null) {
                    var cre = _context.Creators.Where(c => c.NickName == query.Nickname).Include(c => c.Plans)
                        .ThenInclude(p => p.UserPlans).FirstOrDefault();
                    var usu = _context.Users.Where(u => u.Id == query.IdUser).FirstOrDefault();

                    if (cre == null || usu == null || query.IdUser == 0)
                    {
                        return false;
                    }

                    foreach( var p in cre.Plans)
                    {
                        foreach (var u in p.UserPlans)
                        {
                            if (u.IdUser == usu.Id && u.Deleted == false)
                            {
                                var aux = _context.Plans.Where(x => x.Id == p.Id).Include(pl => pl.Benefits).FirstOrDefault();
                                foreach(var b in aux.Benefits)
                                {
                                    if (b.Description.ToLower() == "chat") return true;
                                }
                                return false;
                            }
                        }
                    }
                    return false;
                }
                return false;
            }
        }

    }
}
