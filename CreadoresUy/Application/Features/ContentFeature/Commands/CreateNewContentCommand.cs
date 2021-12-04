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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ContentFeature.Commands
{
    public class CreateNewContentCommand : IRequest<Response<string>>
    {
        public ContentDto Content {  get; set; }
        public class CreateNewContentCommandHandler : IRequestHandler<CreateNewContentCommand, Response<string>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;
            private readonly ImagePostService _imagePost;
            public CreateNewContentCommandHandler(ICreadoresUyDbContext context, IMapper mapper, ImagePostService imagePost)
            {
                _context = context;
                _mapper = mapper;
                _imagePost = imagePost;
            }
            public async Task<Response<string>> Handle(CreateNewContentCommand command, CancellationToken cancellationToken)
            {
                var resp = new Response<string>()
                {
                    Message = new List<string>()
                };
                var dto = command.Content;
                if (dto.Public == true) {
                    var val = new CreateNewContentCommandBasicValidator(_context);
                    ValidationResult result = val.Validate(dto);
                    if (!result.IsValid)
                    {
                        foreach (var error in result.Errors)
                        {
                            resp.Message.Add(error.ErrorMessage);
                        }
                        resp.Obj = "Error";
                        resp.Success = false;
                        resp.CodStatus = HttpStatusCode.BadRequest;
                        return resp;
                    }
                }
                else {
                    var val = new CreateNewContentCommandValidator(_context);
                    ValidationResult result = val.Validate(dto);
                    if (!result.IsValid)
                    {
                        foreach (var error in result.Errors)
                        {
                            resp.Message.Add(error.ErrorMessage);
                        }
                        resp.Obj = "Error";
                        resp.Success = false;
                        resp.CodStatus = HttpStatusCode.BadRequest;
                        return resp;
                    }
                    var cre = _context.Creators.Where(c => c.Id == dto.IdCreator).Where(c => c.NickName == dto.NickName).Include(x => x.Plans).FirstOrDefault();
                    var aux = new List<int>();
                    foreach (var item in cre.Plans)
                    {
                        aux.Add(item.Id);
                    }
                    bool err = false;
                    foreach (var item in dto.Plans)
                    {
                        if (!aux.Contains(item))
                        {
                            err = true;
                        }
                    }
                    if (err == true)
                    {
                        resp.Message.Add("No se ha encontrado alguno/s de los planes ingresados");
                        resp.Obj = "Error";
                        resp.Success = false;
                        resp.CodStatus = HttpStatusCode.BadRequest;
                        return resp;
                    }
                }
                var content = _mapper.Map<Content>(dto);
                content.AddedDate = DateTime.Now;

                if ((int)content.Type == 2) // Imagen
                {
                    var aux1 = content.Dato;
                    content.Dato = "";
                    ImageDto dtoImgCont = new(aux1, dto.Title + " Content Image", "ContenidoIMG");
                    var urlContentImg = await _imagePost.postImage(dtoImgCont);
                    content.Dato = urlContentImg;
                }
                if ((int)content.Type == 4) // Audio
                {
                    var aux1 = content.Dato;
                    content.Dato = "";
                    ImageDto dtoAudioCont = new(aux1, dto.NickName + "Content Audio", "ContenidoAUD");
                    var urlContentAudio = await _imagePost.postImage(dtoAudioCont);
                    content.Dato = urlContentAudio;
                }

                content.ContentPlans = new List<ContentPlan>();
                content.ContentTags = new List<ContentTag>();

                _context.Contents.Add(content);
                await _context.SaveChangesAsync();

                if (dto.Plans != null)
                {
                    foreach (var planId in dto.Plans)
                    {
                        var pl = _context.Plans.Where(p => p.Id == planId).FirstOrDefault();
                        var contentPlan = new ContentPlan
                        {
                            IdPlan = planId,
                            IdContent = content.Id
                        };
                        content.ContentPlans.Add(contentPlan);
                        await _context.SaveChangesAsync();
                        pl.ContentPlans.Add(contentPlan);
                        await _context.SaveChangesAsync();
                    }
                }
                
                if (dto.Tags != null)
                {
                    foreach (var t in dto.Tags)
                    {
                        var tag = _mapper.Map<Tag>(t);
                        var findTag = await _context.Tags.Where(x => x.Name == tag.Name).FirstOrDefaultAsync();

                        if (findTag != null)
                        {
                            tag.Id = findTag.Id;
                        }
                        else
                        {
                            _context.Tags.Add(tag);
                            await _context.SaveChangesAsync();
                        }

                        var tagContent = new ContentTag
                        {
                            IdTag = tag.Id,
                            IdContent = content.Id
                        };
                        _context.ContentTags.Add(tagContent);
                        await _context.SaveChangesAsync();
                        content.ContentTags.Add(tagContent);
                        tag.ContentTags.Add(tagContent);
                        await _context.SaveChangesAsync();
                    }
                }

                resp.CodStatus = HttpStatusCode.Created;
                resp.Success = true;
                resp.Obj = dto.NickName + " Tu nuevo contenido ha sido creado con exito";
                resp.Message.Add("Exito");
                return resp;
            }
        }
    }
}
