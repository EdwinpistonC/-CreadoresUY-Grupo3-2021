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
    public class GetCreatorBySearchQuery : IRequest<Response<List<CreadorDatabaseDto>>>
    {
        public string SearchText {  get; set; }
        public class GetCreatorBySearchQueryHandler : IRequestHandler<GetCreatorBySearchQuery, Response<List<CreadorDatabaseDto>>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public GetCreatorBySearchQueryHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;

            }
            public async Task<Response<List<CreadorDatabaseDto>>> Handle(GetCreatorBySearchQuery query, CancellationToken cancellationToken)
            {
                var creatorList = await _context.Creators.Where(c => c.NickName.Contains(query.SearchText) || c.ContentDescription.Contains(query.SearchText)).ToListAsync();



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

