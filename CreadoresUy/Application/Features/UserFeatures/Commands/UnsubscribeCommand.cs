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

namespace Application.Features.UserFeatures.Commands
{
    public class UnsubscribeCommand : IRequest<Response<string>>
    {
        public int IdUser { get; set; }
        public int IdPlan { get; set; }
        public class UnsubscribeCommandHandler : IRequestHandler<UnsubscribeCommand, Response<string>>
        {
            private readonly ICreadoresUyDbContext _context;

            public UnsubscribeCommandHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }

            public async Task<Response<string>> Handle(UnsubscribeCommand request, CancellationToken cancellationToken)
            {
                var resp = new Response<string>()
                {
                    Message = new List<string>()
                };

                var userPla = _context.UserPlans.Where(up => up.IdUser == request.IdUser && up.IdPlan == request.IdPlan)
                                .FirstOrDefault();
                if (userPla == null || userPla.Deleted == true)
                {
                    resp.Success = false;
                    resp.Message.Add("Error");
                    resp.Obj = "No se ha encontrado plan al cual desuscribirse";
                    resp.CodStatus = HttpStatusCode.BadRequest;
                }
                else
                {
                    userPla.Deleted = true;
                    await _context.SaveChangesAsync();
                    resp.Success = true;
                    resp.Message.Add("Exito");
                    resp.Obj = "Exito ";
                    resp.CodStatus = HttpStatusCode.OK;
                }
                return resp;
            }
        } 
    }
}
