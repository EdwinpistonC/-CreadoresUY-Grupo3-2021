﻿using Application.Features.Validators;
using Application.Interface;
using Application.Service;
using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Share.Dtos;
using Share.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.CreatorFeatures.Queries
{
    public class GetCreatorPlansByNicknameQuery : IRequest<Response<List<UpdatePlanAndBenefitsDto>>>
    {
        public string Nickname { get; set; }
        public class GetCreatorPlansByNicknameQueryHandler : IRequestHandler<GetCreatorPlansByNicknameQuery, Response<List<UpdatePlanAndBenefitsDto>>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;
            public GetCreatorPlansByNicknameQueryHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Response<List<UpdatePlanAndBenefitsDto>>> Handle(GetCreatorPlansByNicknameQuery query, CancellationToken cancellationToken)
            {

                var respuesta = new Response<List<UpdatePlanAndBenefitsDto>>
                {
                    Message = new List<string>()
                };
                var creador = _context.Creators.Where(c => c.NickName == query.Nickname).Include(c => c.Plans).
                                    ThenInclude(p => p.Benefits).FirstOrDefault();
                var listresp = new List<UpdatePlanAndBenefitsDto>();
                if (creador == null)
                {
                    respuesta.Obj = listresp;
                    respuesta.CodStatus = HttpStatusCode.BadRequest;
                    respuesta.Success = false;
                    respuesta.Message.Add("Error - No se ha encontrado el Id ingresado");
                    return respuesta;
                }
                foreach (var item in creador.Plans)
                {
                    var pl = _mapper.Map<UpdatePlanAndBenefitsDto>(item);
                    pl.IdPlan = item.Id;
                    pl.Benefits = new List<string>();
                    foreach (var b in item.Benefits)
                    {
                        pl.Benefits.Add(b.Description);
                    }
                    listresp.Add(pl);
                }
                respuesta.Obj = listresp;
                respuesta.CodStatus = HttpStatusCode.OK;
                respuesta.Success = true;
                respuesta.Message.Add("Exito");
                return respuesta;

            }
        }
    }
}
