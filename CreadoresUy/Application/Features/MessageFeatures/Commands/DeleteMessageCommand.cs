using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.MessageFeatures.Commands
{
    public class DeleteMessageCommand : IRequest<int>
    {
        public int IdChat { get; set; }
        public int IdMessage { get; set; }
        
        public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, int>
        {
            private readonly ICreadoresUyDbContext _context;

            public DeleteMessageCommandHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(DeleteMessageCommand command, CancellationToken cancellationToken)
            {
                /*
                var mensaje = _context.Messages.Where(m => m.Id == command.IdMessage)
                    .Where(m => m.IdChat == command.IdChat).FirstOrDefault();


                if (mensaje != null)
                {
                    mensaje.Deleted = true;
                    await _context.SaveChangesAsync();
                    return 200; //Retorno ficticio corregir
                }
                else
                {
                    return default;
                }
                */
                


                   var Chat = _context.Chats.Where(a => a.Id == command.IdChat).Include(c => c.Messages).FirstOrDefault();
                   if (Chat != null) { 
                       var Mensajes = Chat.Messages;
                        System.Console.WriteLine("salida de texto");

                        System.Console.WriteLine(Chat.Id);
                        System.Console.WriteLine(Chat.Messages.Count());
                        if (Mensajes != null) { 
                           foreach (var item in Mensajes)
                           {
                               if (item.Id == command.IdMessage)
                               {
                                   var mensaje = item;
                                   if (mensaje != null)
                                   {
                                       mensaje.Deleted = true;
                                       await _context.SaveChangesAsync();
                                       return 200;
                                   }
                                   else
                                   {
                                       return default;
                                   }
                               }
                           }
                       }
                    return 403; //Mensaje no existe en chat Valido
                   }
                   else
                   {
                       return 404; //El cht no existe

                   }

               
            }
        }
    }
}