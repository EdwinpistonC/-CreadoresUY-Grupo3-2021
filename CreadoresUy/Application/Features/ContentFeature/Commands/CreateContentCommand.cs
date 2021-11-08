using Application.Features.Validators;
using Application.Interface;
using AutoMapper;
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

namespace Application.Features.ContentFeature.Commands
{
    public class CreateContentCommand : IRequest<Response<String>>
    {
        public ContentDto Content {  get; set; }
       

        public class CreateContentCommandHandler : IRequestHandler<CreateContentCommand, Response<String>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public CreateContentCommandHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Response<String>> Handle(CreateContentCommand command, CancellationToken cancellationToken)
            {
                var dto = command.Content;
                Response<string> res = new Response<String>();
                res.Message = new List<String>();
                var validator = new CreateContentCommandValidator(_context);
                ValidationResult result = validator.Validate(dto);

                if (!result.IsValid)
                {
                    res.CodStatus = HttpStatusCode.BadRequest;
                    res.Success = false;
                    foreach (var error in result.Errors)
                    {
                        var msg = error.ErrorMessage;
                        res.Message.Add(msg);
                    }
                    return res;
                }

                var content = _mapper.Map<Content>(command.Content);

                content.ContentPlans = new List<ContentPlan>();

                content.ContentTags = new List<ContentTag>();

                if (content.Draft == false)
                {
                    content.DatePublish = DateTime.Now;
                }
                _context.Contents.Add(content);
                await _context.SaveChangesAsync();

                if (command.Content.Plans != null)
                {
                    foreach (var planId in command.Content.Plans)
                    {

                        var contentPlan = new ContentPlan
                        {
                            IdPlan = planId,
                            IdContent = content.Id
                        };

                        content.ContentPlans.Add(contentPlan);
                    }
                }
                await _context.SaveChangesAsync();

                if (command.Content.Tags != null)
                {
                    foreach (var t in command.Content.Tags)
                    {
                        var tag = _mapper.Map<Tag>(t);
                        var findTag = await _context.Tags.Where(x => x.Name == tag.Name).FirstOrDefaultAsync();
                        Console.WriteLine("gol");

                        Console.WriteLine(findTag != null);

                        if (findTag != null)
                        {
                            Console.WriteLine(findTag.Id);

                            tag.Id = findTag.Id;
                        }
                        else
                        {
                            _context.Tags.Add(tag);
                            await _context.SaveChangesAsync();

                        }

                        var tagContent = new ContentTag
                        {
                            IdTag= tag.Id,
                            IdContent= content.Id
                        };
                        content.ContentTags.Add(tagContent);
                    }
                }
                await _context.SaveChangesAsync();



                res.CodStatus = HttpStatusCode.Accepted;
                res.Success = true;
                var msg1 = "Content ingresado correctamente";
                res.Message.Add(msg1);
                return res;
        }

    }








        public class CreateUserCommand : IRequest<int>
        {
            public string Name { get; set; }

            public string Email { get; set; }
            public string Password { get; set; }
            public string? Description { get; set; }
            public DateTime Created { get; set; }
            public DateTime? LasLogin { get; set; }
            public string? ImgProfile { get; set; }
            public int CreatorId { get; set; }

            public Creator? Creator { get; set; }

            public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
            {
                private readonly ICreadoresUyDbContext _context;
                public CreateUserCommandHandler(ICreadoresUyDbContext context)
                {
                    _context = context;
                }
                public async Task<int> Handle(CreateUserCommand command, CancellationToken cancellationToken)
                {
                    var user = new User();

                    user.Name = command.Name;
                    user.Email = command.Email;
                    user.Password = command.Password;
                    user.Description = command.Description;
                    user.Created = command.Created;
                    user.LasLogin = command.LasLogin;
                    user.ImgProfile = command.ImgProfile;

                    if (command.Creator != null)
                    {
                        var creator = _context.Creators.Find(command.Creator.Id);
                        if (creator == null)
                        {
                            user.Creator = command.Creator;
                        }
                        else
                        {
                            user.CreatorId = command.Creator.Id;
                        }
                    }
                    if (command.CreatorId != 0)
                    {
                        user.CreatorId = command.CreatorId;
                    }
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    return user.Id;
                }
            }
        }
    }
}
