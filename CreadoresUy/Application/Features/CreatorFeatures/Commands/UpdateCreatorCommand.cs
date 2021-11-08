using Application.Features.Validators;
using Application.Interface;
using FluentValidation.Results;
using MediatR;
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

    public class UpdateCreatorCommand : IRequest<Response<String>>
    {
        public int CreatorId { get; set; }

        public CreatorRawDto Creator { get; set; }

        public class UpdateCreatorCommandHandler : IRequestHandler<UpdateCreatorCommand, Response<String>>
        {
            private readonly ICreadoresUyDbContext _context;
            public UpdateCreatorCommandHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }
            public async Task<Response<String>> Handle(UpdateCreatorCommand command, CancellationToken cancellationToken)
            {



                Response<string> res = new Response<String>
                {
                    Obj = "",
                    Message = new List<String>()
                };
                Creator creator = _context.Creators.Where(c => c.Id == command.CreatorId).FirstOrDefault();
                CreatorRawDto creatorDto = command.Creator;
                Console.WriteLine("resultador ahora");

                Console.WriteLine(creatorDto.UserId);

                if (creator == null)
                {
                    res.Success = false;
                    res.CodStatus = HttpStatusCode.BadRequest;
                    res.Message.Add("Id creador no existe");
                    return res;
                }

                CreatorUpdateCommandValidator validator = new CreatorUpdateCommandValidator(_context);
                ValidationResult result = validator.Validate(creatorDto);

                if (!result.IsValid)
                {
                    res.Success = false;
                    res.CodStatus = HttpStatusCode.BadRequest;
                    foreach (var error in result.Errors)
                    {
                        String msj = error.ErrorMessage;
                        res.Message.Add(msj);
                    }
                    return res;
                }
                if (creatorDto.Category1 != null) creator.Category1 = creatorDto.Category1;
                if (creatorDto.Category2 != null) creator.Category2 = creatorDto.Category2;
                if (creatorDto.CreatorDescription != null) creator.ContentDescription = creatorDto.CreatorDescription;
                if (creatorDto.CreatorName != null) creator.CreatorName = creatorDto.CreatorName;
                if (creatorDto.WelcomeMsg != null) creator.WelcomeMsg = creatorDto.WelcomeMsg;
                if (creatorDto.YoutubeLink != null) creator.YoutubeLink = creatorDto.YoutubeLink;

                await _context.SaveChangesAsync();
                res.Success = true;
                res.CodStatus = HttpStatusCode.OK;
                res.Message.Add("Creator modificado");
                return res;
            }
        }
    }
}


