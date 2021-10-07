using Application.Interface;
using MediatR;
using Share;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserFeatures.Commands
{

    public class UpdateUserCommand : IRequest<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public string? Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LasLogin { get; set; }
        public string? ImgProfile { get; set; }
        public int CreatorId { get; set; }

        public Creator? Creator { get; set; }

        public class UpdateProductCommandHandler : IRequestHandler<UpdateUserCommand, int>
        {
            private readonly ICreadoresUyDbContext _context;
            public UpdateProductCommandHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
            {
                var user = _context.Users.Where(a => a.Id == command.Id).FirstOrDefault();

                if (user == null)
                {
                    return default;
                }
                else

                {
                    user.Name = command.Name;
                    user.Email = command.Email;
                    user.Password = command.Password;
                    user.Description = command.Description;
                    user.Created = command.Created;
                    user.LasLogin = command.LasLogin;
                    user.ImgProfile = command.ImgProfile;
                    if (command.Creator != null)
                    {
                        user.Creator = command.Creator;
                    }
                    if (command.CreatorId != 0)
                    {
                        user.CreatorId = command.CreatorId;
                    }
                    await _context.SaveChangesAsync();
                    return user.Id;
                }
            }
        }
    }
}
