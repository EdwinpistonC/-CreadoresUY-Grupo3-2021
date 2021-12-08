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

namespace Application.Features.ContentFeature.Queries
{
    public class GetContentDraftQuery : IRequest<Response<ContentDto>>
    {
        public string Nickname {  get; set; }
        public class GetContentDraftQueryHandler : IRequestHandler<GetContentDraftQuery, Response<ContentDto>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public GetContentDraftQueryHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<ContentDto>> Handle(GetContentDraftQuery command, CancellationToken cancellationToken)
            {
                var res = new Response<ContentDto>()
                {
                    Message = new List<string>() { }
                };
                var cre = _context.Creators.Where(c => c.NickName == command.Nickname).Include(x => x.Plans)
                    .ThenInclude(p => p.ContentPlans).ThenInclude(p => p.Content).ThenInclude(c => c.ContentTags)
                    .ThenInclude(t => t.Tag).FirstOrDefault();
                var cont = new Content();
                var plaux = cre.Plans.FirstOrDefault();
                var dto = new ContentDto();
                bool encontre = false;
                if (cre != null)
                {
                    foreach (var pl in cre.Plans)
                    {
                        foreach (var contp in pl.ContentPlans)
                        {
                            if (contp.Content.Draft == true && contp.Content.Deleted != true)
                            {
                                cont = contp.Content;
                                encontre = true;
                            }
                        }
                    }
                }
                if (cre == null || encontre == false)
                {
                    res.Success = false;
                    res.CodStatus = HttpStatusCode.BadRequest;
                    if (cre == null) res.Message.Add("No se ha encontrado al creador ingresado");
                    if (encontre == false) res.Message.Add("El creador no tiene Borradores pendientes");
                    dto.NoNulls();
                    res.Obj = dto;
                }
                else
                {
                    dto = _mapper.Map<ContentDto>(cont);
                    dto.IdCreator = cre.Id;
                    dto.NickName = cre.NickName;
                    dto.Tags = new List<TagDto>();
                    dto.Plans = new List<int>();
                    if (cont.ContentTags != null) {
                        foreach (var tag in cont.ContentTags)
                        {
                            var a = _mapper.Map<TagDto>(tag.Tag);
                            dto.Tags.Add(a);
                        }
                    }
                    if (cont.ContentPlans != null) 
                    {
                        if (cont.ContentPlans.Count == 1)
                        {
                            var co = cont.ContentPlans.FirstOrDefault();
                            if(co.IdPlan == plaux.Id)
                            {
                                dto.Plans.Add(0);
                            }
                            else
                            {
                                dto.Plans.Add(co.IdPlan);
                            }
                        }
                        else
                        {
                            foreach (var p in cont.ContentPlans)
                            {
                                dto.Plans.Add(p.IdPlan);
                            }
                        }

                    }
                    res.Success = true;
                    res.CodStatus = HttpStatusCode.OK;
                    res.Message.Add("Exito");
                    dto.NoNulls();
                    res.Obj = dto;
                }
                return res;

            }
        }

    }
}
