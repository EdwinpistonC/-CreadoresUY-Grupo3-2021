using Application.Features.Validators;
using Application.Interface;
using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Share;
using Share.Dtos;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AdminFeatures.Commands
{
    public class CreateAdminCommand : IRequest<Response<String>>
    {
        public CreateUserDto CreateUserDto { get; set; }

        public class CreateAdminCommandHandler : IRequestHandler<CreateAdminCommand, Response<String>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public CreateAdminCommandHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Response<String>> Handle(CreateAdminCommand command, CancellationToken cancellationToken)
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
                user.IsAdmin = true;
                Console.WriteLine(user.IsAdmin);
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                res.Success = true;
                res.CodStatus = HttpStatusCode.OK;
                var msg1 = "Admin ingresado correctamente";
                res.Message.Add(msg1);
                return res;
            }
        }

    }
}
