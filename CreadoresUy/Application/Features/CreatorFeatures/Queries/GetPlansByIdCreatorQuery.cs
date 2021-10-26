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
    public class GetPlansByIdCreatorQuery : IRequest<Response<List<BasePlanDto>>>
    {
        public int IdCreator { get; set; }
        public class GetPlansByIdCreatorQueryHandler : IRequestHandler<GetPlansByIdCreatorQuery, Response<List<BasePlanDto>>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;
            public GetPlansByIdCreatorQueryHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<List<BasePlanDto>>> Handle(GetPlansByIdCreatorQuery query, CancellationToken cancellationToken)
            {
                var usr = _context.Users.Where(u => u.CreatorId == query.IdCreator).Include(c => c.Creator.Plans).FirstOrDefault();
                Response<List<BasePlanDto>> res = new Response<List<BasePlanDto>>();
                List<BasePlanDto> plans = new List<BasePlanDto>();
                res.Message = new List<string>();
                if (usr != null && usr.CreatorId != null)
                { 
                    var cre1 = usr.Creator; 
                    
                    if (cre1.Plans != null) { 
                        foreach (var item in cre1.Plans)
                        {
                            var plan = _mapper.Map<BasePlanDto>(item);
                            plans.Add(plan);
                        }
                        res.Obj = plans;
                        res.Success = true;
                        res.CodStatus = HttpStatusCode.OK;
                        var msj = "Exito";
                        res.Message.Add(msj);
                        return res;
                    }
                    res.Obj = plans;
                    res.Success = false;
                    res.CodStatus = HttpStatusCode.BadRequest;
                    var msj1 = "No se han encontrado planes en el creador";
                    res.Message.Add(msj1);
                    return res;
                }
                res.Obj = plans;
                res.Success = false;
                res.CodStatus = HttpStatusCode.BadRequest;
                var msj2 = "Usuario incorrecto o no tiene un creador asociado";
                res.Message.Add(msj2);
                return res;
            }
        }
    }
}
