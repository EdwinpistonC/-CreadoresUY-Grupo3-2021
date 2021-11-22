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
                Response<CreatorProfileDto> resp = new Response<CreatorProfileDto>();
                var cre = _context.Creators.Where(c => c.NickName == query.Nickname)
                            .Include(c => c.Plans).ThenInclude(b => b.Benefits).Include(c => c.Plans).ThenInclude(up => up.UserPlans).FirstOrDefault();
                var dtocre = new CreatorProfileDto();
                int cantSubs = 0;
                List<PlanDto> plans = new List<PlanDto>();
                if (cre != null)
                {
                    dtocre.CreatorName = cre.CreatorName;
                    dtocre.CreatorImage = cre.CreatorImage;
                    dtocre.CoverImage = cre.CoverImage;
                    dtocre.CantSeguidores = cre.Followers;
                    dtocre.Plans = new List<PlanDto>();
                    //List<ContentDto> contens = new();

                    foreach (var pl in cre.Plans)
                    {
                        PlanDto plDto = _mapper.Map<PlanDto>(pl);

                        plDto.Benefits = new List<BenefitDTO>();

                        foreach (var b in pl.Benefits)
                        {
                            plDto.Benefits.Add(_mapper.Map<BenefitDTO>(b));
                        }

                        plDto.FixIfIsNull();
                        dtocre.Plans.Add(plDto);



                        /*foreach(var contp in plan.ContentPlans)
                        {
                            var content = _context.Contents.Where(c => c.Id == contp.IdContent).FirstOrDefault();
                            var dto = _mapper.Map<ContentDto>(content);
                            dto.NoNulls();
                            dto.ReduceContent();
                            contens.Add(dto);
                        }
                        */
                        cantSubs += pl.UserPlans.Count;

                    }


                    dtocre.CantSubscriptores = cantSubs;
                    dtocre.FixIfIsNull();
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
