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
    
    public class GetUsersChatsQuery : IRequest<Response<List<UserChat>>>
    {
        public string Users { get; set; }

        public class GetUsersChatsQueryHandler : IRequestHandler<GetUsersChatsQuery, Response<List<UserChat>>>
        {
            private readonly ICreadoresUyDbContext _context;
            public GetUsersChatsQueryHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }
            public async Task<Response<List<UserChat>>> Handle(GetUsersChatsQuery query, CancellationToken cancellationToken)
            {


                List<string> ids = query.Users.Split('-').ToList();

                var user = _context.Users.Include(u=>u.Creator).Where(u=> ids.Contains(u.Id.ToString())).ToListAsync();
                Response<List<UserChat>> res = new Response<List<UserChat>>();
                res.Message = new List<string>();

                if (user == null)
                {
                    res.CodStatus = HttpStatusCode.BadRequest;
                    res.Success = false;
                    res.Message.Add("No se ha encontrado el usuario de id " + query.Users);
                    return res;
                }
                List<UserChat> uList = new();

                foreach (var item in user.Result)
                {
                    UserChat u = new();

                    u.idCreator = item.CreatorId ?? default(int);
                    u.idUser = item.Id;
                    u.imgProfile = item.ImgProfile;
                    u.name = item.Name;
                    uList.Add(u);

                }
                res.Obj = uList;
                return res;
            }
        }
    }
}
