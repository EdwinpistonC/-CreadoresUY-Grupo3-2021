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

    public class GetCreatorsSubsQuery : IRequest<Response<List<StatisticsBODto<string, float>>>>
    {

        public class GetCreatorsSubsQueryHandler : IRequestHandler<GetCreatorsSubsQuery, Response<List<StatisticsBODto<string, float>>>>
        {

            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public GetCreatorsSubsQueryHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<List<StatisticsBODto<string, float>>>> Handle(GetCreatorsSubsQuery query, CancellationToken cancellationToken)
            {
                Response<List<StatisticsBODto<string, float>>> response = new();

                var creadores = await _context.Creators.Include(c=>c.Plans).ThenInclude(p =>p.UserPlans).Select(c => new { Id = c.Id, Nombre = c.CreatorName, CamntPlanes = c.Plans.Count}).ToListAsync();

                List<StatisticsBODto<string, float>> listCreadores = new();

                foreach (var cre in creadores)
                {
                    var cantUsuarios = await _context.Plans.Include(p=>p.Creator).Include(c => c.UserPlans).Where(p=>p.Creator.Id== cre.Id ).Select(c => new { cantUsuarios = c.UserPlans.Where(up=> up.Deleted==false).ToList().Count }).ToListAsync();
                    var suma = 0;
                    foreach (var item in cantUsuarios)
                    {
                        suma += item.cantUsuarios;

                    }
                    if (suma>0)
                    {
                        listCreadores.Add(new StatisticsBODto<string, float> { XValue = cre.Nombre, YValue = suma });
                    }

                }

                response.Obj = listCreadores;
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
