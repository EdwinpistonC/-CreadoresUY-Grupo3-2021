using Application.Features.Validators;
using Application.Interface;
using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Share.Dtos;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.BenefitFeaturesBO.Commands
{
    public class CreateBenefitCommandBO : IRequest<Response<String>>
    {
        public string Description { get; set; }
        public class CreateBenefitCommandBOHandler : IRequestHandler<CreateBenefitCommandBO, Response<String>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public CreateBenefitCommandBOHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<String>> Handle(CreateBenefitCommandBO command, CancellationToken cancellationToken)
            {
                var findCategory = _context.DefaultBenefits.Where(c => c.Description==command.Description).FirstOrDefault();
                Response<string> res = new Response<String>
                {
                    Obj = "",
                    Message = new List<String>()
                };
                if (findCategory!=null)
                {
                    res.Obj = "";
                    res.CodStatus = HttpStatusCode.BadRequest;
                    res.Success = false;
                    var msj = "Beneficio ya existe";
                    res.Message.Add(msj);
                    return res;
                }



                DefaultBenefit beneficio = new DefaultBenefit();
                beneficio.Description = command.Description;
                _context.DefaultBenefits.Add(beneficio);
                await _context.SaveChangesAsync();
                res.CodStatus = HttpStatusCode.Created;
                res.Success = true;
                var msg1 = "Category ingresado correctamente";
                res.Message.Add(msg1);
                return res;
            }
        }
    }
}

