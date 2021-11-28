using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Share.Dtos;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserFeatures.Commands
{
    public class FollowCommand : IRequest<Response<string>>
    {
        public int IdUser {  get; set; }
        public string Nickname { get; set; }
        public class FollowCommandHandler : IRequestHandler<FollowCommand, Response<string>>
        {
            private readonly ICreadoresUyDbContext _context;
            public FollowCommandHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }
            public async Task<Response<string>> Handle(FollowCommand command, CancellationToken cancelation)
            {
                var cre = _context.Creators.Where(c => c.NickName == command.Nickname).Include(c => c.UserCreators).FirstOrDefault(); ;
                var usr = _context.Users.Where(u => u.Id == command.IdUser).FirstOrDefault();
                var resp = new Response<string>
                {
                    Message = new List<string>()
                };

                if (usr == null || cre == null)
                {
                    if (usr == null) resp.Message.Add("No se ha encontrado el usuario");
                    if (cre == null) resp.Message.Add("No se ha encontrado el creador");
                    resp.CodStatus = HttpStatusCode.BadRequest;
                    resp.Success = false;
                    resp.Obj = "";
                    return resp;
                }
                var exist = _context.UserCreators.Where(uc => uc.IdCreator == cre.Id)
                    .Where(uc => uc.IdUser == command.IdUser).FirstOrDefault();

                if (exist != null)
                {
                    exist.Unfollow = false;
                    exist.DateUnfollow = DateTime.MinValue;
                }
                if(exist == null)
                {
                    var foll = new UserCreator(cre.Id, usr.Id, DateTime.UtcNow);
                    foll.Creator = cre;
                    foll.User = usr;
                    foll.DateUnfollow = DateTime.MinValue;
                    _context.UserCreators.Add(foll);
                    cre.UserCreators.Add(foll);
                    usr.UserCreators.Add(foll);
                }
                cre.Followers += 1;
                
                await _context.SaveChangesAsync();
                resp.CodStatus = HttpStatusCode.OK;
                resp.Success = true;
                resp.Message.Add("exito");
                string obj = usr.Name + " comenzaste a seguir a " + cre.CreatorName;
                resp.Obj = obj;
                return resp;
            }
        }
    }
}
