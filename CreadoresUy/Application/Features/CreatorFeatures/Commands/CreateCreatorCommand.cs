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

namespace Application.Features.CreatorFeatures.Commands
{
    public class CreateCreatorCommand : IRequest<int>
    {

        public int UserId {  get; set; }
        public string CreatorName { get; set; }
        public string NickName { get; set; }
        public string CreatorDescription { get; set; }
        public DateTime CreatorCreated { get; set; }
        public string YoutubeLink { get; set; }
        public string WelcomeMsg { get; set; }
        public int Followers { get; set; }
        public bool Deleted { get; set; }

        public class CreateCreatorCommandHandler : IRequestHandler<CreateCreatorCommand, int>
        {
            private readonly ICreadoresUyDbContext _context;
            public CreateCreatorCommandHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(CreateCreatorCommand command, CancellationToken cancellationToken)
            {
                var creator = new Creator();

                creator.CreatorName = command.CreatorName;
                creator.NickName = command.NickName;
                creator.CreatorDescription = command.CreatorDescription;
                creator.CreatorCreated = command.CreatorCreated;
                creator.YoutubeLink = command.YoutubeLink;
                creator.WelcomeMsg = command.WelcomeMsg;
                creator.Followers = command.Followers;
                creator.Deleted  = command.Deleted;

                _context.Creators.Add(creator);
                await _context.SaveChangesAsync();

                var user = _context.Users.Where(a => a.Id == command.UserId).FirstOrDefault();

                user.CreatorId = creator.Id;

                await _context.SaveChangesAsync();


                return creator.Id;
            }
        }
    }
}
