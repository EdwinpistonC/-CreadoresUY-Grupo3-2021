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

namespace Application.Features.BenefitFeaturesBO.Queries
{
    public class GetAllBenefitBOQuery : IRequest<Response<List<BenefitBODto>>>
    {

        public class GetAllBenefitBOQueryHandler : IRequestHandler<GetAllBenefitBOQuery, Response<List<BenefitBODto>>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;
            public GetAllBenefitBOQueryHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<List<BenefitBODto>>> Handle(GetAllBenefitBOQuery query, CancellationToken cancellationToken)
            {
                Response<List<BenefitBODto>> res = new();
                res.Message = new List<string>();


                List<DefaultBenefit> benefit = await _context.DefaultBenefits.ToListAsync();
                        //.Where(u => u.Deleted.Equals(false)) --En caso de no querer listar los eliminados logicamente
                if (benefit == null)
                {
                    res.Obj = default;
                    res.CodStatus = HttpStatusCode.BadRequest;
                    res.Success = false;
                    var msj = "No se han encontrado datos para retornar";
                    res.Message.Add(msj);
                    return res;
                }
                
                List<BenefitBODto> benefits = new();
                foreach(DefaultBenefit c in benefit)
                {
                    var cty = _mapper.Map<BenefitBODto>(c);
                    cty.NoNulls();
                    benefits.Add(cty);
                }
               
                res.Obj = benefits;
                res.CodStatus = HttpStatusCode.OK;
                res.Success = true;
                var msj1 = "Ok";
                res.Message.Add(msj1);
                return res;

            }
        }
    }
}

