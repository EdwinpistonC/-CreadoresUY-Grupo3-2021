using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Share.Dtos;
using Share.Dtos.BackOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PaymentFeaturesBO.Queries
{
    public class GetPendingPaymentsByNicknameQuery : IRequest<Response<List<StatisticsBODto<DateTime, double>>>>
    {
        public string Nickname { get; set; }
        public class GetPendingPaymentsByNicknameQueryHandler : IRequestHandler<GetPendingPaymentsByNicknameQuery, Response<List<StatisticsBODto<DateTime, double>>>>
        {
            private readonly ICreadoresUyDbContext _context;

            public GetPendingPaymentsByNicknameQueryHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }

            public async Task<Response<List<StatisticsBODto<DateTime, double>>>> Handle(GetPendingPaymentsByNicknameQuery request, CancellationToken cancellationToken)
            {
                Response<List<StatisticsBODto<DateTime, double>>> res = new();
                res.Message = new List<string>();
                var pagos = new List<StatisticsBODto<DateTime, double>>();
                var resultados = await _context.PagosCreador.Where(x =>x.Nickname == request.Nickname && x.AdeedDate.Month == DateTime.Today.Month && x.AdeedDate.Year == DateTime.Today.Year && x.Pending == true)
                    .GroupBy(x => x.AdeedDate).Select(c => new { Fecha = c.Key, sum = c.Sum(x => x.Amount) })
                    .ToListAsync();

                if(resultados.Count == 0)
                {
                    res.Obj = pagos;
                    res.CodStatus = HttpStatusCode.BadRequest;
                    res.Success = false;
                    var msj2 = " No hay pagos asociados al creador ";
                    res.Message.Add(msj2);
                    return res;

                }

                foreach (var xx in resultados)
                {
                    var dto = new StatisticsBODto<DateTime, double>
                    {
                        XValue = xx.Fecha,
                        YValue = xx.sum,
                    };
                    pagos.Add(dto);
                }

                res.Obj = pagos;
                res.CodStatus = HttpStatusCode.OK;
                res.Success = true;
                var msj1 = "Ok";
                res.Message.Add(msj1);
                return res;

            }
        }
    }
}
