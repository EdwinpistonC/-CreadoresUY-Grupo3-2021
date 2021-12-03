using Application.Interface;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Share;
using Share.Dtos;
using Share.Entities;
using Share.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.CreatorFeatures.Queries
{
    public class GetCreatorBySearchQuery : IRequest<Response<List<CreadorSearchDto>>>
    {
        public string SearchText {  get; set; }
        public int SizePage { get; set; }
        public int Page { get; set; }
        public class GetCreatorBySearchQueryHandler : IRequestHandler<GetCreatorBySearchQuery, Response<List<CreadorSearchDto>>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public GetCreatorBySearchQueryHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Response<List<CreadorSearchDto>>> Handle(GetCreatorBySearchQuery query, CancellationToken cancellationToken)
            {

                var creatorList = await _context.Creators.Where(c => c.NickName.Contains(query.SearchText) || c.CreatorName.Contains(query.SearchText)||
                 c.ContentDescription.Contains(query.SearchText) && c.Deleted==false).Include(c =>  c.Plans).ThenInclude(c=>c.UserPlans).Skip(query.Page * query.SizePage).Take(query.SizePage).ToListAsync();

                List<CreadorSearchDto> list = new List<CreadorSearchDto>();

                var subs=0;
                creatorList.ForEach(x => {
                    subs = 0;
                    CreadorSearchDto creatorDataBaseDto = _mapper.Map<CreadorSearchDto>(x);
                    foreach (Plan p in x.Plans)
                    {
                        subs += p.UserPlans.Count();
                    }


                    creatorDataBaseDto.CantSubscriptores = subs;
                    creatorDataBaseDto.CantSeguidores = x.Followers;
                    creatorDataBaseDto.FixIfIsNull();
                    creatorDataBaseDto.Categorys = new List<string>();
                    if(x.Category1 != null && x.Category1 != "") creatorDataBaseDto.Categorys.Add(x.Category1.ToString());
                    if (x.Category2 != null && x.Category2 != "" && x.Category1 != x.Category2) creatorDataBaseDto.Categorys.Add(x.Category2.ToString());
                    list.Add(creatorDataBaseDto); 
                });





                Response<List<CreadorSearchDto>> res = new Response<List<CreadorSearchDto>>
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

