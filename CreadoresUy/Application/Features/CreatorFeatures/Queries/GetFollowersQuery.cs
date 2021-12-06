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
    public class GetFollowersQuery : IRequest<Response<List<SubscriberDto>>>
    {
        public int IdCreator { get; set; }
        public class GetFollowersQueryHandler : IRequestHandler<GetFollowersQuery, Response<List<SubscriberDto>>>
        {
            private readonly ICreadoresUyDbContext _context;

            public GetFollowersQueryHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }

            public async Task<Response<List<SubscriberDto>>> Handle(GetFollowersQuery request, CancellationToken cancellationToken)
            {
                var res = new Response<List<SubscriberDto>>()
                {
                    Message = new List<string>()
                };
                var cre = _context.Creators.Where(c => c.Id == request.IdCreator)
                               .Include(c => c.UserCreators).ThenInclude(uc => uc.User).FirstOrDefault();
                var list = new List<SubscriberDto>();

                if (cre != null)
                {
                    foreach (var item in cre.UserCreators)
                    {
                        if(item.Unfollow != true) { 
                            var usr = item.User;
                            var usuario = new SubscriberDto(usr.Name, usr.Name, item.DateFollow)
                            {
                                Id = 0
                            };
                            list.Add(usuario);
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
                        res.Message.Add("El Creador no tiene ningun seguidor");
                        res.Obj = list;
                    }
                }
                else
                {
                    res.CodStatus = HttpStatusCode.BadRequest;
                    res.Success = false;
                    res.Message.Add("No se ha encontrado el Creador de id " + request.IdCreator);
                    res.Obj = list;
                }
                return res;
            }
        }
    }
}
