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
    public class UnfollowCommand : IRequest<Response<string>>
    {
        public int IdUser { get; set; }
        public string Nickname { get; set; }
        public class UnfollowCommandHandler : IRequestHandler<UnfollowCommand, Response<string>>
        {
            private readonly ICreadoresUyDbContext _context;
            public UnfollowCommandHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }
            public async Task<Response<string>> Handle(UnfollowCommand command, CancellationToken cancelation)
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

                if (exist == null || exist.Unfollow == true)
                {
                    resp.Message.Add("No se ha encontrado relacion entre usr y cre");
                    resp.CodStatus = HttpStatusCode.BadRequest;
                    resp.Success = false;
                    resp.Obj = "";
                    return resp;
                }
                if (exist != null)
                {
                    exist.Unfollow = true;
                    exist.DateUnfollow = DateTime.UtcNow;
                }
                cre.Followers -= 1;
                
                await _context.SaveChangesAsync();
                resp.CodStatus = HttpStatusCode.OK;
                resp.Success = true;
                resp.Message.Add("exito");
                string obj = usr.Name + " dejaste de seguir a " + cre.CreatorName;
                resp.Obj = obj;
                return resp;
            }
        }
    }
}
