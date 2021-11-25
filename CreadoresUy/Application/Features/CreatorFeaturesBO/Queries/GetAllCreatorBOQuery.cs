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

namespace Application.Features.CreatorFeaturesBO.Queries
{
    public class GetAllCreatorBOQuery : IRequest<Response<List<CreatorBODto>>>
    {

        public class GetAllCreatorBOQueryHandler : IRequestHandler<GetAllCreatorBOQuery, Response<List<CreatorBODto>>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;
            public GetAllCreatorBOQueryHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<List<CreatorBODto>>> Handle(GetAllCreatorBOQuery query, CancellationToken cancellationToken)
            {
                Response<List<CreatorBODto>> res = new();
                res.Message = new List<string>();


                List<Creator> usrs1 = await _context.Creators.ToListAsync();
                        //.Where(u => u.Deleted.Equals(false)) --En caso de no querer listar los eliminados logicamente
                if (usrs1 == null)
                {
                    res.Obj = default;
                    res.CodStatus = HttpStatusCode.BadRequest;
                    res.Success = false;
                    var msj = "No se han encontrado datos para retornar";
                    res.Message.Add(msj);
                    return res;
                }
                
                List<CreatorBODto> usuarios = new();
                foreach(Creator u in usrs1)
                {
                    var usr = _mapper.Map<CreatorBODto>(u);
                    usr.NoNulls();
                    usuarios.Add(usr);
                }
               
                res.Obj = usuarios;
                res.CodStatus = HttpStatusCode.OK;
                res.Success = true;
                var msj1 = "Ok";
                res.Message.Add(msj1);
                return res;

            }
        }
    }
}

