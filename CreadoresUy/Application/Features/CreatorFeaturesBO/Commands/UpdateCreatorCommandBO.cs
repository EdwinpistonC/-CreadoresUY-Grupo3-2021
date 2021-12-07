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

namespace Application.Features.CreatorFeaturesBO.Commands
{
    public class UpdateCreatorCommandBO : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public string CreatorName { get; set; }
        public string NickName { get; set; }
        public string ContentDescription { get; set; }
        public string Biography { get; set; }
        public string YoutubeLink { get; set; }
        public string CreatorImage { get; set; }
        public string CoverImage { get; set; }
        public string WelcomeMsg { get; set; }
        public String Category1 { get; set; }
        public String Category2 { get; set; }

        public class UpdateCreatorCommandBOHandler : IRequestHandler<UpdateCreatorCommandBO, Response<string>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly ImagePostService _imagePost;

            public UpdateCreatorCommandBOHandler(ICreadoresUyDbContext context, ImagePostService imagePost)
            {
                _context = context;
                _imagePost = imagePost;
            }
            public async Task<Response<string>> Handle(UpdateCreatorCommandBO command, CancellationToken cancellationToken)
            {
                var user = _context.Creators.Where(u => u.Id == command.Id).FirstOrDefault();
                Response<string> res = new()
                {
                    Message = new List<string>(),
                    Obj = ""
                };
                if (user == null )
                {
                    res.Message.Add("No se ha encontrado el creador de id: " + command.Id);
                    res.Success = false;
                    res.CodStatus = HttpStatusCode.BadRequest;
                    return res;
                }

                if (command.CreatorName != "") user.CreatorName = command.CreatorName;
                if (command.NickName != "") user.NickName = command.NickName;
                if (command.ContentDescription != "") user.ContentDescription = command.ContentDescription;
                if (command.Biography != "") user.Biography = command.Biography;
                if (command.YoutubeLink != "") user.YoutubeLink = command.YoutubeLink;
                if (command.CreatorImage != "") {
                    ImageDto dtoImgPrf = new(command.CreatorImage, command.NickName + DateTime.Now.ToString() + "photo", "Creadores");
                    var urlCreatorImg = await _imagePost.postImage(dtoImgPrf);
                    user.CreatorImage = urlCreatorImg;
                }
                if (command.CoverImage != "") {
                    ImageDto dtoImgPrf = new(command.CoverImage, command.NickName + DateTime.Now.ToString() + "photo", "PortadasCreadores");
                    var urlCreatorImg = await _imagePost.postImage(dtoImgPrf);
                    user.CoverImage = urlCreatorImg;
                }
                if (command.WelcomeMsg != "") user.WelcomeMsg = command .WelcomeMsg;
                if (command.Category1 != "") user.Category1 = command.Category1;
                if (command.Category2 != "") user.Category2 = command.Category2;

                await _context.SaveChangesAsync();
                res.Message.Add("Exito");
                res.Success = true;
                res.CodStatus = HttpStatusCode.OK;
                return res;
            }

        }
        
    }
   
}
