using Application.Interface;
using AutoMapper;
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
    public class CreateDraftContentCommand : IRequest<Response<ContentDto>>
    {
        public ContentDto Content { get; set; }
        public class CreateDraftContentCommandHandler : IRequestHandler<CreateDraftContentCommand, Response<ContentDto>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public CreateDraftContentCommandHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<ContentDto>> Handle(CreateDraftContentCommand command, CancellationToken cancellationToken)
            {
                var resp = new Response<ContentDto>()
                {
                    Message = new List<string>()
                };
                var dto = command.Content;
                var cre = _context.Creators.Where(c => c.Id == dto.IdCreator && c.NickName == dto.NickName).Include(x => x.Plans).ThenInclude(p => p.ContentPlans).ThenInclude(p => p.Content).FirstOrDefault();
                bool b = ExisteDraft(cre);
                if (cre == null || b == true)
                {
                    if(b == true) resp.Message.Add("Se ha encontrado un borrador pendiente");
                    if(cre == null) resp.Message.Add("No se ha encontrado al creador ingresado");
                    dto.NoNulls();
                    resp.Obj = dto;
                    resp.Success = false;
                    resp.CodStatus = HttpStatusCode.BadRequest;
                    return resp;
                }
                var content = _mapper.Map<Content>(dto);
                content.AddedDate = DateTime.Now;
                content.ContentPlans = new List<ContentPlan>();
                content.ContentTags = new List<ContentTag>();
                _context.Contents.Add(content);
                await _context.SaveChangesAsync();

                var pl1 = cre.Plans.FirstOrDefault();
                await AddContentPlan(pl1, content, _context);

                var dtores = _mapper.Map<ContentDto>(content);
                dtores.IdCreator = dto.IdCreator;
                dtores.NickName = dto.NickName;
                dtores.Plans = new List<int>();
                dtores.Tags = new List<TagDto>();
                if (content.ContentTags != null)
                {
                    foreach (var tag in content.ContentTags)
                    {
                        var a = _mapper.Map<TagDto>(tag.Tag);
                        dtores.Tags.Add(a);
                    }
                }
                if (content.ContentPlans != null)
                {
                    dtores.Plans.Add(0);
                    /*foreach (var p in content.ContentPlans)
                    {
                        dtores.Plans.Add(p.IdPlan);
                    }*/
                }
                dtores.NoNulls();
                resp.CodStatus = HttpStatusCode.Created;
                resp.Success = true;
                resp.Obj = dtores;
                resp.Message.Add("Exito");
                return resp;

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
                await _context.SaveChangesAsync();
            }

            public bool ExisteDraft(Creator cre)
            {
                if (cre != null)
                {
                    foreach (var pl in cre.Plans)
                    {
                        foreach (var contp in pl.ContentPlans)
                        {
                            if (contp.Content.Draft == true && contp.Content.Deleted == false)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
        }
    }
}
