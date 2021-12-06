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
   public class FollowingToQuery : IRequest<Response<List<SubscriberDto>>>
    {
        public int IdUser { get; set; }

        public class FollowingToQueryHandler : IRequestHandler<FollowingToQuery, Response<List<SubscriberDto>>>
        {
            private readonly ICreadoresUyDbContext _context;

            public FollowingToQueryHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }

            public async Task<Response<List<SubscriberDto>>> Handle(FollowingToQuery request, CancellationToken cancellation )
            {
                var res = new Response<List<SubscriberDto>>()
                {
                    Message = new List<string>()
                };
                var list = new List<SubscriberDto>();
                var usr = _context.Users.Where(u => u.Id == request.IdUser).Include(u => u.UserCreators)
                  .ThenInclude(uc => uc.Creator).FirstOrDefault();

                if (usr != null)
                {
                    foreach (var item in usr.UserCreators)
                    {
                        if (item.Unfollow != true)
                        {
                            var cre = item.Creator;
                            var creador = new SubscriberDto(cre.CreatorName, cre.CreatorImage, item.DateFollow)
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
                        res.Message.Add("El usuario no sigue a ningun Creador");
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
