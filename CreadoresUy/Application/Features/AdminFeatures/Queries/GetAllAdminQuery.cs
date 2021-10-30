using Application.Interface;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Share.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AdminFeatures.Queries
{
    public class GetAllAdminQuery : IRequest<Response<List<AdminDto>>>
    {
        public class GetAllAdminQueryHandler : IRequestHandler<GetAllAdminQuery, Response<List<AdminDto>>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public GetAllAdminQueryHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;

            }
            public async Task<Response<List<AdminDto>>> Handle(GetAllAdminQuery query, CancellationToken cancellationToken)
            {
                var userList = await _context.Users.Where(x=> x.IsAdmin == true).ToListAsync();


                List<AdminDto> list = new List<AdminDto>();



                userList.ForEach(x => list.Add(_mapper.Map<AdminDto>(x)));

                Response<List<AdminDto>> res = new Response<List<AdminDto>>
                {
                    Message = new List<String>
                    {
                        "Lista de administradores"
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
