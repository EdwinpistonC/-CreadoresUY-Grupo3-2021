using Application.Interface;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Share.Dtos;
using Share.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DefaultPlanFeaturesBO.Queries
{
    public class GetAllDefaultPlanBOQuery : IRequest<Response<List<DefaultPlanBODto>>>
    {

        public class GetAllDefaultPlanBOQueryHandler : IRequestHandler<GetAllDefaultPlanBOQuery, Response<List<DefaultPlanBODto>>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;
            public GetAllDefaultPlanBOQueryHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<List<DefaultPlanBODto>>> Handle(GetAllDefaultPlanBOQuery query, CancellationToken cancellationToken)
            {
                Response<List<DefaultPlanBODto>> res = new();
                res.Message = new List<string>();


                List<DefaultPlan> defaultPlan = await _context.DefaultPlans.ToListAsync();
                        //.Where(u => u.Deleted.Equals(false)) --En caso de no querer listar los eliminados logicamente
                if (defaultPlan == null)
                {
                    res.Obj = default;
                    res.CodStatus = HttpStatusCode.BadRequest;
                    res.Success = false;
                    var msj = "No se han encontrado datos para retornar";
                    res.Message.Add(msj);
                    return res;
                }
                
                List<DefaultPlanBODto> defaultPlans = new();
                foreach(DefaultPlan c in defaultPlan)
                {
                    var cty = _mapper.Map<DefaultPlanBODto>(c);
                    cty.NoNulls();
                    defaultPlans.Add(cty);
                }
               
                res.Obj = defaultPlans;
                res.CodStatus = HttpStatusCode.OK;
                res.Success = true;
                var msj1 = "Ok";
                res.Message.Add(msj1);
                return res;

            }
        }
    }
}

