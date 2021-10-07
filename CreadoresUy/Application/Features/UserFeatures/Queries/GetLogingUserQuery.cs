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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserFeatures.Queries
{
    public class GetLogingUserQuery : IRequest<Boolean>
    {
        public LoginDto User {  get; set; }

        public class GetLogingUserHandler : IRequestHandler<GetLogingUserQuery, Boolean>
        {

            private readonly ICreadoresUyDbContext _context;

            private readonly IMapper _mapper;

            public GetLogingUserHandler(ICreadoresUyDbContext context,IMapper imapper)
            {
                _context = context;
                _mapper = imapper;
            }
            public async Task<Boolean> Handle(GetLogingUserQuery query, CancellationToken cancellationToken)
            {

                var u= _mapper.Map<User>(query.User);
                var user = await _context.Users.Where(x =>  (x.Email == u.Email  &&  x.Password == u.Password)).FirstOrDefaultAsync();
                if (user == null) return false;
                return true;
            }
        }
    }

}
