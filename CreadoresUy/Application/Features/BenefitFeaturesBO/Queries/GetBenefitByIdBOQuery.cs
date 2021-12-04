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

namespace Application.Features.BenefitFeaturesBO.Queries
{
    public class GetBenefitByIdBOQuery : IRequest<Response<BenefitBODto>>
    {
        public int Id {  get; set; }

        public class GetBenefitByIdBOQueryHandler : IRequestHandler<GetBenefitByIdBOQuery, Response<BenefitBODto>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public GetBenefitByIdBOQueryHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<BenefitBODto>> Handle(GetBenefitByIdBOQuery query, CancellationToken cancellationToken)
            {
                var benefit = _context.DefaultBenefits.Where(c => c.Id == query.Id).FirstOrDefault();
                Response<BenefitBODto> res = new();
                res.Message = new List<string>();
                //if (user == null || user.Deleted == true)
                if (benefit == null)
                {
                    res.Obj = default;
                    res.CodStatus = HttpStatusCode.BadRequest;
                    res.Success = false;
                    var msj = "No se ha encontrado una categoria asociado al id ingresado";
                    res.Message.Add(msj);
                    return res;
                }
                var dto = _mapper.Map<BenefitBODto>(benefit);
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
