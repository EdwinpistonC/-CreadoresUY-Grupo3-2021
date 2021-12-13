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

namespace Application.Features.UserFeatures.Queries
{
    public class GetUserProfileQuery : IRequest<Response<UserProfileDto>>
    {
        public int IdUser { get; set; }
        public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, Response<UserProfileDto>>
        {
            private readonly ICreadoresUyDbContext _context;

            public GetUserProfileQueryHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }

            public async Task<Response<UserProfileDto>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
            {
                var resp = new Response<UserProfileDto>
                {
                    Message = new List<string>()
                };
                var usr = _context.Users.Where(u => u.Id == request.IdUser).FirstOrDefault();
                if (usr == null)
                {
                    var dto = new UserProfileDto();
                    dto.FixIfIsNull();
                    resp.Obj = dto;
                    resp.Message.Add("No se ha encontrado al usuario ingresado");
                    resp.CodStatus = HttpStatusCode.BadRequest;
                    resp.Success = false;
                }
                else
                {
                    var dto = new UserProfileDto(usr.Name, usr.Email, usr.ImgProfile, usr.Created);
                    dto.FixIfIsNull();
                    resp.Obj = dto;
                    resp.Message.Add("Exito");
                    resp.CodStatus = HttpStatusCode.OK;
                    resp.Success = true;
                }
                return resp;
            }
        }
    }
}
