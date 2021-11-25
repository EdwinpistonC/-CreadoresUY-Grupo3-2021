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

namespace Application.Features.CreatorFeaturesBO.Commands
{
    public class CreateAdminCommandBO : IRequest<Response<String>>
    {
        public AdminBODto CreateAdminDto { get; set; }
        public class CreateAdminCommandBOHandler : IRequestHandler<CreateAdminCommandBO, Response<String>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public CreateAdminCommandBOHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<String>> Handle(CreateAdminCommandBO command, CancellationToken cancellationToken)
            {
                var dto = command.CreateAdminDto;

                Response<string> res = new Response<String>
                {
                    Obj = "",
                    Message = new List<String>()
                };
                var validator = new AdminSignUpValidatorBO(_context);
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

                var user = _mapper.Map<Creator>(dto);
                user.CreatorCreated = DateTime.Now;
                _context.Creators.Add(user);
                await _context.SaveChangesAsync();
                res.CodStatus = HttpStatusCode.Created;
                res.Success = true;
                var msg1 = "Creator ingresado correctamente";
                res.Message.Add(msg1);
                return res;
            }
        }
    }
}

