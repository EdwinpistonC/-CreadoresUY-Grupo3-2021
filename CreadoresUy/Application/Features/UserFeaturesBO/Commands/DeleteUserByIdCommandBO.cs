using Application.Interface;
using MediatR;
using Share.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserFeaturesBO.Commands
{
    public class DeleteUserByIdCommandBO : IRequest<Response<String>>
    {
        public int Id {  get; set; }

        public class DeleteUserByIdCommandBOHandler : IRequestHandler<DeleteUserByIdCommandBO, Response<String>>
        {
            private readonly ICreadoresUyDbContext _context;
            public DeleteUserByIdCommandBOHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }
            public async Task<Response<String>> Handle(DeleteUserByIdCommandBO command, CancellationToken cancellationToken)
            {
                var usr = _context.Users.Where(u => u.Id == command.Id).FirstOrDefault();
                Response<string> res = new();
                res.Message = new List<string>();
                if(usr == null )
                {
                    res.Obj = "";
                    res.CodStatus = HttpStatusCode.BadRequest;
                    res.Success = false;
                    var msj = "Id no encontrado ";
                    res.Message.Add(msj);
                    return res;
                }
                usr.Deleted = !usr.Deleted;
                await _context.SaveChangesAsync();
                res.Obj = "";
                res.CodStatus = HttpStatusCode.OK;
                res.Success = true;
                res.Message.Add("Exito");
                return res;
            }
        }
    }
    
}
