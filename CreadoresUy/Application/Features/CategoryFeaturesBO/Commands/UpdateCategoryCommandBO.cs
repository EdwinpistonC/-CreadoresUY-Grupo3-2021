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

namespace Application.Features.CategoryFeaturesBO.Commands
{
    public class UpdateCategoryCommandBO : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public class UpdateCategoryCommandBOHandler : IRequestHandler<UpdateCategoryCommandBO, Response<string>>
        {
            private readonly ICreadoresUyDbContext _context;

            public UpdateCategoryCommandBOHandler(ICreadoresUyDbContext context )
            {
                _context = context;
            }
            public async Task<Response<string>> Handle(UpdateCategoryCommandBO command, CancellationToken cancellationToken)
            {
                var category = _context.Categorys.Where(c => c.Id == command.Id).FirstOrDefault();
                Response<string> res = new()
                {
                    Message = new List<string>(),
                    Obj = ""
                };
                if (category == null )
                {
                    res.Message.Add("No se ha encontrado la categoría de id: " + command.Id);
                    res.Success = false;
                    res.CodStatus = HttpStatusCode.BadRequest;
                    return res;
                }

                if (command.Name != "") category.Name = command.Name;

                await _context.SaveChangesAsync();
                res.Message.Add("Exito");
                res.Success = true;
                res.CodStatus = HttpStatusCode.OK;
                return res;
            }

        }
        
    }
   
}
