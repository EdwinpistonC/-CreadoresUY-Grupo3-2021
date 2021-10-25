using Application.Interface;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Share;
using Share.Dtos;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Models;

namespace Application.Features.UserFeatures.Queries
{
    public class GetLogingUserQuery : IRequest<Response<AuthenticateResponse>>
    {
        public LoginDto User {  get; set; }

        public class GetLogingUserHandler : IRequestHandler<GetLogingUserQuery, Response<AuthenticateResponse>>
        {

            private readonly ICreadoresUyDbContext _context;

            private readonly IMapper _mapper;

            public GetLogingUserHandler(ICreadoresUyDbContext context,IMapper imapper)
            {
                _context = context;
                _mapper = imapper;
            }
            public async Task<Response<AuthenticateResponse>> Handle(GetLogingUserQuery query, CancellationToken cancellationToken)
            {

                var u= _mapper.Map<User>(query.User);
                var user = await _context.Users.Where(x =>  (x.Email == u.Email  &&  x.Password == u.Password)).FirstOrDefaultAsync();

                Response<AuthenticateResponse> res = new Response<AuthenticateResponse>();

                if (user == null) {
                    res.CodStatus = HttpStatusCode.BadRequest;
                    res.Success = false;
                    res.Message.Add("Contraseña o email erroneos");
                    return res;

                }
                var token = _context.GenerateJWT(user);

                res.CodStatus = HttpStatusCode.OK;
                res.Success = true;
                res.Obj = new AuthenticateResponse(user, token); ;


                return res;
            }
            

        }
    }

}
