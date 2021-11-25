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

namespace Application.Features.AdminFeaturesBO.Queries
{
    public class GetAllAdminBOQuery : IRequest<Response<List<AdminBODto>>>
    {

        public class GetAllAdminBOQueryHandler : IRequestHandler<GetAllAdminBOQuery, Response<List<AdminBODto>>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;
            public GetAllAdminBOQueryHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<List<AdminBODto>>> Handle(GetAllAdminBOQuery query, CancellationToken cancellationToken)
            {
                Response<List<AdminBODto>> res = new();
                res.Message = new List<string>();


                List<User> usrs1 = await _context.Users.Where(u=> u.IsAdmin== true).ToListAsync();
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
                
                List<AdminBODto> usuarios = new();
                foreach(User u in usrs1)
                {
                    var usr = _mapper.Map<AdminBODto>(u);
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

