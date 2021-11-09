using Application.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.CreatorFeatures.Queries
{
    public class IsValidNicknameQuery :IRequest<bool> //Mediador
    {
        public string Nickname {  get; set; }
        public class IsValidNicknameQueryHandler : IRequestHandler<IsValidNicknameQuery, bool>
        {
            //Implementamos el IRequest, recibimos IsValidNicknameQuery y vamos a retornar un bool
            //Cuando del Controlador llaman a la query automaticamente se ejecuta el mediador IsValidNicknameQueryHandler
            private readonly ICreadoresUyDbContext _context;
            public IsValidNicknameQueryHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }
            public async Task<bool> Handle(IsValidNicknameQuery query, CancellationToken cancellationToken)
            {
                if(query.Nickname != null) { 
                    var cre =  _context.Creators.Where(c => c.NickName == query.Nickname).FirstOrDefault();
                    if(cre == null)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
        }

    }
}
