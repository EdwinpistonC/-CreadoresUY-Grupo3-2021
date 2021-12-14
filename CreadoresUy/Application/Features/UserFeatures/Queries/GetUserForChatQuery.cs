using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Share;
using Share.Dtos;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Models;

namespace Application.Features.UserFeatures.Queries
{
    
    public class GetUserForChatQuery : IRequest<Response<UserChat>>
    {
        public string Nickname { get; set; }

        public class GetUserForChatQueryHandler : IRequestHandler<GetUserForChatQuery, Response<UserChat>>
        {
            private readonly ICreadoresUyDbContext _context;
            public GetUserForChatQueryHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }
            public async Task<Response<UserChat>> Handle(GetUserForChatQuery query, CancellationToken cancellationToken)
            {
                
                var user = _context.Users.Include(u=>u.Creator).Where(a => (a.Creator.NickName == query.Nickname  )).FirstOrDefault();
                Response<UserChat> res = new Response<UserChat>();
                res.Message = new List<string>();

                if (user == null)
                {
                    res.CodStatus = HttpStatusCode.BadRequest;
                    res.Success = false;
                    res.Message.Add("No se ha encontrado el usuario de id " + query.Nickname);
                    return res;
                }

                UserChat u = new();



                u.idCreator = user.CreatorId ?? default(int);
                u.idUser = user.Id;
                u.imgProfile = user.ImgProfile;
                u.name = user.Name;
                res.Obj = u;


                return res;
            }
        }
    }
}
