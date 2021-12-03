using Application.Interface;
using AutoMapper;
using MediatR;
using Share.Dtos;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DefaultPlanFeaturesBO.Queries
{
    public class GetDefaultPlanByIdBOQuery : IRequest<Response<DefaultPlanBODto>>
    {
        public int Id {  get; set; }

        public class GetDefaultPlanByIdBOQueryHandler : IRequestHandler<GetDefaultPlanByIdBOQuery, Response<DefaultPlanBODto>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public GetDefaultPlanByIdBOQueryHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<DefaultPlanBODto>> Handle(GetDefaultPlanByIdBOQuery query, CancellationToken cancellationToken)
            {
                var defaultPlan = _context.DefaultPlans.Where(c => c.Id == query.Id).FirstOrDefault();
                Response<DefaultPlanBODto> res = new();
                res.Message = new List<string>();
                //if (user == null || user.Deleted == true)
                if (defaultPlan == null)
                {
                    res.Obj = default;
                    res.CodStatus = HttpStatusCode.BadRequest;
                    res.Success = false;
                    var msj = "No se ha encontrado una categoria asociado al id ingresado";
                    res.Message.Add(msj);
                    return res;
                }
                var dto = _mapper.Map<DefaultPlanBODto>(defaultPlan);
                dto.NoNulls();
                res.Obj = dto;
                res.CodStatus = HttpStatusCode.OK;
                res.Success = true;
                var msj1 = "Ok";
                res.Message.Add(msj1);
                return res;

            }

        }
    }
}
