using Application.Interface;
using FluentValidation;
using Share.Dtos;
using Share.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Features.Validators
{
    public class CreatorSignUpCommandValidator : AbstractValidator<CreatorDto>
    {
        private readonly ICreadoresUyDbContext _context;
        public CreatorSignUpCommandValidator(ICreadoresUyDbContext context)
        {
            _context = context;
            RuleFor(x => x.IdUser).NotEmpty().WithMessage("{PropertyName} es un campo requerido")
            .Must(IdValido).WithMessage("No se a encontrado el usuario ingresado o el Id es invalido")
            .Must(ExisteCreador).WithMessage("El Id ya esta asociado a una cuenta de creador");

            RuleFor(x => x.Category1).Must(CategoriaValida).WithMessage("{PropertyName} Dato invalido");
            RuleFor(x => x.Category2).Must(CategoriaValida).WithMessage("{PropertyName} Dato invalido");
            RuleFor(x => x.CreatorName).NotEmpty().WithMessage("{PropertyName} No puede ser vacio");
            RuleFor(x => x.NickName).NotEmpty().WithMessage("{PropertyName} No puede ser vacio");
            RuleFor(x => x.CreatorDescription).NotEmpty().WithMessage("{PropertyName} No puede ser vacio");
            RuleFor(x => x.Plans).NotEmpty().WithMessage("{PropertyName} es un campo requerido")
            .Must(MinimosValidos).WithMessage("{PropertyName} debe contener al menos 3 planes");
            RuleFor(x => x.YoutubeLink).NotEmpty().WithMessage("{PropertyName} es un campo requerido");
        }

        public bool IdValido(int id) 
        {
            var u = _context.Users.Where(u => u.Id == id).FirstOrDefault();
            if(u == null)
            {
                return false;
            }
            return true;
        }
        public bool ExisteCreador(int id)
        {
            var u = _context.Users.Where(u => u.Id == id).FirstOrDefault();
            if (IdValido(id))
            {
                if (u.CreatorId != null)
                {
                    return false;
                }
                return true;
            }
            return true;

        }
        public bool CategoriaValida(TipoCategory nam)
        {
            if (((int)nam)==0 || nam.ToString() == "Arte" || nam.ToString() == "Comida" || nam.ToString() == "Trading" || nam.ToString() == "Música")
            {
                return true;
            }
            return false;
        }

        private bool MinimosValidos(ICollection<BasePlanDto> plans)
        {
            if (plans.Count() < 3)
            {
                return false;
            }
            return true;
        }
    }
}
