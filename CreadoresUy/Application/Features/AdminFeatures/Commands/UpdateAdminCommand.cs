using Application.Interface;
using MediatR;
using Share.Dtos;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AdminFeatures.Commands
{

    public class UpdateAdminCommand : IRequest<Response<String>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LasLogin { get; set; }
        public string? ImgProfile { get; set; }
        public int CreatorId { get; set; }

        public Creator? Creator { get; set; }

        public class UpdateAdminHandler : IRequestHandler<UpdateAdminCommand, Response<String>>
        {
            private readonly ICreadoresUyDbContext _context;
            public UpdateAdminHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }
            public async Task<Response<String>> Handle(UpdateAdminCommand command, CancellationToken cancellationToken)
            {
                var user = _context.Users.Where(a => a.Id == command.Id).FirstOrDefault();
                Response<string> res = new Response<String>
                {
                    Obj = "",
                    Message = new List<String>()
                };

                var msg = "";

                if (user == null)
                {
                    res.Success = false;
                    res.CodStatus = HttpStatusCode.BadRequest;
                    msg = "Id no pertenece a un admin";
                    res.Message.Add(msg);
                    return res;

                }
                else

                {
                    user.Name = command.Name;
                    user.Email = command.Email;
                    user.Password = command.Password;
                    user.Description = command.Description;
                    user.Created = command.Created;
                    user.LasLogin = command.LasLogin;
                    user.ImgProfile = command.ImgProfile;
                    if (command.Creator != null)
                    {
                        user.Creator = command.Creator;
                    }
                    if (command.CreatorId != 0)
                    {
                        user.CreatorId = command.CreatorId;
                    }
                    await _context.SaveChangesAsync();
                    res.Success = true;
                    res.CodStatus = HttpStatusCode.OK;
                    msg = "Admin eliminado";
                    res.Message.Add(msg);
                    return res;
                }
            }
        }
    }
}
