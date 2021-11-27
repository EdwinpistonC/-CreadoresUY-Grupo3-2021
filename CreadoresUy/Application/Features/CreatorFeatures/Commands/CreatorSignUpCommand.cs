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
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.CreatorFeatures.Commands
{
    public class CreatorSignUpCommand : IRequest<Response<String>>
    {
        public CreatorDto CreatorDto { get; set; }
        public class CreatorSignUpCommandHandler : IRequestHandler<CreatorSignUpCommand, Response<String>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;
            private readonly ImagePostService _imagePost;

            public CreatorSignUpCommandHandler(ICreadoresUyDbContext context, IMapper mapper, ImagePostService imagePost)
            {
                _context = context;
                _mapper = mapper;
                _imagePost = imagePost;
            }

            public async Task<Response<String>> Handle(CreatorSignUpCommand command, CancellationToken cancellationToken)
            {
                var dto = command.CreatorDto;
                Response<string> res = new Response<String>
                {
                    Obj = "",
                    Message = new List<String>()
                };
                
                var validator = new CreatorSignUpCommandValidator(_context);
                ValidationResult result = validator.Validate(dto);
                if (!result.IsValid)
                {
                    res.Success = false;
                    res.CodStatus = HttpStatusCode.BadRequest;
                    foreach(var error in result.Errors)
                    {
                        var msj = error.ErrorMessage;
                        res.Message.Add(msj);
                    }
                    return res;
                }
                //Datos FINANCIEROS del creador
                var entidad = _context.FinancialEntities.Where(e => e.Name == dto.InfoPago.NombreEntidadFinanciera)
                                                        .FirstOrDefault();
                BanckAccount banck = new();
                banck.AccountHolder = dto.InfoPago.NombreTitular;
                banck.Date = DateTime.UtcNow;
                banck.AccountNumber = dto.InfoPago.NumeroDeCuenta;
                banck.FinancialEntity = entidad;
                banck.FinancialEntityId = entidad.Id;
                _context.BanckAccounts.Add(banck);
                entidad.BanckAccounts.Add(banck);

                //Almacenamiento externo de imagenes en FIREBASE
                ImageDto dtoImgCre = new(dto.CreatorImage,dto.NickName+"photo","Creadores");
                ImageDto dtoImgCreCover = new(dto.CoverImage, dto.NickName + "cover", "PortadasCreadores");
                var urlCreatorImg = await _imagePost.postImage(dtoImgCre);
                var urlCreatorCoverImg = await _imagePost.postImage(dtoImgCreCover);

                var cre = new Creator
                {
                    CreatorName = dto.CreatorName,
                    NickName = dto.NickName,
                    CreatorCreated = DateTime.UtcNow,
                    ContentDescription = dto.ContentDescription,
                    Biography = dto.Biography,
                    CreatorImage = urlCreatorImg,
                    CoverImage = urlCreatorCoverImg,
                    Plans = new List<Plan>(),
                    YoutubeLink = dto.YoutubeLink,
                    BanckAccountId = banck.Id,
                    BanckAccount = banck
                };

                if(dto.Category1 == "" && dto.Category2 == "")
                {
                    res.Success = false;
                    res.CodStatus = HttpStatusCode.BadRequest;
                    res.Message.Add("Al menos debe ingresar una categoria");
                    return res;
                }
                cre.Category1 = dto.Category1 != "" ? dto.Category1 : "";
                cre.Category2 = dto.Category2 != "" ? dto.Category2 : "";

                var u = _context.Users.Where(u => u.Id == dto.IdUser).FirstOrDefault();
                _context.Creators.Add(cre);
                await _context.SaveChangesAsync();
                u.CreatorId = cre.Id;
                banck.Creator = cre;
                banck.CreatorId = cre.Id;
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

