using Application.Interface;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Share.Dtos;
using Share.Dtos.BackOffice;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.StatisticsFeaturesBO.Queries
{      
    
    //cuantos pagos por mes

    public class GetFinancesCreatorQuery : IRequest<Response<List<StatisticsBODto<DateTime, double>>>>
    {
        public int idCreator { get; set; }


        public class GetFinancesCreatorQueryHandler : IRequestHandler<GetFinancesCreatorQuery, Response<List<StatisticsBODto<DateTime, double>>>>
        {

            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public GetFinancesCreatorQueryHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<List<StatisticsBODto<DateTime, double>>>> Handle(GetFinancesCreatorQuery query, CancellationToken cancellationToken)
            {
                Response<List<StatisticsBODto<DateTime, double>>> response = new();

                var pagos = await _context.PagosCreador.Where(pc=> pc.IdCreator==query.idCreator).GroupBy(p =>new { p.AdeedDate.Year, p.AdeedDate.Month}).Select(p => new { Fecha = p.Key, Pagos = p.Sum( x=> x.Amount)}).ToListAsync();

                List<StatisticsBODto<DateTime, double>> listPagos = new();
                foreach (var item in pagos)
                {
                    listPagos.Add(new StatisticsBODto<DateTime, double> { XValue= new DateTime(item.Fecha.Year, item.Fecha.Month, 1) , YValue = item.Pagos });
                }

                response.Obj = listPagos;
                response.CodStatus = HttpStatusCode.OK;
                response.Success = true;
                response.Message = new();
                var msj1 = "Ok";
                response.Message.Add(msj1);

                return response;
            }

        }
    }
}
