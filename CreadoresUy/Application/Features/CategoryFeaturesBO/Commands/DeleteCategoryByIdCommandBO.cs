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

namespace Application.Features.CategoryFeaturesBO.Commands
{
    public class DeleteCategoryByIdCommandBO : IRequest<Response<String>>
    {
        public int Id {  get; set; }

        public class DeleteCategoryByIdCommandBOHandler : IRequestHandler<DeleteCategoryByIdCommandBO, Response<String>>
        {
            private readonly ICreadoresUyDbContext _context;
            public DeleteCategoryByIdCommandBOHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }
            public async Task<Response<String>> Handle(DeleteCategoryByIdCommandBO command, CancellationToken cancellationToken)
            {
                var category = _context.Categorys.Where(u => u.Id == command.Id).FirstOrDefault();
                Response<string> res = new();
                res.Message = new List<string>();
                if(category == null )
                {
                    res.Obj = "";
                    res.CodStatus = HttpStatusCode.BadRequest;
                    res.Success = false;
                    var msj = "Id no encontrado ";
                    res.Message.Add(msj);
                    return res;
                }
                category.Deleted = !category.Deleted;
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
