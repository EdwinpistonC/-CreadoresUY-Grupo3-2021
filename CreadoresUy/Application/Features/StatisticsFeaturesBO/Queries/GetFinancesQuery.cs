﻿using Application.Interface;
using AutoMapper;
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

namespace Application.Features.StatisticsFeaturesBO.Queries
{      
    
    //cuantos pagos por mes

    public class GetFinancesQuery : IRequest<Response<List<StatisticsBODto<DateTime>>>>
    {

        public class GetFinancesQueryHandler : IRequestHandler<GetFinancesQuery, Response<List<StatisticsBODto<DateTime>>>>
        {

            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public GetFinancesQueryHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<List<StatisticsBODto<DateTime>>>> Handle(GetFinancesQuery query, CancellationToken cancellationToken)
            {
                Response<List<StatisticsBODto<DateTime>>> response = new();

                var pagos = await _context.PagosCreador.GroupBy(p =>new { p.AdeedDate.Year, p.AdeedDate.Month}).Select(p => new { Fecha = p.Key, Pagos = p.Sum( x=> x.Amount)}).ToListAsync();

                List<StatisticsBODto<DateTime>> listPagos = new();
                foreach (var item in pagos)
                {
                    listPagos.Add(new StatisticsBODto<DateTime> { XValue= new DateTime(item.Fecha.Year, item.Fecha.Month, 1) , YValue = item.Pagos });
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
