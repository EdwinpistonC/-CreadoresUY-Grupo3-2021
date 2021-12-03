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

namespace Application.Features.DefaultPlanFeaturesBO.Commands
{
    public class CreateDefaultPlanCommandBO : IRequest<Response<String>>
    {
        public DefaultPlanBODto CreatePlanDto { get; set; }
        public class CreateDefaultPlanCommandBOHandler : IRequestHandler<CreateDefaultPlanCommandBO, Response<String>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public CreateDefaultPlanCommandBOHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<String>> Handle(CreateDefaultPlanCommandBO command, CancellationToken cancellationToken)
            {
                var dto = command.CreatePlanDto;

                Response<string> res = new Response<String>
                {
                    Obj = "",
                    Message = new List<String>()
                };






                var Plan = new DefaultPlan();
                Plan= _mapper.Map<DefaultPlan>(dto);
                Console.WriteLine(command.CreatePlanDto);


                _context.DefaultPlans.Add(Plan);
                await _context.SaveChangesAsync();
                res.CodStatus = HttpStatusCode.Created;
                res.Success = true;
                var msg1 = "Plan ingresado correctamente";
                res.Message.Add(msg1);
                return res;
            }
        }
    }
}

