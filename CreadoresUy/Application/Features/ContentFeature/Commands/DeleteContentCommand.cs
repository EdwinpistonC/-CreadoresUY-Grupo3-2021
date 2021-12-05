using Application.Interface;
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
    public class DeleteContentCommand : IRequest<Response<string>>
    {
        public int IdCreator { get; set; }
        public int IdContent { get; set; }

        public class DeleteContentCommandHandler : IRequestHandler<DeleteContentCommand, Response<string>>
        {
            private readonly ICreadoresUyDbContext _context;

            public DeleteContentCommandHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }

            public async Task<Response<string>> Handle(DeleteContentCommand request, CancellationToken cancellationToken)
            {
                var res = new Response<string>()
                {
                    Message = new List<string>()
                };

                var cre = _context.Creators.Where(c => c.Id == request.IdCreator).Include(x => x.Plans)
                    .ThenInclude(p => p.ContentPlans).ThenInclude(p => p.Content)
                    .ThenInclude(p => p.ContentTags).ThenInclude(ct => ct.Tag).FirstOrDefault();

                var contentAux = new Content();
                bool encontre = false;
                if (cre != null) { 
                    foreach (var pl in cre.Plans)
                    {
                        foreach (var contp in pl.ContentPlans)
                        {
                            if (contp.Content.Id == request.IdContent && contp.Content.Deleted == false)
                            {
                                contentAux = contp.Content;
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
                    if (encontre == false) res.Message.Add("Error");
                    res.Obj = "No se ha encontrado un contenido con el Id ingresado";
                }
                else
                {
                    //Remuevo los links 
                    if (contentAux.ContentPlans != null)
                    {
                        foreach (var cpl in contentAux.ContentPlans)
                        {
                            await RemoveContentPlan(cpl, contentAux, _context);
                        }
                    }
                    if (contentAux.ContentTags != null)
                    {
                        foreach (var ctg in contentAux.ContentTags)
                        {
                            await RemoveContentTag(ctg, contentAux, _context);
                        }
                    }
                    contentAux.Deleted = true;
                    await _context.SaveChangesAsync();
                    res.Success = true;
                    res.CodStatus = HttpStatusCode.OK;
                    res.Message.Add("Exito");
                    res.Obj = "Se ha eliminado el Contenido correctamente";
                }
                return res;

            }

            //Funciones auxiliares
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
        }
    }
}
