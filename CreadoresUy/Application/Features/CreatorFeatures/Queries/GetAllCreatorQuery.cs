using Application.Interface;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Share;
using Share.Dtos;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.CreatorFeatures.Queries
{
    public class GetAllCreatorQuery : IRequest<Response<List<CreadorDatabaseDto>>>
    {
        public class GetAllCreatorQueryHandler : IRequestHandler<GetAllCreatorQuery, Response<List<CreadorDatabaseDto>>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public GetAllCreatorQueryHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;

            }
            public async Task<Response<List<CreadorDatabaseDto>>> Handle(GetAllCreatorQuery query, CancellationToken cancellationToken)
            {
                var creatorList = await _context.Creators.ToListAsync();


                List<CreadorDatabaseDto> list = new List<CreadorDatabaseDto>();



                creatorList.ForEach(x => {
                    CreadorDatabaseDto creatorDataBaseDto = _mapper.Map<CreadorDatabaseDto>(x);
                    creatorDataBaseDto.FixIfIsNull();

                    list.Add(creatorDataBaseDto); 
                });

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

