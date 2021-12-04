using Application.Interface;
using MediatR;
using Share.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.BenefitFeaturesBO.Commands
{
    public class DeleteBenefitByIdCommandBO : IRequest<Response<String>>
    {
        public int Id {  get; set; }

        public class DeleteBenefitByIdCommandBOHandler : IRequestHandler<DeleteBenefitByIdCommandBO, Response<String>>
        {
            private readonly ICreadoresUyDbContext _context;
            public DeleteBenefitByIdCommandBOHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }
            public async Task<Response<String>> Handle(DeleteBenefitByIdCommandBO command, CancellationToken cancellationToken)
            {
                var benefit = _context.DefaultBenefits.Where(u => u.Id == command.Id).FirstOrDefault();
                Response<string> res = new();
                res.Message = new List<string>();
                if(benefit == null )
                {
                    res.Obj = "";
                    res.CodStatus = HttpStatusCode.BadRequest;
                    res.Success = false;
                    var msj = "Id no encontrado ";
                    res.Message.Add(msj);
                    return res;
                }
                benefit.Deleted = !benefit.Deleted;
                await _context.SaveChangesAsync();
                res.Obj = "";
                res.CodStatus = HttpStatusCode.OK;
                res.Success = true;
                res.Message.Add("Exito");
                return res;
            }
        }
    }
    
}
