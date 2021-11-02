
using Application.Interface;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Share.Dtos;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.CreatorFeatures.Commands
{

    public class DeleteCreatorCommand : IRequest<Response<String>>
    {
        public int CreatorId { get; set; }

        public class DeleteCreatorCommandHandler : IRequestHandler<DeleteCreatorCommand, Response<String>>
        {
            private readonly ICreadoresUyDbContext _context;
            public DeleteCreatorCommandHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }
            public async Task<Response<String>> Handle(DeleteCreatorCommand command, CancellationToken cancellationToken)
            {



                Response<string> res = new Response<String>
                {
                    Obj = "",
                    Message = new List<String>()
                };
                Creator creator = _context.Creators.Where(c => c.Id == command.CreatorId).FirstOrDefault();


                if (creator == null)
                {
                    res.Success = false;
                    res.CodStatus = HttpStatusCode.BadRequest;
                    res.Message.Add("Id creador no existe");
                    return res;
                }


                var users = await _context.Users.Where(u => u.CreatorId == command.CreatorId).ToListAsync();

                users.ForEach(u => u.CreatorId = null);
                await _context.SaveChangesAsync();

                _context.Creators.Remove(creator);

                await _context.SaveChangesAsync();
                res.Success = true;
                res.CodStatus = HttpStatusCode.OK;
                res.Message.Add("Creator eliminado");
                return res;
            }
        }
    }
}


