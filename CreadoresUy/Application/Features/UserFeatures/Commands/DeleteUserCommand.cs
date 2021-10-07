using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserFeatures.Commands
{

    
    public class DeleteUserCommand : IRequest<int>
    {
        public int Id { get; set; }
        public class DeleteProductByIdCommandHandler : IRequestHandler<DeleteUserCommand, int>
        {
            private readonly ICreadoresUyDbContext _context;
            public DeleteProductByIdCommandHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
            {
                var user = await _context.Users.Where(a => a.Id == command.Id).FirstOrDefaultAsync();
                if (user == null) return default;
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return user.Id;
            }
        }
    }
}
