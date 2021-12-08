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
                var usr = _context.Users.Where(u => u.Id == dto.IdUser).Include(u => u.UserPlans).FirstOrDefault();
                var plan = cre.Plans.Where(p => p.Id == dto.IdPlan).FirstOrDefault();
                
                bool resultado = ExisteSuscripcion(_context, cre, usr);
                
                if (resultado == false)
                {
                    await CrearSuscripcion(_context, dto, plan, usr);
                }
                else
                {
                    await ActualizarSuscripcion(_context, dto, plan, usr,cre);
                }
                resp.Message.Add("Exito");
                resp.CodStatus = HttpStatusCode.OK;
                resp.Success = true;
                resp.Obj = "Te has suscripto exitosamente al plan: "+plan.Name;
                return resp;

            }



            //Funciones Externas 
            public async Task CrearSuscripcion(ICreadoresUyDbContext _context, SubscribeToDto dto, Plan plan, User usr)
            {
                var userp = new UserPlan(dto.IdPlan, dto.IdUser, DateTime.Now)
                {
                    Plan = plan,
                    User = usr
                };
                _context.UserPlans.Add(userp);
                plan.UserPlans.Add(userp);
                await _context.SaveChangesAsync();

                await CrearPago(_context, dto, userp, plan.Name);
                
            }

            public async Task CrearPago(ICreadoresUyDbContext _context, SubscribeToDto dto, UserPlan userp, string nomp)
            {
                var payment = new Payment(dto.ExternalPaymentId, dto.NickName, dto.PaymentAmount, userp.IdUser, userp.IdPlan);
                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();
                userp.Payments.Add(payment);
                await _context.SaveChangesAsync();
                //Pago al creador y retencion de la plataforma
                var cre = _context.Creators.Where(p => p.NickName == dto.NickName).FirstOrDefault();
                double plataforma = dto.PaymentAmount * 0.15; //Retencion plataforma 
                double creador = dto.PaymentAmount - plataforma; //Ingreso para el creador
                var pagoPlat = new PagoPlataforma(plataforma, payment.Id);
                var pagoCre = new PagoCreador(creador, cre.Id ,cre.NickName, nomp, payment.Id);
                _context.PagosCreador.Add(pagoCre);
                _context.PagosPlataforma.Add(pagoPlat);
                await _context.SaveChangesAsync();
            }
            public bool ExisteSuscripcion(ICreadoresUyDbContext _context, Creator creator, User user)
            {
                foreach(var plan in creator.Plans)
                {
                    foreach(var us in plan.UserPlans)
                    {
                        if(us.IdUser == user.Id )
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

            public async Task ActualizarSuscripcion(ICreadoresUyDbContext _context, SubscribeToDto dto, Plan plan, User usr, Creator cre)
            {
                var userp = new UserPlan();
                bool encontre = false;
                foreach (var pl in cre.Plans)
                {
                    foreach (var us in pl.UserPlans)
                    {
                        if (us.IdUser == usr.Id)
                        {
                            if(us.IdPlan == plan.Id)
                            {
                                userp = us;
                                encontre = true;
                            }

                        }
                    }
                }
                if(encontre == true) //Tiene una suscripcion que expiro y la quiere renovar
                {
                    userp.Deleted = false;
                    userp.SusbscriptionDate = DateTime.Now;
                    userp.ExpirationDate = DateTime.Now.AddDays(30);
                    await _context.SaveChangesAsync();
                    await CrearPago(_context, dto, userp, plan.Name);
                }
                else //Tiene una suscripcion que quiere cambiar o que expiro y quiere cambiar 
                {
                    await CrearSuscripcion(_context, dto, plan, usr);
                }
            }

        }
    }
}
