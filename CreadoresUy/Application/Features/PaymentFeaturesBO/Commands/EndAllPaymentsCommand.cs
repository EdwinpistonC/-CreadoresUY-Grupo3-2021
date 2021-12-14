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

namespace Application.Features.PaymentFeaturesBO.Commands
{
    public class EndAllPaymentsCommand : IRequest<Response<string>>
    {
        public List<string> Nicknames { get; set; }
        public class EndAllPaymentsCommandHandler : IRequestHandler<EndAllPaymentsCommand, Response<string>>
        {
            private readonly ICreadoresUyDbContext _context;

            public EndAllPaymentsCommandHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }

            public async Task<Response<string>> Handle(EndAllPaymentsCommand request, CancellationToken cancellationToken)
            {
                Response<string> res = new();
                res.Message = new List<string>();
                foreach (var item in request.Nicknames)
                {
                    var pagos = _context.PagosCreador.Where(x => x.Nickname == item && x.AdeedDate.Month == DateTime.Today.Month && x.AdeedDate.Year == DateTime.Today.Year && x.Pending == true).ToList();
                    if (pagos != null)
                    {
                        foreach (var x in pagos)
                        {
                            x.Pending = false;
                            x.PayDate = DateTime.Today;
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                res.Obj = "Exito";
                res.CodStatus = HttpStatusCode.OK;
                res.Success = true;
                var msj1 = "Ok";
                res.Message.Add(msj1);
                return res;
            }
        }
    }
}
