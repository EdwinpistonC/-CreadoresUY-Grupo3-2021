using Application.Features.Validators;
using Application.Interface;
using Application.Service;
using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Share.Dtos;
using Share.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.CreatorFeatures.Queries
{
    public class GetCreatorPlansByNicknameQuery : IRequest<Response<PlansAndIdDto>>
    {
        public int IdUser { get; set; }
        public string Nickname { get; set; }
        public class GetCreatorPlansByNicknameQueryHandler : IRequestHandler<GetCreatorPlansByNicknameQuery, Response<PlansAndIdDto>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;
            public GetCreatorPlansByNicknameQueryHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Response<PlansAndIdDto>> Handle(GetCreatorPlansByNicknameQuery query, CancellationToken cancellationToken)
            {

                var respuesta = new Response<PlansAndIdDto>
                {
                    Message = new List<string>()
                };
                var creador = _context.Creators.Where(c => c.NickName == query.Nickname).Include(c => c.Plans).
                    ThenInclude(p => p.Benefits).Include(c => c.Plans).ThenInclude(p => p.UserPlans).FirstOrDefault();                
                var listresp = new List<UpdatePlanAndBenefitsDto>();
                var obj = new PlansAndIdDto();
                if (creador == null)
                {
                    obj.SubscribedTo = 0;
                    obj.Plans = listresp;
                    respuesta.Obj = obj;
                    respuesta.CodStatus = HttpStatusCode.BadRequest;
                    respuesta.Success = false;
                    respuesta.Message.Add("Error - No se ha encontrado el Id ingresado");
                    return respuesta;
                }
                foreach (var item in creador.Plans)
                {
                    foreach (var up in item.UserPlans)
                    {
                        if(up.IdUser == query.IdUser && up.Deleted == false)
                        {
                            obj.SubscribedTo = up.IdPlan;
                        }
                    }
                    var pl = _mapper.Map<UpdatePlanAndBenefitsDto>(item);
                    pl.IdPlan = item.Id;
                    pl.Benefits = new List<string>();
                    foreach (var b in item.Benefits)
                    {
                        pl.Benefits.Add(b.Description);
                    }
                    pl.FixIsNull();
                    listresp.Add(pl);
                }
                obj.Plans = listresp;
                respuesta.Obj = obj;
                respuesta.CodStatus = HttpStatusCode.OK;
                respuesta.Success = true;
                respuesta.Message.Add("Exito");
                return respuesta;

            }
        }
    }
}
