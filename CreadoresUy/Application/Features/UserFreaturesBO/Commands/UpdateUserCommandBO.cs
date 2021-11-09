using Application.Interface;
using AutoMapper;
using MediatR;
using Share.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserFreaturesBO.Commands
{
    public class UpdateUserCommandBO : IRequest<Response<string>>
    {
        public int Id {  get; set; }
        public UpdateUserDto User {  get; set; }

        public class UpdateUserCommandBOHandler : IRequestHandler<UpdateUserCommandBO, Response<string>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public UpdateUserCommandBOHandler(ICreadoresUyDbContext context, IMapper mapper )
            {
                _context = context;
                _mapper = mapper;   
            }
            public async Task<Response<string>> Handle(UpdateUserCommandBO command, CancellationToken cancellationToken)
            {
                var user = _context.Users.Where(u => u.Id == command.Id).FirstOrDefault();
                Response<string> res = new()
                {
                    Message = new List<string>(),
                    Obj = ""
                };
                if (user == null )
                {
                    res.Message.Add("No se ha encontrado el usuario de id: " + command.Id);
                    res.Success = false;
                    res.CodStatus = HttpStatusCode.BadRequest;
                    return res;
                }
                if (command.User.Name != "") user.Name = command.User.Name;
                if (command.User.Email != "") user.Email = command.User.Email;
                if (command.User.Description != "") user.Description = command.User.Description;
                if (command.User.ImgProfile != "") user.ImgProfile = command.User.ImgProfile;
                if(command.User.Deleted != null) user.Deleted = (bool)command.User.Deleted;
                await _context.SaveChangesAsync();
                res.Message.Add("Exito");
                res.Success = true;
                res.CodStatus = HttpStatusCode.OK;
                return res;
            }

        }
        
    }
   
}
