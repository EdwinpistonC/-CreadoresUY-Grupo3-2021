using Application.Features.Validators;
using Application.Interface;
using Application.Service;
using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Share.Dtos;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserFeaturesBO.Commands
{
    public class CreateUserCommandBO : IRequest<Response<String>>
    {
        public String Name { get; set; }
        public String Image { get; set; }
        public String Password { get; set; }
        public String Email { get; set; }
        public class CreateUserCommandBOHandler : IRequestHandler<CreateUserCommandBO, Response<String>>
        {
            private readonly ICreadoresUyDbContext _context;

            private readonly ImagePostService _imagePost;
            public CreateUserCommandBOHandler(ICreadoresUyDbContext context, ImagePostService imagePost)
            {
                _context = context;
                _imagePost = imagePost;
            }

            public async Task<Response<String>> Handle(CreateUserCommandBO command, CancellationToken cancellationToken)
            {

                Response<string> res = new Response<String>
                {
                    Obj = "",
                    Message = new List<String>()
                };

                var user = new User();
                user.Name = command.Name;
                user.Email = command.Email;

                ImageDto dtoImgPrf = new(command.Image, user.Name+  DateTime.Now.ToString() + "photo", "Usuarios");
                var urlCreatorImg = await _imagePost.postImage(dtoImgPrf);

                user.ImgProfile = urlCreatorImg;
                user.Created = DateTime.Now;
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                res.CodStatus = HttpStatusCode.Created;
                res.Success = true;
                var msg1 = "Usuario ingresado correctamente";
                res.Message.Add(msg1);
                return res;
            }
        }
    }
}

