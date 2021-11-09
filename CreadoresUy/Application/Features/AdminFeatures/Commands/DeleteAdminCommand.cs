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

namespace Application.Features.AdminFeatures.Commands
{

    
    public class DeleteAdminCommand : IRequest<Response<String>>
    {
        public int Id { get; set; }
        public class DeleteAdminCommandHandler : IRequestHandler<DeleteAdminCommand, Response<String>>
        {
            private readonly ICreadoresUyDbContext _context;
            public DeleteAdminCommandHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }
            public async Task<Response<String>> Handle(DeleteAdminCommand command, CancellationToken cancellationToken)
            {
                var user = await _context.Users.Where(a => (a.Id == command.Id && a.IsAdmin== true)).FirstOrDefaultAsync();


                Response<string> res = new Response<String>
                {
                    Obj = "",
                    Message = new List<String>()
                };
                var msg="";
                if (user == null) {
                    res.Success = false;
                    res.CodStatus = HttpStatusCode.BadRequest;
                    msg = "Id no pertenece a un admin";
                    res.Message.Add(msg);
                    return res;

                }; 
                _context.Users.Remove(user);
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
