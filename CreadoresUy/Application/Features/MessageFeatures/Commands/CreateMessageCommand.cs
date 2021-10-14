using Application.Interface;
using MediatR;
using Share.Dtos;
using Share.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.MessageFeatures.Commands
{
    public class CreateMessageCommand : IRequest<int>
    {
        public MessageDto MessageDto {  get; set; }

        public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, int>
        {
            private readonly ICreadoresUyDbContext _context;

            public CreateMessageCommandHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(CreateMessageCommand command, CancellationToken cancellationToken)
            {
                var mDto = command.MessageDto;

                var chat = _context.Chats.Where(a => a.IdCreator == mDto.IdCreator)
                    .Where(a => a.IdUser == mDto.IdUser).FirstOrDefault();
                if (chat == null)
                {
                    chat = new Chat(mDto.IdCreator, mDto.IdUser);

                    _context.Chats.Add(chat);
                    await _context.SaveChangesAsync();
                }

                int idUserSended;
                if (mDto.TipoEmisor.ToString().Equals("U"))
                {
                    idUserSended = mDto.IdUser;

                }
                else
                {
                    var user = _context.Users.Where(a => a.CreatorId == mDto.IdCreator).FirstOrDefault();
                    idUserSended = user.Id;
                }


                var mensaje = new Message(
                    idUserSended,
                    mDto.TipoEmisor,
                    mDto.Text,
                    DateTime.Now,
                    chat.Id);


                _context.Messages.Add(mensaje);
                chat.Messages.Add(mensaje);

                await _context.SaveChangesAsync();
                return chat.Id;


            }
        }

    }
}
