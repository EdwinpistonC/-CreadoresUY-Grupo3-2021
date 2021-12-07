using Application.Features.Validators;
using Application.Interface;
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

namespace Application.Features.UserFeatures.Commands
{
    public class SubscribeToCommand : IRequest<Response<string>>
    {
        public SubscribeToDto dto { get; set; }
        public class SubscribeToCommandHandler : IRequestHandler<SubscribeToCommand, Response<string>>
        {
            private readonly ICreadoresUyDbContext _context;
            public SubscribeToCommandHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }
            public async Task<Response<string>> Handle(SubscribeToCommand request, CancellationToken cancellationToken)
            {
                var resp = new Response<string>() { 
                    Message = new List<string>()
                };
                var dto = request.dto;
                var validador = new SubscribeToCommandValidator(_context, dto.NickName);
                ValidationResult result = validador.Validate(request.dto);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        resp.Message.Add(error.ErrorMessage);
                    }
                    resp.CodStatus = HttpStatusCode.BadRequest;
                    resp.Success = false;
                    resp.Obj = "Error";
                    return resp;
                }
                var cre = _context.Creators.Where(c => c.NickName == dto.NickName).Include(c => c.Plans)
                    .ThenInclude(p => p.UserPlans).FirstOrDefault();
                var usr = _context.Users.Where(u => u.Id == dto.IdUser).FirstOrDefault();
                var plan = cre.Plans.Where(p => p.Id == dto.IdPlan).FirstOrDefault();
                bool resultado = CambiarPlan(_context, cre, plan, usr);
                
                var userp = new UserPlan(dto.IdPlan, dto.IdUser, DateTime.Now)
                {
                    Plan = plan
                };
                _context.UserPlans.Add(userp);
                await _context.SaveChangesAsync();
                plan.UserPlans.Add(userp);
                var payment = new Payment(dto.ExternalPaymentId, dto.NickName)
                {
                    IdUser = dto.IdUser,
                    UserPlanId = userp.IdPlan,
                    UserPlan = userp
                };
                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();
                userp.Payments.Add(payment);
                await _context.SaveChangesAsync();

                resp.Message.Add("Exito");
                resp.CodStatus = HttpStatusCode.OK;
                resp.Success = true;
                resp.Obj = "Te has suscripto exitosamente al plan: "+plan.Name;
                return resp;
            }

            public bool CambiarPlan(ICreadoresUyDbContext _context, Creator creator, Plan pl, User user)
            {
                foreach(var plan in creator.Plans)
                {
                    foreach(var us in pl.UserPlans)
                    {
                        if(us.IdUser == user.Id && us.IdPlan != pl.Id)
                        {
                            
                        }
                    }
                }

                return true;
            }
        }
    }
}
