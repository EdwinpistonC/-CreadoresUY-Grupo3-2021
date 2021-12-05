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
                var val = new CreateNewContentCommandValidator(_context,dto.NickName, dto.IdCreator);
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
                var cre = _context.Creators.Where(c => c.Id == dto.IdCreator && c.NickName == dto.NickName).Include(x => x.Plans).ThenInclude(p => p.ContentPlans).ThenInclude(p => p.Content).FirstOrDefault();
                
                //Si Todo fue valido da de alta al contenido
                var content = _mapper.Map<Content>(dto);
                content.AddedDate = DateTime.Now;
                
                //Almacenamiento externo de Contendios de acuerdo al tipo
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
                    ImageDto dtoAudioCont = new(aux1, dto.Title + "Content Audio", "ContenidoAUD");
                    var urlContentAudio = await _imagePost.postImage(dtoAudioCont);
                    content.Dato = urlContentAudio;
                }
                content.ContentPlans = new List<ContentPlan>();
                content.ContentTags = new List<ContentTag>();
                _context.Contents.Add(content);
                await _context.SaveChangesAsync();
                
                if (dto.Public != true)
                {
                    foreach (var planId in dto.Plans)
                    {
                        var pl = _context.Plans.Where(p => p.Id == planId && p.Deleted == false).FirstOrDefault();
                        await AddContentPlan(pl, content, _context);
                    }
                }
                else
                {
                    var pl1 = cre.Plans.FirstOrDefault();
                    await AddContentPlan(pl1, content, _context);
                }
                                
                if (dto.Tags != null)
                {
                    foreach (var t in dto.Tags)
                    {
                        var tag = _mapper.Map<Tag>(t);
                        var findTag = await _context.Tags.Where(x => x.Name == tag.Name).FirstOrDefaultAsync();
                        if (findTag != null) tag = findTag;
                        else
                        {
                            _context.Tags.Add(tag);
                            await _context.SaveChangesAsync();
                        }
                        await AddContentTag(tag, content, _context);
                    }
                }

                resp.CodStatus = HttpStatusCode.Created;
                resp.Success = true;
                resp.Obj = dto.NickName + " Tu nuevo contenido ha sido creado con exito";
                resp.Message.Add("Exito");
                return resp;
            }

            //FUNCIONES AUXILIARES (para tener el codigo mas limpio)
            public async Task AddContentPlan(Plan pl, Content content, ICreadoresUyDbContext _context)
            {
                var contentPlan = new ContentPlan
                {
                    IdPlan = pl.Id,
                    IdContent = content.Id
                };

                _context.ContentPlans.Add(contentPlan);
                await _context.SaveChangesAsync();
                contentPlan.Plan = pl;
                contentPlan.Content = content;
                content.ContentPlans.Add(contentPlan);
                pl.ContentPlans.Add(contentPlan);
                await _context.SaveChangesAsync();
            }

            public async Task AddContentTag(Tag tag, Content content, ICreadoresUyDbContext _context)
            {
                var tagContent = new ContentTag
                {
                    IdTag = tag.Id,
                    IdContent = content.Id
                };
                tagContent.Tag = tag;
                _context.ContentTags.Add(tagContent);
                await _context.SaveChangesAsync();
                content.ContentTags.Add(tagContent);
                tag.ContentTags.Add(tagContent);
                await _context.SaveChangesAsync();
            }
        }
    }
}
