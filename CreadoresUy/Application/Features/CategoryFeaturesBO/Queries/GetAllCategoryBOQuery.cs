using Application.Interface;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Share.Dtos;
using Share.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.CategoryFeaturesBO.Queries
{
    public class GetAllCategoryBOQuery : IRequest<Response<List<CategoryBODto>>>
    {

        public class GetAllCategoryBOQueryHandler : IRequestHandler<GetAllCategoryBOQuery, Response<List<CategoryBODto>>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;
            public GetAllCategoryBOQueryHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<List<CategoryBODto>>> Handle(GetAllCategoryBOQuery query, CancellationToken cancellationToken)
            {
                Response<List<CategoryBODto>> res = new();
                res.Message = new List<string>();


                List<Category> category = await _context.Categorys.ToListAsync();
                        //.Where(u => u.Deleted.Equals(false)) --En caso de no querer listar los eliminados logicamente
                if (category == null)
                {
                    res.Obj = default;
                    res.CodStatus = HttpStatusCode.BadRequest;
                    res.Success = false;
                    var msj = "No se han encontrado datos para retornar";
                    res.Message.Add(msj);
                    return res;
                }
                
                List<CategoryBODto> categorys = new();
                foreach(Category c in category)
                {
                    var cty = _mapper.Map<CategoryBODto>(c);
                    cty.NoNulls();
                    categorys.Add(cty);
                }
               
                res.Obj = categorys;
                res.CodStatus = HttpStatusCode.OK;
                res.Success = true;
                var msj1 = "Ok";
                res.Message.Add(msj1);
                return res;

            }
        }
    }
}

