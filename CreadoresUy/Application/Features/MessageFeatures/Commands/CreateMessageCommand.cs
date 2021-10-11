using Application.Interface;
using MediatR;
using Share.Dtos;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                var chat = new Chat();
                if (command.MessageDto.TipoEmisor.ToString() == "C")
                {
                    chat = _context.Chats.Where(a => a.IdCreator == command.MessageDto.IdSender)
                    .Where(a => a.IdUser == command.MessageDto.IdReceiver).FirstOrDefault();
                }
                else
                {
                    chat = _context.Chats.Where(a => a.IdCreator == command.MessageDto.IdReceiver)
                    .Where(a => a.IdUser == command.MessageDto.IdSender).FirstOrDefault();
                }

                if(chat != null){
                    var mensaje = new Message();
                    if (command.MessageDto.TipoEmisor.ToString() == "U") { 
                        mensaje.IdUser = command.MessageDto.IdSender;
                        mensaje.User = _context.Users.Where(a => a.Id == command.MessageDto.IdSender).FirstOrDefault();

                    }
                    else
                    {
                        mensaje.User = _context.Users.Where(a => a.CreatorId == command.MessageDto.IdSender).FirstOrDefault();
                        mensaje.IdUser = mensaje.User.Id;
                    }
                    mensaje.TipoEmisor = command.MessageDto.TipoEmisor;
                    mensaje.Text = command.MessageDto.Text;
                    mensaje.IdChat = chat.Id;
                    mensaje.Chat = chat;
                    mensaje.Sended = DateTime.Now;
                    _context.Messages.Add(mensaje);
                    chat.Messages.Add(mensaje);
                    await _context.SaveChangesAsync();
                    return chat.Id;
                }
                else
                {
                    if (command.MessageDto.TipoEmisor.ToString() == "U")
                    {
                        var chat1 = new Chat();
                        chat1.IdUser = command.MessageDto.IdSender;
                        var user = _context.Users.Where(a => a.Id == command.MessageDto.IdSender).FirstOrDefault();
                        chat1.User = user;
                        chat1.IdCreator = command.MessageDto.IdReceiver;
                        var creator = _context.Creators.Where(a => a.Id == command.MessageDto.IdReceiver).FirstOrDefault();
                        chat1.Creator = creator;
                        chat1.Messages = new Collection<Message>();
                        _context.Chats.Add(chat1);

                        var mensaje = new Message
                        {
                            IdUser = command.MessageDto.IdSender,
                            User = user,
                            TipoEmisor = command.MessageDto.TipoEmisor,
                            Text = command.MessageDto.Text,
                            IdChat = chat1.Id,
                            Chat = chat1,
                            Sended = DateTime.Now
                        };
                        _context.Messages.Add(mensaje);
                        chat1.Messages.Add(mensaje);
                        creator.Chats.Add(chat1);
                        user.Chats.Add(chat1);
                        await _context.SaveChangesAsync();
                        return chat1.Id;

                    }
                    else{
                        var chat1 = new Chat();
                        chat1.IdCreator = command.MessageDto.IdSender;
                        var creator = _context.Creators.Where(a => a.Id == command.MessageDto.IdSender).FirstOrDefault();
                        chat1.Creator = creator;
                        chat1.IdUser = command.MessageDto.IdReceiver;
                        var user = _context.Users.Where(a => a.Id == command.MessageDto.IdReceiver).FirstOrDefault();
                        chat1.User = user;
                        chat1.Messages = new Collection<Message>();
                        _context.Chats.Add(chat1);

                        var mensaje = new Message();
                        mensaje.User = _context.Users.Where(a => a.CreatorId == command.MessageDto.IdSender).FirstOrDefault();
                        mensaje.IdUser = mensaje.User.Id;
                        mensaje.TipoEmisor = command.MessageDto.TipoEmisor;
                        mensaje.Text = command.MessageDto.Text;
                        mensaje.IdChat = chat1.Id;
                        mensaje.Chat = chat1;
                        mensaje.Sended = DateTime.Now;
                        _context.Messages.Add(mensaje);

                        chat1.Messages.Add(mensaje);
                        creator.Chats.Add(chat1);   
                        user.Chats.Add(chat1);
                        await _context.SaveChangesAsync();
                        return chat1.Id;
                    }

                }

            }
        }

    }
}
