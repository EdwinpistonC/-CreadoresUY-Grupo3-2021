using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Share.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.CreatorFeatures.Queries
{
    public class GetSubscribersQuery : IRequest<Response<List<SubscriberDto>>>
    {
        public int IdCreator { get; set; }
        public class GetSubscribersQueryHandler : IRequestHandler<GetSubscribersQuery, Response<List<SubscriberDto>>>
        {
            private readonly ICreadoresUyDbContext _context; 
            public GetSubscribersQueryHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }

            public async Task<Response<List<SubscriberDto>>> Handle(GetSubscribersQuery query, CancellationToken cancellationToken)
            {
                var res = new Response<List<SubscriberDto>>() { 
                    Message = new List<string>()
                };
                var cre = _context.Creators.Where(c => c.Id == query.IdCreator)
                               .Include(c => c.Plans).FirstOrDefault();
                var list = new List<SubscriberDto>();
                if (cre != null)
                {
                    foreach (var pl in cre.Plans)
                    {
                        var plan = _context.Plans.Where(p => p.Id == pl.Id)
                        .Include(p => p.UserPlans).ThenInclude(p => p.User).Include(p => p.ContentPlans).FirstOrDefault();
                        foreach (var usu in plan.UserPlans)
                        {
                            if (usu.Deleted != true)
                            {
                                var usr = new SubscriberDto(usu.User.Name, usu.User.ImgProfile, usu.SusbscriptionDate);
                                list.Add(usr);
                            }
                        }
                    }
                    if(list.Count > 0)
                    {
                        res.CodStatus = HttpStatusCode.OK;
                        res.Success = true;
                        res.Message.Add("Exito");
                        res.Obj = list;
                    }
                    else
                    {
                        res.CodStatus = HttpStatusCode.OK;
                        res.Success = true;
                        res.Message.Add("No se han encontrado usuarios subscriptos a los planes del creador");
                        res.Obj = list;
                    }
                }
                else
                {
                    res.CodStatus = HttpStatusCode.BadRequest;
                    res.Success = false;
                    res.Message.Add("No se ha encontrado el creador de id "+query.IdCreator);
                    res.Obj = list;
                }

                return res;
            }
        }
    }
}
