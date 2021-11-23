using Application.Interface;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Share;
using Share.Dtos;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.CreatorFeatures.Queries
{
    public class GetCreatorBySearchQuery : IRequest<Response<List<CreatorProfileDto>>>
    {
        public string SearchText {  get; set; }
        public int SizePage { get; set; }
        public int Page { get; set; }
        public class GetCreatorBySearchQueryHandler : IRequestHandler<GetCreatorBySearchQuery, Response<List<CreatorProfileDto>>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public GetCreatorBySearchQueryHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;

            }
            public async Task<Response<List<CreatorProfileDto>>> Handle(GetCreatorBySearchQuery query, CancellationToken cancellationToken)
            {
                var creatorList = await _context.Creators.Where(c => c.NickName.Contains(query.SearchText) || c.ContentDescription.Contains(query.SearchText)).Include(c =>  c.Plans).ThenInclude(c=>c.UserPlans).Skip(query.Page * query.SizePage).Take(query.SizePage).ToListAsync();



                List<CreatorProfileDto> list = new List<CreatorProfileDto>();


                var subs=0;
                creatorList.ForEach(x => {
                    CreatorProfileDto creatorDataBaseDto = _mapper.Map<CreatorProfileDto>(x);
                    foreach (var p in x.Plans)
                    {
                        subs+= p.UserPlans.Count();
                    }
                    creatorDataBaseDto.CantSubscriptores = subs;
                    creatorDataBaseDto.CantSeguidores = x.Followers;
                    creatorDataBaseDto.FixIfIsNull();
                    creatorDataBaseDto.Plans = new List<PlanDto>();
                    list.Add(creatorDataBaseDto); 
                });





                Response<List<CreatorProfileDto>> res = new Response<List<CreatorProfileDto>>
                {
                    Message = new List<String>
                    {
                        "Lista de Creadores"
                    },
                    Success = true,
                    CodStatus = System.Net.HttpStatusCode.OK,
                    Obj = list
                };

                return res;
            }
        }
    }
}

