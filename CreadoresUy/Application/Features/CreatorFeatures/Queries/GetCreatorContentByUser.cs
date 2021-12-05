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
                    var cre = _context.Creators.Where(c => c.NickName == query.Nickname)
                                .Include(c => c.Plans).FirstOrDefault();
                    if ( cre == null)
                    {   if (cre == null) res.Message.Add("No se ha encontrado el nickname ingresado");
                        res.Success = false;
                        res.CodStatus = HttpStatusCode.BadRequest;
                        userpc.FixIsNull();
                    }

                    if ( cre != null)
                    {
                        List<ContentAndBoolDto> contenidos = new();
                        foreach (var pl in cre.Plans)
                        {
                            bool authorized = true;
                            var plan = _context.Plans.Where(p => p.Id == pl.Id)
                           .Include(p => p.UserPlans).Include(p => p.ContentPlans)
                           .ThenInclude(p => p.Content).ThenInclude(c => c.ContentTags).ThenInclude(t => t.Tag).FirstOrDefault();

                            foreach (var contp in plan.ContentPlans)
                            {
                                var content = contp.Content;
                                var dtoplan = _mapper.Map<ContentDto>(content);
                                dtoplan.Tags = new List<TagDto>();
                                dtoplan.Plans = new List<int>();
                                foreach(var tag in content.ContentTags)
                                {
                                    var a = _mapper.Map<TagDto>(tag.Tag);
                                    dtoplan.Tags.Add(a);
                                }
                                foreach(var p in content.ContentPlans)
                                {
                                    dtoplan.Plans.Add(p.IdPlan);
                                }
                                dtoplan.IdCreator = cre.Id;
                                dtoplan.NickName = cre.NickName;
                                dtoplan.NoNulls();
                                if (authorized == false) dtoplan.ReduceContent();
                                ContentAndBoolDto dto = new(dtoplan, authorized);
                                contenidos.Add(dto);
                                
                            }
                        }
                        contenidos = contenidos.OrderByDescending(c => c.Content.AddedDate).ToList();//ordeno la lista desc por fecha 

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
                    var cre = _context.Creators.Where(c => c.NickName == query.Nickname)
                                .Include(c => c.Plans).FirstOrDefault();
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
                        foreach (var pl in cre.Plans)
                        {
                            bool authorized = false;
                            var plan = _context.Plans.Where(p => p.Id == pl.Id)
                            .Include(p => p.UserPlans).Include(p => p.ContentPlans)
                            .ThenInclude(p => p.Content).ThenInclude(c => c.ContentTags).ThenInclude(t => t.Tag).FirstOrDefault();
                            foreach (var usu in plan.UserPlans)
                            {
                                if (usu.IdUser == user.Id && usu.Deleted == false)
                                {
                                    authorized = true;
                                }
                            }
                            if (user.CreatorId == cre.Id) authorized = true;
                            bool EraFalse = false;
                            foreach (var contp in plan.ContentPlans)
                            {
                                var content = contp.Content;
                                var dtoplan = _mapper.Map<ContentDto>(content);
                                dtoplan.Tags = new List<TagDto>();
                                dtoplan.Plans = new List<int>();
                                foreach (var tag in content.ContentTags)
                                {
                                    var a = _mapper.Map<TagDto>(tag.Tag);
                                    dtoplan.Tags.Add(a);
                                }
                                foreach (var p in content.ContentPlans)
                                {
                                    dtoplan.Plans.Add(p.IdPlan);
                                }
                                dtoplan.IdCreator = cre.Id;
                                dtoplan.NickName = cre.NickName;
                                dtoplan.NoNulls();
                                if (dtoplan.Public == true)
                                {
                                    if(authorized == false) EraFalse = true;
                                    authorized = true;
                                }
                                if (authorized == false) dtoplan.ReduceContent();
                                ContentAndBoolDto dto = new(dtoplan, authorized);
                                contenidos.Add(dto);
                                if (EraFalse == true)
                                {
                                    authorized = false;
                                    EraFalse = false;
                                }
                            }
                        }
                        contenidos = contenidos.OrderByDescending(c => c.Content.AddedDate).ToList();//ordeno la lista desc por fecha 

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

        }

    }
}
