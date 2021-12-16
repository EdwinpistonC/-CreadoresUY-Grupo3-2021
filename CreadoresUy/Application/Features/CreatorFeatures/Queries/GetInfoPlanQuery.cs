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

namespace Application.Features.CreatorFeatures.Queries
{
    public class GetInfoPlanQuery : IRequest<Response<InfoPlanDto>>
    {
        public int IdPlan { get; set; }
        public class GetInfoPlanQueryHandler : IRequestHandler<GetInfoPlanQuery, Response<InfoPlanDto>>
        {
            private readonly ICreadoresUyDbContext _contex;

            public GetInfoPlanQueryHandler(ICreadoresUyDbContext contex)
            {
                _contex = contex;
            }

            public async Task<Response<InfoPlanDto>> Handle(GetInfoPlanQuery request, CancellationToken cancellationToken)
            {
                var res = new Response<InfoPlanDto>()
                {
                    Message = new List<string>()
                };

                var pl = _contex.Plans.Where(p => p.Id == request.IdPlan).FirstOrDefault();
                var dto = new InfoPlanDto();
                if(pl == null)
                {
                    res.CodStatus = HttpStatusCode.BadRequest;
                    res.Success = false;
                    res.Message.Add("No se ha encontrado el plan de id: " + request.IdPlan);
                    dto.NoNulls();
                    res.Obj = dto;
                }
                else
                {
                    res.CodStatus = HttpStatusCode.OK;
                    res.Success = true;
                    res.Message.Add(" Exito ");
                    dto.SubscriptionMsg = pl.SubscriptionMsg;
                    dto.WelcomeVideoLink = pl.WelcomeVideoLink;
                    dto.NoNulls();
                    res.Obj = dto;
                }
                return res;
            }
        }
    }
}
