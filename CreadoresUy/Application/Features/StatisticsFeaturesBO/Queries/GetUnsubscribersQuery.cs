using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Share.Dtos;
using Share.Dtos.BackOffice;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.StatisticsFeaturesBO.Queries
{
    public class GetUnsubscribersQuery  : IRequest<Response<List<StatisticsBODto<string, int>>>>
    {
        public class GetUnsubscribersQueryHandler : IRequestHandler<GetUnsubscribersQuery, Response<List<StatisticsBODto<string, int>>>>
        {
            private readonly ICreadoresUyDbContext _context;

            public GetUnsubscribersQueryHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }

            public async  Task<Response<List<StatisticsBODto<string,int>>>> Handle(GetUnsubscribersQuery request, CancellationToken cancellationToken)
            {
                Response<List<StatisticsBODto<string, int >>> response = new();
                var creadores = await _context.Creators.Include(c => c.Plans).ThenInclude(p => p.UserPlans).Select(c => new { Id = c.Id, Nombre = c.CreatorName, CamntPlanes = c.Plans.Count }).ToListAsync();
                List<StatisticsBODto<string, int>> listCreadores = new();

                foreach (var cre in creadores)
                {
                    var cantUsuarios = await _context.Plans.Include(p => p.Creator).Include(c => c.UserPlans).Where(p => p.Creator.Id == cre.Id ).Select(c => new { cantUsuarios = c.UserPlans.Where(up => up.Deleted == true).ToList().Count }).ToListAsync();
                    var suma = 0;
                    foreach (var item in cantUsuarios)
                    {
                        suma += item.cantUsuarios;

                    }
                    if (suma > 0)
                    {
                        listCreadores.Add(new StatisticsBODto<string, int> { XValue = cre.Nombre, YValue = suma });
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
