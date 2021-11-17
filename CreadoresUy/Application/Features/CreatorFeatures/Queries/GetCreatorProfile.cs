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
        public string Nickname {  get; set; }

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
                var cre = _context.Creators.Where(c => c.NickName == query.Nickname)
                            .Include(c => c.Plans).FirstOrDefault();
                var dtocre = new CreatorProfileDto();
                if (cre != null) { 
                    dtocre.CreatorName = cre.CreatorName;
                    dtocre.CreatorImage = cre.CreatorImage;
                    dtocre.CoverImage = cre.CoverImage;
                    dtocre.CantSeguidores = cre.Followers;
                    dtocre.Biography = cre.Biography;
                    dtocre.ContentDescription = cre.ContentDescription;
                    dtocre.YoutubeLink = cre.YoutubeLink;
                    int cantSubs = 0;
                    foreach(var pl in cre.Plans)
                    {
                        var plan = _context.Plans.Where(p => p.Id == pl.Id)
                        .Include(p => p.UserPlans).Include(p=> p.ContentPlans).FirstOrDefault();
                        cantSubs += plan.UserPlans.Count;
                    }
                    dtocre.CantSubscriptores = cantSubs;
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
                    dtocre.FixIsNull();
                    resp.Obj = dtocre;
                    resp.CodStatus = HttpStatusCode.BadRequest;
                    resp.Success = false;
                    resp.Message = new List<string>
                    {
                        "No se ha encontrado al creador ingresado"
                    };
                }
                return resp;
            }

        }
    }
}
