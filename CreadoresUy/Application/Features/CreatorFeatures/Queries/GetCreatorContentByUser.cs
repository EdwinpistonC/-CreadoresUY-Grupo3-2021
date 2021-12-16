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

namespace Application.Features.CreatorFeatures.Queries
{
    public class GetCreatorContentByUser : IRequest<Response<UserPlanAndContentsDto>>
    {
        public string Nickname {  get; set; }
        public int IdUser {  get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public class GetCreatorContentByUserHandler : IRequestHandler<GetCreatorContentByUser, Response<UserPlanAndContentsDto>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public GetCreatorContentByUserHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task <Response<UserPlanAndContentsDto>> Handle(GetCreatorContentByUser query, CancellationToken cancellation)
            {
                Response<UserPlanAndContentsDto> res = new();
                UserPlanAndContentsDto userpc = new();
                res.Message = new List<string>();
                if( query.Nickname == "")
                {
                    res.Message.Add("Datos invalidos");
                    res.Success = false;
                    res.CodStatus = HttpStatusCode.BadRequest;
                    userpc.FixIsNull();
                }

                if(query.IdUser == 0)
                {
                    var cre = _context.Creators.Include(c => c.Plans).Where(c => c.NickName == query.Nickname)
                                .FirstOrDefault();
                    if ( cre == null)
                    {   if (cre == null) res.Message.Add("No se ha encontrado el nickname ingresado");
                        res.Success = false;
                        res.CodStatus = HttpStatusCode.BadRequest;
                        userpc.FixIsNull();
                    }

                    if ( cre != null)
                    {
                        List<ContentAndBoolDto> contenidos = new();
                        var aux = new List<int>();
                        foreach (var pl in cre.Plans)
                        {
                            aux.Add(pl.Id);
                        }

                        var content1 = await _context.Contents.Include(c=> c.ContentTags).ThenInclude(t => t.Tag).Include(c => c.ContentPlans).ThenInclude(cp => cp.Plan).Include(c => c.ContentPlans)
                            .ThenInclude(cp => cp.Plan).ThenInclude(p => p.Creator).Where(c => c.ContentPlans.Any(cp => cp.Plan.Creator.Id == cre.Id) 
                            && c.ContentPlans.Any(cp => aux.Contains(cp.IdPlan))).ToListAsync();

                        bool authorized = true;
                        foreach (var content in content1)
                        {
                            if (content.Deleted == false && content.Draft == false && content.PublishDate.Date <= DateTime.Now.Date)
                            {
                                var dtoplan = GetDto(content, cre.Id, cre.NickName);
                                if (authorized == false) dtoplan.ReduceContent();
                                ContentAndBoolDto dto = new(dtoplan, authorized);
                                contenidos.Add(dto);
                            }
                        }
                        contenidos = contenidos.OrderByDescending(c => c.Content.PublishDate).ToList();//ordeno la lista desc por fecha 

                        // Paginado 
                        var reqPage = new RequestPageUser();
                        reqPage.RequestPageUser1(query.PageNumber, query.PageSize);
                        var skip = (reqPage.PageNumber - 1) * reqPage.PageSize;
                        List<ContentAndBoolDto> contenidosResult = new();
                        contenidosResult = contenidos.Skip(skip).Take(reqPage.PageSize).ToList();

                        userpc.Follower = false;
                        userpc.ContentsAndBool = contenidosResult; //guardo los contenidos del cre, con el bool usr auth
                        userpc.Results = contenidosResult.Count;
                        res.Message.Add("Exito");
                        res.Success = true;
                        res.CodStatus = HttpStatusCode.OK;
                    }

                    res.Obj = userpc;
                    return res;
                }
                else
                {
                    var user = _context.Users.Where(u => u.Id == query.IdUser).FirstOrDefault();
                    var cre = _context.Creators.Include(c => c.Plans).Where(c => c.NickName == query.Nickname).FirstOrDefault();
                    if (user == null || cre == null)
                    {
                        if (user == null) res.Message.Add("No se ha encontrado el iduser ingresado");
                        if (cre == null) res.Message.Add("No se ha encontrado el nickname ingresado");
                        res.Success = false;
                        res.CodStatus = HttpStatusCode.BadRequest;
                        userpc.FixIsNull();
                    }

                    if (user != null && cre != null)
                    {
                        List<ContentAndBoolDto> contenidos = new();
                        var aux = new List<int>();
                        foreach (var pl in cre.Plans)
                        {
                            bool authorized = false;
                            bool EraFalse = false;
                            var plan = _context.Plans.Where(p => p.Id == pl.Id)
                            .Include(p => p.UserPlans).Include(p => p.ContentPlans)
                            .ThenInclude(p => p.Content).ThenInclude(c => c.ContentTags).ThenInclude(t => t.Tag).FirstOrDefault();
                            foreach (var usu in plan.UserPlans)
                            {
                                if (usu.IdUser == user.Id && usu.Deleted == false && usu.ExpirationDate > DateTime.Today && !usu.Plan.Deleted)
                                {
                                    authorized = true;
                                }
                            }
                            if (user.CreatorId == cre.Id) authorized = true;
                            foreach (var contp in plan.ContentPlans)
                            {
                                var content = contp.Content;
                                if (content.Deleted == false && content.Draft == false && content.PublishDate.Date <= DateTime.Now.Date)
                                {
                                    if (aux.Contains(content.Id))
                                    {
                                        foreach(var item in contenidos)
                                        {
                                            if(item.Content.Id == content.Id)
                                            {
                                                var dtoaux = GetDto(content, cre.Id, cre.NickName);
                                                item.Content = dtoaux;
                                                if(item.Authorized == false && authorized == true) item.Authorized = authorized;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var dtoplan = GetDto(content, cre.Id, cre.NickName);
                                        if (dtoplan.IsPublic == true)
                                        {
                                            if (authorized == false) EraFalse = true;
                                            authorized = true;
                                        }
                                        if (authorized == false) dtoplan.ReduceContent();
                                        ContentAndBoolDto dto = new(dtoplan, authorized);
                                        contenidos.Add(dto);
                                        aux.Add(dtoplan.Id);
                                        if (EraFalse == true)
                                        {
                                            authorized = false;
                                            EraFalse = false;
                                        }
                                    }
                                }
                            }
                        }
                        contenidos = contenidos.OrderByDescending(c => c.Content.PublishDate).ToList();//ordeno la lista desc por fecha 

                        // Paginado 
                        var reqPage = new RequestPageUser();
                        reqPage.RequestPageUser1(query.PageNumber, query.PageSize);
                        var skip = (reqPage.PageNumber - 1) * reqPage.PageSize;
                        List<ContentAndBoolDto> contenidosResult = new();
                        contenidosResult = contenidos.Skip(skip).Take(reqPage.PageSize).ToList();

                        var siguiendo = _context.UserCreators.Where(u => u.IdUser == query.IdUser).
                            Where(c => c.IdCreator == cre.Id).FirstOrDefault();
                        if (siguiendo != null && siguiendo.Unfollow != true) userpc.Follower = true;
                        userpc.ContentsAndBool = contenidosResult; //guardo los contenidos del cre, con el bool usr auth
                        userpc.Results = contenidosResult.Count;
                        res.Message.Add("Exito");
                        res.Success = true;
                        res.CodStatus = HttpStatusCode.OK;
                    }

                    res.Obj = userpc;
                    return res;
                }
               
            }

            public ContentDto GetDto(Content content, int id, string name)
            {
                var dtoaux = _mapper.Map<ContentDto>(content);
                dtoaux.Tags = new List<TagDto>();
                dtoaux.Plans = new List<int>();
                foreach (var tag in content.ContentTags)
                {
                    var a = _mapper.Map<TagDto>(tag.Tag);
                    dtoaux.Tags.Add(a);
                }
                foreach (var p in content.ContentPlans)
                {
                    dtoaux.Plans.Add(p.IdPlan);
                }
                dtoaux.IdCreator = id;
                dtoaux.NickName = name;
                dtoaux.NoNulls();
                return dtoaux;
            }

        }

    }
}
