using Application.Features.Validators;
using Application.Interface;
using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Share.Dtos;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserFeatures.Commands
{
    public class UserSignUpCommand : IRequest<Response<String>>
    {
        public CreateUserDto CreateUserDto { get; set; }

        public class UserSignUpCommandHandler : IRequestHandler<UserSignUpCommand, Response<String>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public UserSignUpCommandHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Response<String>> Handle(UserSignUpCommand command, CancellationToken cancellationToken)
            {
                var dto = command.CreateUserDto;

                Response<string> res = new Response<String>
                {
                    Obj = "",
                    Message = new List<String>()
                };
                var validator = new UserSignUpCommandValidator(_context);
                ValidationResult result = validator.Validate(dto);

                if (!result.IsValid)
                {
                    res.CodStatus = HttpStatusCode.BadRequest;
                    res.Success = false;
                    foreach (var error in result.Errors)
                    {
                        var msg = error.ErrorMessage;
                        res.Message.Add(msg);
                    }
                    return res;
                }

                var user = _mapper.Map<User>(dto);
                user.Created = DateTime.Now;
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                res.CodStatus = HttpStatusCode.Created;
                res.Success = true;
                var msg1 = "Usuario ingresado correctamente";
                res.Message.Add(msg1);
                return res;
            }
        }

    }
}
