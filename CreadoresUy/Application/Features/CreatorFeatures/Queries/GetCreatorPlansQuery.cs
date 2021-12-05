using Application.Interface;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
    public class GetCreatorPlansQuery : IRequest<Response<List<BasicPlanDto>>>
    {
        public string Nickname {  get; set; }
        public class GetCreatorPlansQueryHandler : IRequestHandler<GetCreatorPlansQuery, Response<List<BasicPlanDto>>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public GetCreatorPlansQueryHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<List<BasicPlanDto>>> Handle(GetCreatorPlansQuery request, CancellationToken cancellationToken)
            {
                var resp = new Response<List<BasicPlanDto>>()
                {
                    Message = new List<string> { }
                };
                var list = new List<BasicPlanDto>();
                var cre = _context.Creators.Where(c => c.NickName == request.Nickname).Include(c => c.Plans).FirstOrDefault();
                if (cre == null || cre.Plans.Count == 0)
                {
                    if (cre == null) resp.Message.Add("No se ha encontrado el nickname ingresado");
                    if(cre.Plans.Count == 0) resp.Message.Add("No se han encontrado planes relacionados al nickname ingresado");
                    resp.Success = false;
                    resp.CodStatus = HttpStatusCode.BadRequest;
                    resp.Obj = list;
                }
                else
                {
                    foreach (var p in cre.Plans)
                    {
                        var aux = _mapper.Map<BasicPlanDto>(p);
                        list.Add(aux);
                    }
                    resp.Success = true;
                    resp.CodStatus = HttpStatusCode.OK;
                    resp.Obj = list;
                }
                return resp;
            }
        }
    }
}
