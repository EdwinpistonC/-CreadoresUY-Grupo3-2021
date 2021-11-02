using Application.Interface;
using AutoMapper;
using MediatR;
using Share;
using Share.Dtos;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.CreatorFeatures.Commands
{
    public class CreateCreatorCommand : IRequest<Response<string>>
    {

        public CreatorRawDto Creator { get; set; }
        public class CreateCreatorCommandHandler : IRequestHandler<CreateCreatorCommand, Response<string>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public CreateCreatorCommandHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Response<string>> Handle(CreateCreatorCommand command, CancellationToken cancellationToken)
            {
                var user = _context.Users.Where(a => a.Id == command.Creator.UserId).FirstOrDefault();
                Response<string> res = new Response<String>
                {
                    Obj = "",
                    Message = new List<String>()
                };
                if (user == null)
                {
                    res.CodStatus = HttpStatusCode.BadRequest;
                    res.Success = false;
                    res.Message.Add("Id de usuario no existe");
                    return res;
                }else if (user.CreatorId != null)
                {
                    res.CodStatus = HttpStatusCode.BadRequest;
                    res.Success = false;
                    res.Message.Add("Usuario ya posee cuenta de creador");
                    return res;
                }
                var creator = _mapper.Map<Creator>(command.Creator);

                var creatorDto = command.Creator;


                _context.Creators.Add(creator);
                await _context.SaveChangesAsync();


                user.CreatorId = creator.Id;

                await _context.SaveChangesAsync();



                res.CodStatus = HttpStatusCode.Created;
                res.Success = true;
                var msg1 = "Creador ingresado correctamente";
                res.Message.Add(msg1);

                return res;
            }
        }
    }
}
