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
    public class SubscribeToCommandValidator : AbstractValidator<SubscribeToDto>
    {
        private readonly ICreadoresUyDbContext _context;
        public string Nickname { get; set; }
        public SubscribeToCommandValidator(ICreadoresUyDbContext context, string nickname)
        {
            _context = context;
            Nickname = nickname;
            RuleFor(x => x.IdUser).NotEqual(0).WithMessage("{PropertyName} -> {PropertyValue} no es un id Valido")
                .Must(ExisteUser).WithMessage("{PropertyValue} -> no se ha encontrado el usuario");
            RuleFor(x => x.NickName).NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
                .Must(ExisteCreador).WithMessage("{PropertyValue} -> no se ha encontrado el creador");
            RuleFor(x => x.IdPlan).NotEqual(0).WithMessage("{PropertyName} -> {PropertyValue} no es un id Valido")
                .Must(PlanValido).WithMessage("{PropertyValue} -> no se ha encontrado el Plan o no esta relacionado al creador ingresado");
            RuleFor(x => x.ExternalPaymentId).NotEmpty().WithMessage("{PropertyName} no puede ser vacio");
        }

        public bool ExisteUser(int id)
        {
            var usr = _context.Users.Where(u => u.Id == id).FirstOrDefault();
            if(usr == null) return false;
            return true;
        }

        public bool ExisteCreador(string nickname)
        {
            var cre = _context.Creators.Where(c => c.NickName == nickname).FirstOrDefault();
            if (cre == null) return false;
            return true;
        }

        public bool PlanValido(int IdPlan)
        {
            var cre = _context.Creators.Where(c => c.NickName == Nickname).Include(c => c.Plans).FirstOrDefault();
            if (cre != null)
            {
                foreach (var plan in cre.Plans)
                {
                    if (plan.Id == IdPlan) return true;
                }
                return false;
            }
            return false;
        }
    }
}
