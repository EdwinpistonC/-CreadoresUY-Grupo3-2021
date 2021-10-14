using Application.Interface;
using AutoMapper;
using MediatR;
using Share.Dtos;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserFeatures.Commands
{
    public class UserSignInCommand : IRequest<ReturnDto>
    {
        public CreateUserDto CreateUserDto { get; set; }
        public class UserSignInCommandHandler : IRequestHandler<UserSignInCommand, ReturnDto>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public UserSignInCommandHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<ReturnDto> Handle(UserSignInCommand command, CancellationToken cancellationToken)
            {
                var dto = command.CreateUserDto;
                var userB = _context.Users.Where(u => u.Email == dto.Email).FirstOrDefault();
                if (userB != null)
                {
                    var item = new ItemDto(409, "El correo ingresado ya se encuentra en registrado en el sistema");
                    var list = new List<ItemDto>
                    {
                        item
                    };
                    var ret1 = new ReturnDto(false,list);
                    return ret1;
                }
                   
                var user = _mapper.Map<User>(dto);
                user.Created = DateTime.Now;
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                var item1 = new ItemDto(200, "Usuario ingresado correctamente");
                var list1 = new List<ItemDto>
                    {
                        item1
                    };
                var ret = new ReturnDto(true,list1);
                return ret;
            }
        }

    }
}
