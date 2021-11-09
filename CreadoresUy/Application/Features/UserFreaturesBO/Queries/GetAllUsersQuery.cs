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

namespace Application.Features.UserFreaturesBO.Queries
{
    public class GetAllUsersQuery : IRequest<Response<List<UserDto>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Response<List<UserDto>>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;
            public GetAllUsersQueryHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<List<UserDto>>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
            {
                Response<List<UserDto>> res = new();
                res.Message = new List<string>();
                var reqPage = new RequestPageUser(query.PageNumber, query.PageSize);
                var skip = (reqPage.PageNumber - 1) * reqPage.PageSize;
                List<User> usrs1 = await _context.Users.Skip(skip).Take(reqPage.PageSize).ToListAsync();
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
                
                List<UserDto> usuarios = new();
                foreach(User u in usrs1)
                {
                    var usr = _mapper.Map<UserDto>(u);
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

