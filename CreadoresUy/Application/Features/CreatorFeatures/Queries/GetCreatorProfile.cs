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
    public class GetCreatorProfile : IRequest<Response<CreatorProfileDto>>
    {
        public int IdCreator {  get; set; }

        public class GetCreatorProfileHandle : IRequestHandler<GetCreatorProfile, Response<CreatorProfileDto>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public GetCreatorProfileHandle(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<CreatorProfileDto>> Handle(GetCreatorProfile query, CancellationToken cancellation)
            {
                Response<CreatorProfileDto> resp = new();
                var cre = _context.Creators.Where(c => c.Id == query.IdCreator)
                            .Include(c => c.Plans).FirstOrDefault();
                var dtocre = new CreatorProfileDto();
                if (cre != null) { 
                    dtocre.CreatorName = cre.CreatorName;
                    dtocre.CreatorImage = cre.CreatorImage;
                    dtocre.CoverImage = cre.CoverImage;
                    dtocre.CantSeguidores = cre.Followers;
                    int cantSubs = 0;
                    List<ContentDto> contens = new();
                    foreach(var pl in cre.Plans)
                    {
                        var plan = _context.Plans.Where(p => p.Id == pl.Id)
                        .Include(p => p.UserPlans).Include(p=> p.ContentPlans).FirstOrDefault();
                        foreach(var contp in plan.ContentPlans)
                        {
                            var content = _context.Contents.Where(c => c.Id == contp.IdContent).FirstOrDefault();
                            var dto = _mapper.Map<ContentDto>(content);
                            dto.NoNulls();
                            dto.ReduceContent();
                            contens.Add(dto);
                        }
                        cantSubs += plan.UserPlans.Count;
                        
                    }
                    dtocre.CantSubscriptores = cantSubs;
                    dtocre.Contens = contens;
                    dtocre.FixIsNull();
                    resp.Obj = dtocre;
                    resp.CodStatus = HttpStatusCode.OK;
                    resp.Success = true;
                    resp.Message = new List<string>
                    {
                        "Exito"
                    };
                }
                else
                {
                    resp.Obj = dtocre;
                    resp.CodStatus = HttpStatusCode.BadRequest;
                    resp.Success = false;
                    resp.Message.Add("No se ha encontrado al creador ingresado");
                }
                return resp;
            }

        }
    }
}
