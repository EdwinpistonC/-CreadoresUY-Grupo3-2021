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

namespace Application.Features.CreatorFeatures.Queries
{
    public class GetCategoryes : IRequest<Response<List<String>>>
    {
        public class GetCategoryesHandler : IRequestHandler<GetCategoryes, Response<List<String>>>
        {
            private readonly ICreadoresUyDbContext _context;
            public GetCategoryesHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }

            public async Task<Response<List<string>>> Handle(GetCategoryes query, CancellationToken cancellationToken)
            {
                var res = new Response<List<string>>
                {
                    Message = new List<string>()
                };
                var listaux = new List<string>();
                var listcat = _context.Categorys.ToList();
                
                if(listcat.Count == 0)
                {
                    res.Obj = listaux;
                    res.Message.Add("No se han encontrado categorias");
                    res.Success = false;
                    res.CodStatus = HttpStatusCode.BadRequest;
                    return res;
                }
                foreach (var item in listcat)
                {
                    listaux.Add(item.Name);
                }
                res.Obj = listaux;
                res.Message.Add("Exito");
                res.Success = true;
                res.CodStatus = HttpStatusCode.OK;
                return res;
            }
        }
    }
}
