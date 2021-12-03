using Application.Interface;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Share.Dtos;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.CreatorFeaturesBO.Queries
{
    public class GetCreatorByIdBOQuery : IRequest<Response<CreatorBODto>>
    {
        public int Id {  get; set; }

        public class GetCreatorByIdBOQueryHandler : IRequestHandler<GetCreatorByIdBOQuery, Response<CreatorBODto>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public GetCreatorByIdBOQueryHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<CreatorBODto>> Handle(GetCreatorByIdBOQuery query, CancellationToken cancellationToken)
            {
                var creator = _context.Creators.Where(u => u.Id == query.Id ).FirstOrDefault();

                var usuario = _context.Users.Where(u => u.CreatorId == query.Id).FirstOrDefault();

                Response<CreatorBODto> res = new();
                res.Message = new List<string>();
                //if (user == null || user.Deleted == true)
                if (creator == null)
                {
                    res.Obj = default;
                    res.CodStatus = HttpStatusCode.BadRequest;
                    res.Success = false;
                    var msj = "No se ha encontrado un usuario asociado al id ingresado";
                    res.Message.Add(msj);
                    return res;
                }
                var dto = _mapper.Map<CreatorBODto>(creator);
                dto.UserId = usuario.Id;
                dto.NoNulls();
                res.Obj = dto;
                res.CodStatus = HttpStatusCode.OK;
                res.Success = true;
                var msj1 = "Ok";
                res.Message.Add(msj1);
                return res;

            }

        }
    }
}
