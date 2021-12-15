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
    public class PlansAndBenefitsValidator : AbstractValidator<PlanAndBenefitsDto>
    {
        private readonly ICreadoresUyDbContext _context;
        public string Nickname;
        public PlansAndBenefitsValidator(ICreadoresUyDbContext context, string nickname)
        {
            _context = context;
            Nickname = nickname;
            RuleFor(x => x.Name).NotEmpty().WithMessage("{PropertyName} No puede ser vacio")
                .Must(ExistePlan).WithMessage("{PropertyName} = {PropertyValue} -> no puede ser ingresado porque ya existe un plan con el mismo nombre");
            RuleFor(x => x.Price).NotEmpty().WithMessage("{PropertyName} No puede ser vacio");
            RuleFor(x => x.Benefits).Must(Validar).WithMessage("{PropertyName} Debe contener al menos un Beneficio");
        }
        public bool Validar(ICollection<string> dato)
        {
            if (dato.Count == 0) return false;

            return true;
        }

        public bool ExistePlan(string name)
        {
            var cre = _context.Creators.Where(c => c.NickName == Nickname)
                                            .Include(c => c.Plans).FirstOrDefault();
            if(Nickname != string.Empty) { 
                if(cre != null)
                {
                    foreach(var plan in cre.Plans)
                    {
                        if (plan.Name == name) return false;
                    }
                    return true;
                }
                return true;
            }
            return true;
        }
    }
}
