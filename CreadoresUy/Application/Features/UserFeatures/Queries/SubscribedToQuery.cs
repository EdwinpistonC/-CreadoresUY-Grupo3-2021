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

namespace Application.Features.UserFeatures.Queries
{
    public class SubscribedToQuery : IRequest<Response<List<SubscriberDto>>>
    {
        public int IdUser { get; set; }
        public class SubscribedToQueryHandler : IRequestHandler<SubscribedToQuery, Response<List<SubscriberDto>>>
        {
            private readonly ICreadoresUyDbContext _context;
            public SubscribedToQueryHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }

            public async Task<Response<List<SubscriberDto>>> Handle(SubscribedToQuery request, CancellationToken cancellationToken)
            {
                var res = new Response<List<SubscriberDto>>()
                {
                    Message = new List<string>()
                };
                var list = new List<SubscriberDto>();
                var usr = _context.Users.Where(u => u.Id == request.IdUser).Include(up => up.UserPlans)
                  .ThenInclude(p => p.Plan).FirstOrDefault();

                if (usr != null)
                {
                    foreach(var item in usr.UserPlans)
                    {
                        if (!item.Deleted)
                        {
                            var cre = _context.Creators.Where(c => c.Id == item.Plan.CreatorId).FirstOrDefault();
                            var creador = new SubscriberDto(cre.CreatorName, cre.CreatorImage, item.SusbscriptionDate)
                            {
                                Id = cre.Id
                            };
                            list.Add(creador);
                        }
                    }
                    if (list.Count > 0)
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
                        res.Message.Add("El usuario no se encuentra suscripto a ningun plan");
                        res.Obj = list;
                    }
                }
                else
                {
                    res.CodStatus = HttpStatusCode.BadRequest;
                    res.Success = false;
                    res.Message.Add("No se ha encontrado el usuario de id " + request.IdUser);
                    res.Obj = list;
                }
                return res;
            }
        }
    }
}
