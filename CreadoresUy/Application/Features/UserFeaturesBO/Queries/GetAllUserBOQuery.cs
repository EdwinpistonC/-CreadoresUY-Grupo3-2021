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

namespace Application.Features.UserFeaturesBO.Queries
{
    public class GetAllUsersBOQuery : IRequest<Response<List<UserBODto>>>
    {

        public class GetAllUsersBOQueryQueryHandler : IRequestHandler<GetAllUsersBOQuery, Response<List<UserBODto>>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;
            public GetAllUsersBOQueryQueryHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<List<UserBODto>>> Handle(GetAllUsersBOQuery query, CancellationToken cancellationToken)
            {
                Response<List<UserBODto>> res = new();
                res.Message = new List<string>();


                List<User> usrs1 = await _context.Users.ToListAsync();
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
                
                List<UserBODto> usuarios = new();
                foreach(User u in usrs1)
                {
                    var usr = _mapper.Map<UserBODto>(u);
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

