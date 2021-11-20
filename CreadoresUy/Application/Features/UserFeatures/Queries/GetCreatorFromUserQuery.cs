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

namespace Application.Features.UserFeatures.Queries
{
    public class GetCreatorFromUserQuery : IRequest<Response<List<CreadorDatabaseDto>>>
    {
        public int IdUser {  get; set; }
        public class GetCreatorFromUserQueryHandler : IRequestHandler<GetCreatorFromUserQuery, Response<List<CreadorDatabaseDto>>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public GetCreatorFromUserQueryHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;

            }
            public async Task<Response<List<CreadorDatabaseDto>>> Handle(GetCreatorFromUserQuery query, CancellationToken cancellationToken)
            {

                var creatorList = await
                    (from creador in _context.Creators
                    join plan in _context.Plans on creador equals plan.Creator
                    join userPlan in _context.UserPlans on plan equals userPlan.Plan
                    where userPlan.IdUser == query.IdUser
                    select creador).ToListAsync();


                List<CreadorDatabaseDto> list = new List<CreadorDatabaseDto>();


                foreach(Creator cr in creatorList)
                {
                    CreadorDatabaseDto creatorDataBaseDto = _mapper.Map<CreadorDatabaseDto>(cr);
                    creatorDataBaseDto.FixIfIsNull();
                    list.Add(creatorDataBaseDto);
                }


                Response<List<CreadorDatabaseDto>> res = new Response<List<CreadorDatabaseDto>>
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

