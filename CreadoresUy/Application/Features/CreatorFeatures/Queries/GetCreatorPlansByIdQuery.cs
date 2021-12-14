using Application.Interface;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Share.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.CreatorFeatures.Queries
{
    public class GetCreatorPlansByIdQuery : IRequest<Response<List<UpdatePlanAndBenefitsDto>>>
    {
        public int CreatorId { get; set; }
        public class GetCreatorPlansByIdQueryHandler : IRequestHandler<GetCreatorPlansByIdQuery, Response<List<UpdatePlanAndBenefitsDto>>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;
            public GetCreatorPlansByIdQueryHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Response<List<UpdatePlanAndBenefitsDto>>> Handle(GetCreatorPlansByIdQuery query, CancellationToken cancellationToken)
            {

                var respuesta = new Response<List<UpdatePlanAndBenefitsDto>>
                {
                    Message = new List<string>()
                };
                var creador = _context.Creators.Where(c => c.Id == query.CreatorId).Include(c => c.Plans).
                                    ThenInclude(p => p.Benefits).FirstOrDefault();
                var listresp = new List<UpdatePlanAndBenefitsDto>();
                if (creador == null)
                {
                    respuesta.Obj = listresp;
                    respuesta.CodStatus = HttpStatusCode.BadRequest;
                    respuesta.Success = false;
                    respuesta.Message.Add("Error - No se ha encontrado el Id ingresado");
                    return respuesta;
                }
                foreach(var item in creador.Plans)
                {
                    var pl = _mapper.Map<UpdatePlanAndBenefitsDto>(item);
                    pl.IdPlan = item.Id;
                    pl.Benefits = new List<string>();
                    foreach(var b in item.Benefits)
                    {
                        pl.Benefits.Add(b.Description);
                    }
                    pl.FixIsNull();
                    listresp.Add(pl);
                }
                respuesta.Obj = listresp;
                respuesta.CodStatus = HttpStatusCode.OK;
                respuesta.Success = true;
                respuesta.Message.Add("Exito");
                return respuesta;

            }
        }
    }
}
