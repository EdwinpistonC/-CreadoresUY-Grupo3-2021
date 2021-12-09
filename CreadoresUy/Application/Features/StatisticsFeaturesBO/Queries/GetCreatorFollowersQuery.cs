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

namespace Application.Features.StatisticsFeaturesBO.Queries
{
    public class GetCreatorFollowersQuery : IRequest<Response<List<StatisticsBODto<string, int>>>>
    {
        public class GetCreatorFollowersQueryHandler : IRequestHandler<GetCreatorFollowersQuery, Response<List<StatisticsBODto<string, int>>>>
        {
            private readonly ICreadoresUyDbContext _context;

            public GetCreatorFollowersQueryHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }

            public async Task<Response<List<StatisticsBODto<string, int>>>> Handle(GetCreatorFollowersQuery request, CancellationToken cancellationToken)
            {
                Response<List<StatisticsBODto<string, int>>> response = new();
                var creadores = await _context.Creators.Include(c => c.UserCreators).Select(c => new { Id = c.Id, Nombre = c.CreatorName, CantSeguidores = c.UserCreators.Where(uc => uc.Unfollow == false).ToList().Count }).ToListAsync();
                List<StatisticsBODto<string, int>> listCreadores = new();
                foreach(var c in creadores)
                {
                    var a = c.CantSeguidores;
                    if (a > 0)
                    {
                        listCreadores.Add(new StatisticsBODto<string, int> { XValue = c.Nombre, YValue = a });
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
