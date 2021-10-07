using Application.Interface;
using AutoMapper;
using MediatR;
using Share.Dtos;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ContentFeature.Commands
{
    public class CreateContentCommand : IRequest<int>
    {
        public ContentDto Content {  get; set; }
        public ICollection<PlanDto> Plans { get; set; }

        public ICollection<TagDto> Tags { get; set; }

        public class CreateContentCommandHandler : IRequestHandler<CreateContentCommand, int>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public CreateContentCommandHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<int> Handle(CreateContentCommand command, CancellationToken cancellationToken)
            {
                var content = _mapper.Map<Content>(command.Content);
                content.ContentPlans = new List<ContentPlan>();

                content.ContentTags = new List<ContentTag>();

                if (command.Plans != null)
                {
                    foreach (var p in command.Plans)
                    {
                        var contentPlan = new ContentPlan()
                        {
                            IdPlan = p.Id,
                            IdContent = content.Id
                        };
                        content.ContentPlans.Add(contentPlan);
                    }
                }
                

                foreach (var t in command.Tags)
                {
                    var tag = new Tag()
                    {
                        Name = t.Name
                    };
                    _context.Tags.Add(tag);
                    await _context.SaveChangesAsync();

                    var contentTag = new ContentTag()
                    {
                        IdTag = tag.Id,
                        IdContent = content.Id
                    };
                    content.ContentTags.Add(contentTag);
                }


                _context.Contents.Add(content);

                await _context.SaveChangesAsync();
                return content.Id;
            }
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
