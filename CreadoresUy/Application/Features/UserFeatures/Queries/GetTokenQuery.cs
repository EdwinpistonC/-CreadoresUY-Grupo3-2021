using Application.Interface;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Share.Dtos;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Models;

namespace Application.Features.UserFeatures.Queries
{

    public class GetTokenQuery : IRequest<string>
    {
        public LoginDto User { get; set; }

        public class GetTokenQueryHandler : IRequestHandler<GetTokenQuery, string>
        {

            private readonly ICreadoresUyDbContext _context;

            private readonly IMapper _mapper;

            public GetTokenQueryHandler(ICreadoresUyDbContext context, IMapper imapper)
            {
                _context = context;
                _mapper = imapper;
            }
            public async Task<string> Handle(GetTokenQuery query, CancellationToken cancellationToken)
            {

                var u = _mapper.Map<User>(query.User);
                var user = await _context.Users.Where(x => (x.Email == u.Email && x.Password == u.Password)).FirstOrDefaultAsync();

                Response<AuthenticateResponse> res = new Response<AuthenticateResponse>();

                if (user == null)
                {
                    return "Contraseña o email erroneos";

                }
                var token = _context.GenerateJWT(user);


                return token;
            }


        }
    }
}
