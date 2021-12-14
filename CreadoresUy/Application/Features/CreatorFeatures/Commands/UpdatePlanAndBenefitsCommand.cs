using Application.Features.Validators;
using Application.Interface;
using Application.Service;
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

namespace Application.Features.CreatorFeatures.Commands
{
    public class UpdatePlanAndBenefitsCommand :IRequest<Response<string>>
    {
        public string Nickname {  get; set; }
        public UpdatePlanAndBenefitsDto PandB { get; set; }
        public class UpdatePlanAndBenefitsCommandHandler : IRequestHandler<UpdatePlanAndBenefitsCommand, Response<string>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly ImagePostService _imagePost;
            public UpdatePlanAndBenefitsCommandHandler(ICreadoresUyDbContext context, ImagePostService imagePost)
            {
                _context = context;
                _imagePost = imagePost;
            }

            public async Task<Response<string>> Handle(UpdatePlanAndBenefitsCommand command, CancellationToken cancellationToken)
            {
                var res = new Response<string>()
                {
                    Message = new List<string>()
                };
                var cre = _context.Creators.Where(c => c.NickName == command.Nickname)
                                            .Include(c => c.Plans).ThenInclude(pl => pl.Benefits).FirstOrDefault();
                var dto = command.PandB;
                //Validacion 
                var validador = new UpdatePlanAndBenefitsValidator(_context, command.Nickname);
                ValidationResult result = validador.Validate(dto);
                //Validacion invalida
                if (!result.IsValid || cre == null)
                {
                    if (cre == null) res.Message.Add("No se ha encontrado el Nickname ingresado");
                    if (!result.IsValid)
                    {
                        foreach (var er in result.Errors)
                        {
                            res.Message.Add(er.ErrorMessage);
                        }
                    }
                    res.CodStatus = HttpStatusCode.BadRequest;
                    res.Success = false;
                    res.Obj = "Error";
                    return res;
                }

                foreach(var plan in cre.Plans)
                {
                    if (plan.Id == dto.IdPlan)
                    {
                        plan.Name = dto.Name;
                        plan.Price = dto.Price;
                        plan.SubscriptionMsg = dto.SubscriptionMsg;
                        plan.Description = dto.Description;
                        plan.WelcomeVideoLink = dto.WelcomeVideoLink;
                        if (dto.Image != string.Empty)
                        {
                            ImageDto dtoImgPlan = new(dto.Image, plan.Name + " photo by " + cre.NickName, "Planes");
                            var urlPlanImg = await _imagePost.postImage(dtoImgPlan);
                            plan.Image = urlPlanImg;
                        }
                        await _context.SaveChangesAsync();
                        foreach (var be in plan.Benefits)
                        {
                            plan.Benefits.Remove(be);
                            _context.Benefits.Remove(be);
                            await _context.SaveChangesAsync();
                        }
                        foreach (var b in dto.Benefits)
                        {
                            var ben = new Benefit(b, plan.Id, plan);
                            _context.Benefits.Add(ben);
                            await _context.SaveChangesAsync();
                            plan.Benefits.Add(ben);
                            await _context.SaveChangesAsync();
                        }
                        res.CodStatus = HttpStatusCode.OK;
                        res.Success = true;
                        res.Obj = "Tus planes ha sido actualizado con Exito";
                        res.Message.Add("Exito");
                    }
                }
                return res;
            }
        }
    }
}
