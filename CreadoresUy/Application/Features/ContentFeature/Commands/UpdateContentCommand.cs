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
    public class UpdateContentCommand : IRequest<Response<string>>
    {
        public ContentDto Content { get; set; }
        public class UpdateContentCommandHandler : IRequestHandler<UpdateContentCommand, Response<string>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;
            private readonly ImagePostService _imagePost;

            public UpdateContentCommandHandler(ICreadoresUyDbContext context, IMapper mapper, ImagePostService imagePost)
            {
                _context = context;
                _mapper = mapper;
                _imagePost = imagePost; 
            }

            public async Task<Response<string>> Handle(UpdateContentCommand request, CancellationToken cancellationToken)
            {
                var resp = new Response<string>()
                {
                    Message = new List<string>()
                };
                var dto = request.Content;
                
                var validator = new UpdateContentCommandValidator(_context, dto.NickName, dto.IdCreator, dto.Id, dto.IsPublic);
                ValidationResult result = validator.Validate(dto);
                if(dto.Draft == false) 
                { 
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
                //Si la validacion fue exitosa
                var cre = _context.Creators.Where(c => c.Id == dto.IdCreator && c.NickName == dto.NickName).Include(x => x.Plans)
                    .ThenInclude(p => p.ContentPlans).ThenInclude(p => p.Content)
                    .ThenInclude(p => p.ContentTags).ThenInclude(ct => ct.Tag).FirstOrDefault();
                var contentAux = new Content();
                foreach (var pl in cre.Plans)
                {
                    foreach (var contp in pl.ContentPlans)
                    {
                        if (contp.Content.Id == dto.Id)
                        {
                            contentAux = contp.Content;
                        }
                    }
                }

                //Verifico si hubo cambios en el contenido
                contentAux.AddedDate = DateTime.Today;
                if (contentAux.PublishDate != dto.PublishDate) contentAux.PublishDate = dto.PublishDate;
                if (contentAux.Type != dto.Type) contentAux.Type = dto.Type;
                if (contentAux.PublishDate.Date < DateTime.Today.Date) contentAux.PublishDate = DateTime.Today;
                if (contentAux.Title != dto.Title) contentAux.Title = dto.Title;
                if (contentAux.Description != dto.Description) contentAux.Description = dto.Description;
                if (contentAux.Draft != dto.Draft) contentAux.Draft = dto.Draft;
                if (contentAux.IsPublic != dto.IsPublic) contentAux.IsPublic = dto.IsPublic;
                if (contentAux.Dato != dto.Dato)
                {
                    if ((int)contentAux.Type == 2) //Imagen 
                    {
                        var aux1 = dto.Dato;
                        contentAux.Dato = "";
                        ImageDto dtoImgCont = new(aux1, contentAux.Title + " Content Image", "ContenidoIMG");
                        var urlContentImg = await _imagePost.postImage(dtoImgCont);
                        contentAux.Dato = urlContentImg;
                    }
                    else if ((int)contentAux.Type == 4) // Audio
                    {
                        var aux1 = dto.Dato;
                        contentAux.Dato = "";
                        ImageDto dtoAudioCont = new(aux1, contentAux.Title + "Content Audio", "ContenidoAUD");
                        var urlContentAudio = await _imagePost.postImage(dtoAudioCont);
                        contentAux.Dato = urlContentAudio;
                    }
                    else { 
                        contentAux.Dato = dto.Dato;
                    }
                }

                //Remuevo los links anteriores
                if (contentAux.ContentPlans != null)
                {
                    foreach (var cpl in contentAux.ContentPlans)
                    {
                        await RemoveContentPlan(cpl, contentAux, _context);
                    }
                }
                if(contentAux.ContentTags != null)
                {
                    foreach(var ctg in contentAux.ContentTags)
                    {
                        await RemoveContentTag(ctg, contentAux, _context);
                    }
                }

                await _context.SaveChangesAsync();

                //Actualizo los nuevos links
                if (dto.IsPublic != true)
                {
                    if (dto.Plans.Count == 1 && dto.Plans.Contains(0) == true)
                    {
                        var pl1 = cre.Plans.FirstOrDefault();
                        await AddContentPlan(pl1, contentAux, _context);
                    }
                    else
                    {
                        foreach (var planId in dto.Plans)
                        {
                            var pl = _context.Plans.Where(p => p.Id == planId && p.Deleted == false).FirstOrDefault();
                            await AddContentPlan(pl, contentAux, _context);
                        }
                    }
                }
                else
                {
                    var pl1 = cre.Plans.FirstOrDefault();
                    await AddContentPlan(pl1, contentAux, _context);
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
                        await AddContentTag(tag, contentAux, _context);
                    }
                }

                resp.CodStatus = HttpStatusCode.Created;
                resp.Success = true;
                resp.Obj = dto.NickName + " Tu nuevo contenido ha sido creado con exito";
                resp.Message.Add("Exito");
                return resp;

            }
            public async Task RemoveContentPlan(ContentPlan cpl, Content content, ICreadoresUyDbContext _context)
            {
                var pl = _context.Plans.Where(p => p.Id == cpl.IdPlan).FirstOrDefault();
                pl.ContentPlans.Remove(cpl);
                content.ContentPlans.Remove(cpl);
                _context.ContentPlans.Remove(cpl);
                await _context.SaveChangesAsync();
            }

            public async Task RemoveContentTag(ContentTag ctg, Content content, ICreadoresUyDbContext _context)
            {
                var tg = _context.Tags.Where(t => t.Id == ctg.IdTag).FirstOrDefault();
                tg.ContentTags.Remove(ctg);
                content.ContentTags.Remove(ctg);
                _context.ContentTags.Remove(ctg);
                await _context.SaveChangesAsync();
            }

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
