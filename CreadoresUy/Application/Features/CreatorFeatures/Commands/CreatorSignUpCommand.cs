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
    public class CreatorSignUpCommand : IRequest<Response<int>>
    {
        public CreatorDto CreatorDto { get; set; }
        public class CreatorSignUpCommandHandler : IRequestHandler<CreatorSignUpCommand, Response<int>>
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

            public async Task<Response<int>> Handle(CreatorSignUpCommand command, CancellationToken cancellationToken)
            {
                var dto = command.CreatorDto;
                Response<int> res = new Response<int>
                {
                    Message = new List<String>()
                };
                
                var validator = new CreatorSignUpCommandValidator(_context);
                ValidationResult result = validator.Validate(dto);
                if (!result.IsValid)
                {
                    res.Obj = 0;
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

                var cre = new Creator
                {
                    CreatorName = dto.CreatorName,
                    NickName = dto.NickName,
                    CreatorCreated = DateTime.UtcNow,
                    ContentDescription = dto.ContentDescription,
                    Biography = dto.Biography,
                    Plans = new List<Plan>(),
                    YoutubeLink = dto.YoutubeLink,
                    BanckAccountId = banck.Id,
                    BanckAccount = banck
                };

                //Almacenamiento externo de imagenes en FIREBASE
                if (dto.CreatorImage != string.Empty) {
                    ImageDto dtoImgCre = new(dto.CreatorImage, dto.NickName + "photo", "Creadores");
                    var urlCreatorImg = await _imagePost.postImage(dtoImgCre);
                    cre.CreatorImage = urlCreatorImg;
                }
                else
                {
                    cre.CreatorImage = "https://firebasestorage.googleapis.com/v0/b/creadoresuy-674c1.appspot.com/o/creadores%2Fdefaultimage.jpg?alt=media&token=20acde1a-b875-43dd-a68c-e36a6e8c2abc";
                }
                if (dto.CoverImage != string.Empty)
                {
                    ImageDto dtoImgCreCover = new(dto.CoverImage, dto.NickName + "cover", "PortadasCreadores");
                    var urlCreatorCoverImg = await _imagePost.postImage(dtoImgCreCover);
                    cre.CoverImage = urlCreatorCoverImg;
                }
                else
                {
                    cre.CoverImage = "https://firebasestorage.googleapis.com/v0/b/creadoresuy-674c1.appspot.com/o/PortadasCreadores%2FCreado1cover?alt=media&token=20b0460f-94e2-499a-9528-5fd880e37ef8";
                }

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
                //Setear los planes por defecto
                var listplan = _context.DefaultPlans.ToList();
                var listdb = _context.DefaultBenefits.ToList();
                if (listplan.Count != 0) { 
                    foreach(var item in listplan)
                    {
                        var plan = new Plan(item.Name, item.Description, item.Price, item.Image, item.SubscriptionMsg, item.WelcomeVideoLink, cre.Id, cre);
                        _context.Plans.Add(plan);
                        await _context.SaveChangesAsync();
                        foreach (var b in listdb)
                        {
                            if(b.IdDefaultPlan== item.Id) { 
                                var ben = new Benefit(b.Description, plan.Id, plan);
                                _context.Benefits.Add(ben);
                                await _context.SaveChangesAsync();
                                plan.Benefits.Add(ben);
                            }
                        }
                        cre.Plans.Add(plan);
                        await _context.SaveChangesAsync();
                    }
                }
                res.Obj = cre.Id;
                res.CodStatus = HttpStatusCode.Created;
                res.Success = true;
                var msg1 = "Usuario ingresado correctamente";
                res.Message.Add(msg1);
                return res;        
            }
        }
    }
}

