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
    public class GetFeedQuery : IRequest<Response<List<ContentDto>>>
    {
        public int IdUser {  get; set; }
        public class GetFeedQueryHandler : IRequestHandler<GetFeedQuery, Response<List<ContentDto>>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public GetFeedQueryHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;

            }
            public async Task<Response<List<ContentDto>>> Handle(GetFeedQuery query, CancellationToken cancellationToken)
            {
                var idPlans = await _context.UserPlans.Where(up => up.IdUser == query.IdUser).ToListAsync();

                var listPlans = new List<int>();

                foreach (var idPlan in idPlans) {

                    listPlans.Add(idPlan.IdPlan);
                }


                var content = await _context.Contents.Where(c => c.ContentPlans.Any(cp=>listPlans.Contains(cp.IdPlan))).ToListAsync();


                List<ContentDto> list = new List<ContentDto>();


                content.ForEach(x => {
                    ContentDto contentDataBaseDto = _mapper.Map<ContentDto>(x);
                    list.Add(contentDataBaseDto); 
                });

                Response<List<ContentDto>> res = new Response<List<ContentDto>>
                {
                    Message = new List<String>
                    {
                        "Lista De Feed"
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

