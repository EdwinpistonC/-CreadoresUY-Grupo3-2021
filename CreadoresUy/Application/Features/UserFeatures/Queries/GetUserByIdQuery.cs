using Application.Interface;
using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Share;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Share.Dtos;

namespace Application.Features.UserFeatures.Queries
{

    public class GetUserById : IRequest<UserDto>
    {
        public int Id { get; set; }
        public class GetUserByIdHandler : IRequestHandler<GetUserById, UserDto>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public GetUserByIdHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<UserDto> Handle(GetUserById query, CancellationToken cancellationToken)
            {
                var user = await _context.Users.Where(a => a.Id == query.Id).FirstOrDefaultAsync();
                if (user.CreatorId != null)
                {
                    user.Creator = _context.Creators.Where(a => a.Id == user.CreatorId).FirstOrDefault();
                }

                if (user == null) return null;
                var dto = _mapper.Map<UserDto>(user);
                dto.FixIfIsNull();
                return dto;
            }
        }
    }
}
