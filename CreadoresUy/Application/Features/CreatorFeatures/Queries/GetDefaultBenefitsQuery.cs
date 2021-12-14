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

namespace Application.Features.CreatorFeatures.Queries
{
    public class GetDefaultBenefitsQuery : IRequest<Response<List<string>>>
    {

        public class GetDefaultBenefitsQueryHandler : IRequestHandler<GetDefaultBenefitsQuery, Response<List<string>>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;
            public GetDefaultBenefitsQueryHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<List<string>>> Handle(GetDefaultBenefitsQuery query, CancellationToken cancellationToken)
            {
                Response<List<string>> res = new();
                res.Message = new List<string>();


                List<DefaultBenefit> benefit = await _context.DefaultBenefits.Where(u => u.Deleted == false).ToListAsync();
                        
                if (benefit == null)
                {
                    res.Obj = default;
                    res.CodStatus = HttpStatusCode.BadRequest;
                    res.Success = false;
                    var msj = "No se han encontrado datos para retornar";
                    res.Message.Add(msj);
                    return res;
                }
                
                List<string> benefits = new();
                foreach(DefaultBenefit c in benefit)
                {
                    benefits.Add(c.Description);
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

