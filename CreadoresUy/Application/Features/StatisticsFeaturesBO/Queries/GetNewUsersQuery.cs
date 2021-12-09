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

    public class GetNewUsersQuery : IRequest<Response<List<StatisticsBODto<DateTime,float>>>>
    {

        public class GetNewUsersQueryHandler : IRequestHandler<GetNewUsersQuery, Response<List<StatisticsBODto<DateTime, float>>>>
        {

            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public GetNewUsersQueryHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<List<StatisticsBODto<DateTime, float>>>> Handle(GetNewUsersQuery query, CancellationToken cancellationToken)
            {
                Response<List<StatisticsBODto<DateTime, float>>> response = new();

                var pagos = await _context.Users.GroupBy(p =>new { p.Created.Year, p.Created.Month, p.Created.Day }).Select(p => new { Fecha = p.Key, Usuarios = p.Count()}).ToListAsync();

                List<StatisticsBODto<DateTime, float>> listPagos = new();
                foreach (var item in pagos)
                {
                    listPagos.Add(new StatisticsBODto<DateTime, float> { XValue= new DateTime(item.Fecha.Year, item.Fecha.Month, item.Fecha.Day) , YValue = item.Usuarios });
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
