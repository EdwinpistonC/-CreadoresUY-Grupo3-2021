using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Share.Dtos;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.CreatorFeatures.Queries
{
    public class GetBenefitCantQuery : IRequest<Response<List<BenefitCantDto>>>
    {
        public string Nickname {  get; set; }
        public class GetBenefitCantQueryHandler : IRequestHandler<GetBenefitCantQuery, Response<List<BenefitCantDto>>>
        {
            private readonly ICreadoresUyDbContext _context;

            public GetBenefitCantQueryHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }

            public async Task<Response<List<BenefitCantDto>>> Handle(GetBenefitCantQuery request, CancellationToken cancellationToken)
            {
                Response<List<BenefitCantDto>> res = new();
                res.Message = new List<string>();
                var listaBen = new List<BenefitCantDto>();
                var cre = _context.Creators.Where(c => c.NickName == request.Nickname)
                    .Include(c => c.Plans).ThenInclude(p => p.Benefits).FirstOrDefault();
                var list = new List<Benefit>();
                if (cre == null)
                {
                    res.Success = false;
                    res.CodStatus = HttpStatusCode.BadRequest;
                    res.Message.Add("No se ha encontrado al creador ingresado");
                    res.Obj = listaBen;
                }
                else
                {
                    var aux = new List<string>();
                    foreach (var plan in cre.Plans)
                    {
                        foreach (var b in plan.Benefits)
                        {
                            list.Add(b);
                        }
                    }

                    foreach (var b in list)
                    {
                        int x = 0;
                        foreach (var ben in list)
                        {
                            if (b.Description.ToLower() == ben.Description.ToLower() && !aux.Contains(b.Description.ToLower()))
                            {
                                x++;
                            }
                        }
                        if (x > 0)
                        {
                            var dto = new BenefitCantDto(b.Description, x);
                            aux.Add(b.Description.ToLower());
                            listaBen.Add(dto);
                        }
                    }
                    res.Success = true;
                    res.CodStatus = HttpStatusCode.OK;
                    if(listaBen.Count == 0) res.Message.Add("No se ha encontrado beneficios para listar");
                    if (listaBen.Count > 0) res.Message.Add("Exito ");
                    res.Obj = listaBen;
                }

                return res;
            }
        }
    }
}
