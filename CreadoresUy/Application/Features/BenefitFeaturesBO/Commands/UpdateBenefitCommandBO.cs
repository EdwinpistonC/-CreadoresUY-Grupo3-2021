using Application.Interface;
using AutoMapper;
using MediatR;
using Share.Dtos;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.BenefitFeaturesBO.Commands
{
    public class UpdateBenefitCommandBO : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public class UpdateBenefitCommandBOHandler : IRequestHandler<UpdateBenefitCommandBO, Response<string>>
        {
            private readonly ICreadoresUyDbContext _context;

            public UpdateBenefitCommandBOHandler(ICreadoresUyDbContext context )
            {
                _context = context;
            }
            public async Task<Response<string>> Handle(UpdateBenefitCommandBO command, CancellationToken cancellationToken)
            {
                var benefit = _context.DefaultBenefits.Where(c => c.Id == command.Id).FirstOrDefault();
                Response<string> res = new()
                {
                    Message = new List<string>(),
                    Obj = ""
                };
                if (benefit == null )
                {
                    res.Message.Add("No se ha encontrado la categoría de id: " + command.Id);
                    res.Success = false;
                    res.CodStatus = HttpStatusCode.BadRequest;
                    return res;
                }

                if (command.Description != "") benefit.Description = command.Description;

                await _context.SaveChangesAsync();
                res.Message.Add("Exito");
                res.Success = true;
                res.CodStatus = HttpStatusCode.OK;
                return res;
            }

        }
        
    }
   
}
