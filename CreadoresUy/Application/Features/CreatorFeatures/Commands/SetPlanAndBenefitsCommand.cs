using Application.Features.Validators;
using Application.Interface;
using Application.Service;
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

namespace Application.Features.CreatorFeatures.Commands
{
    public class SetPlanAndBenefitsCommand : IRequest<Response<string>>
    {
        public string Nickname { get; set; }
        public PlanAndBenefitsDto PandB { get; set; }
        public class SetPlanAndBenefitsCommandHandler : IRequestHandler<SetPlanAndBenefitsCommand, Response<string>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly ImagePostService _imagePost;
            public SetPlanAndBenefitsCommandHandler(ICreadoresUyDbContext context, ImagePostService imagePost)
            {
                _context = context;
                _imagePost = imagePost;
            }
            public async Task<Response<string>> Handle(SetPlanAndBenefitsCommand command, CancellationToken cancellationToken)
            {
                var res = new Response<string>()
                {
                    Message = new List<string>()
                };
                var pl = command.PandB;
                var cre = _context.Creators.Where(c => c.NickName == command.Nickname)
                                            .Include(c => c.Plans).FirstOrDefault();
                //Validacion 
                var validador = new PlansAndBenefitsValidator(_context, command.Nickname);
                ValidationResult result = validador.Validate(pl);

                //Validacion invalida
                if (!result.IsValid || cre == null)
                {
                    if (!result.IsValid)
                    {
                        foreach (var er in result.Errors)
                        {
                            res.Message.Add(er.ErrorMessage);
                        }
                    }
                    if (cre == null) res.Message.Add("No se ha encontrado el Nickname ingresado");
                    res.CodStatus = HttpStatusCode.BadRequest;
                    res.Success = false;
                    res.Obj = "Error";
                    return res;
                }

                //Validacion valida (OK)
                //Almacenamiento externo de imagen en FIREBASE
                string urlPlanImg; 
                if(pl.Image != string.Empty)
                {
                    ImageDto dtoImgPlan = new(pl.Image, pl.Name + " photo by " + cre.NickName, "Planes");
                    urlPlanImg = await _imagePost.postImage(dtoImgPlan);
                }
                else
                {
                    urlPlanImg = "https://firebasestorage.googleapis.com/v0/b/creadoresuy-674c1.appspot.com/o/Planes%2Fdefault%20plan.jpg?alt=media&token=19d2c351-4f92-406f-a794-0a5bb156a8ac";
                }
                
                var plan = new Plan(pl.Name, pl.Description, pl.Price, urlPlanImg, pl.SubscriptionMsg, pl.WelcomeVideoLink, cre.Id, cre);
                _context.Plans.Add(plan);
                await _context.SaveChangesAsync();
                foreach (var b in pl.Benefits)
                {
                    var ben = new Benefit(b, plan.Id, plan);
                    _context.Benefits.Add(ben);
                    await _context.SaveChangesAsync();
                    plan.Benefits.Add(ben);
                }
                cre.Plans.Add(plan);
                await _context.SaveChangesAsync();

                res.CodStatus = HttpStatusCode.OK;
                res.Success = true;
                res.Obj = "Tu nuevo plan ha sido registrado con Exito";
                res.Message.Add("Exito");
                return res;
            }
        }

    }
}


