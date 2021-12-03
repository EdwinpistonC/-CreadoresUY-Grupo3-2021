using Application.Features.Validators;
using Application.Interface;
using Application.Service;
using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Share.Dtos;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.CreatorFeaturesBO.Commands
{
    public class CreateCreatorCommandBO : IRequest<Response<String>>
    {
        public CreatorBODto CreatorBODto { get; set; }
        public class CreateCreatorCommandBOHandler : IRequestHandler<CreateCreatorCommandBO, Response<String>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;
            private readonly ImagePostService _imagePost;

            public CreateCreatorCommandBOHandler(ICreadoresUyDbContext context, IMapper mapper, ImagePostService imagePost)
            {
                _context = context;
                _mapper = mapper;
                _imagePost = imagePost;
            }

            public async Task<Response<String>> Handle(CreateCreatorCommandBO command, CancellationToken cancellationToken)
            {
                var dto = command.CreatorBODto;
                var user = await _context.Users.Where(u => u.Id == dto.UserId).FirstOrDefaultAsync();

                Response<string> res = new Response<String>
                {
                    Obj = "",
                    Message = new List<String>()
                };


                var creator = _mapper.Map<Creator>(dto);

                if (creator.CoverImage != "")
                {
                    ImageDto dtoImgPrf = new(creator.CoverImage, creator.NickName + DateTime.Now.ToString() + "photo", "PortadasCreadores");
                    var urlCreatorImg = await _imagePost.postImage(dtoImgPrf);
                    creator.CoverImage = urlCreatorImg;
                }

                if (creator.CreatorImage != "")
                {
                    ImageDto dtoImgPrf = new(creator.CreatorImage, creator.NickName + DateTime.Now.ToString() + "photo", "Creadores");
                    var urlCreatorImg = await _imagePost.postImage(dtoImgPrf);
                    creator.CreatorImage = urlCreatorImg;
                }

                creator.CreatorCreated = DateTime.Now;
                creator.User = user;
                _context.Creators.Add(creator);

                

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

