using Application.Interface;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Share.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Validators
{
    public class UpdatePlanAndBenefitsValidator : AbstractValidator<UpdatePlanAndBenefitsDto>
    {
        private readonly ICreadoresUyDbContext _context;
        public string Nickname;
        public UpdatePlanAndBenefitsValidator(ICreadoresUyDbContext context, string nickname)
        {
            _context = context;
            Nickname = nickname;
            RuleFor(x => x.IdPlan).NotEqual(0).WithMessage("{PropertyValue} -> no es un id valido")
                .Must(ExistePlan).WithMessage("{PropertyName} = {PropertyValue} -> no puede ser actualizado porque no existe");
            RuleFor(x => x.Name).NotEmpty().WithMessage("{PropertyName} No puede ser vacio");               
            RuleFor(x => x.Price).NotNull().WithMessage("{PropertyName} No puede ser vacio");
            RuleFor(x => x.WelcomeVideoLink).NotEmpty().WithMessage("{PropertyName} No puede ser vacio");
            RuleFor(x => x.SubscriptionMsg).NotEmpty().WithMessage("{PropertyName} No puede ser vacio");
            RuleFor(x => x.Benefits).Must(Validar).WithMessage("{PropertyName} Debe contener al menos un Beneficio");
        }
        public bool Validar(ICollection<string> dato)
        {
            if (dato.Count == 0) return false;

            return true;
        }

        public bool ExistePlan(int id)
        {
            var cre = _context.Creators.Where(c => c.NickName == Nickname)
                                            .Include(c => c.Plans).FirstOrDefault();
            if (cre != null)
            {
                foreach (var plan in cre.Plans)
                {
                    if (plan.Id == id) return true; 
                }
                return false;
            }
            return true;
        }
    }
}
