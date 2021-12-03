using Application.Interface;
using Application.Service;
using AutoMapper;
using MediatR;
using Share.Dtos;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AdminFeaturesBO.Commands
{
    public class UpdateAdminCommandBO : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LasLogin { get; set; }
        public string? ImgProfile { get; set; }

        public class  UpdateAdminCommandBOHandler : IRequestHandler<UpdateAdminCommandBO, Response<string>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly ImagePostService _imagePost;

            public UpdateAdminCommandBOHandler(ICreadoresUyDbContext context , ImagePostService imagePost)
            {
                _context = context;
                _imagePost= imagePost;
            }
            public async Task<Response<string>> Handle(UpdateAdminCommandBO command, CancellationToken cancellationToken)
            {
                var user = _context.Users.Where(u => u.Id == command.Id).FirstOrDefault();
                Response<string> res = new()
                {
                    Message = new List<string>(),
                    Obj = ""
                };
                if (user == null )
                {
                    res.Message.Add("No se ha encontrado el usuario de id: " + command.Id);
                    res.Success = false;
                    res.CodStatus = HttpStatusCode.BadRequest;
                    return res;
                }

                if (command.Name != "") user.Name = command.Name;
                if (command.Email != "") user.Email = command.Email;
                if (command.Description != "") user.Description = command.Description;
                if (command.ImgProfile != "")
                {
                    ImageDto dtoImgPrf = new(command.ImgProfile, user.Name + DateTime.Now.ToString() + "photo", "Usuarios");
                    var urlCreatorImg = await _imagePost.postImage(dtoImgPrf);
                    user.ImgProfile = urlCreatorImg;

                }

                if (command.Password != "") user.Password = command.Password;
                if (command.Created != DateTime.MinValue) user.Created = command.Created;
                if (command.LasLogin !=  DateTime.MinValue) user.LasLogin = command.LasLogin;

                await _context.SaveChangesAsync();
                res.Message.Add("Exito");
                res.Success = true;
                res.CodStatus = HttpStatusCode.OK;
                return res;
            }

        }
        
    }
   
}
