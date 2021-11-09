using Application.Interface;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Share.Dtos;
using Share.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Features.Validators
{
    public class CreatorUpdateCommandValidator : AbstractValidator<CreatorRawDto>
    {
        private readonly ICreadoresUyDbContext _context;
        public CreatorUpdateCommandValidator(ICreadoresUyDbContext context)
        {
            _context = context;



            When(creator => creator.UserId !=null, () =>
             {
             RuleFor(x => x.UserId).MustAsync(async (UserId,cancellation) => ( await IdValido(UserId) )).WithMessage("No se a encontrado el usuario ingresado o el Id es invalido")
                 .MustAsync(async (UserId, cancellation) => (await ExisteCreador(UserId))).WithMessage("El Id ya esta asociado a una cuenta de creador");
             });
            When(creator => creator.Category1 != null, () =>
            {
                RuleFor(x => x.Category1).Must(CategoriaValida).WithMessage("{PropertyName} Dato invalido");
            });
            When(creator => creator.Category2 != null, () =>
            {
                RuleFor(x => x.Category2).Must(CategoriaValida).WithMessage("{PropertyName} Dato invalido");
            });
            When(creator => creator.CreatorName != null, () =>
            {
                RuleFor(x => x.CreatorName).NotEmpty().WithMessage("{PropertyName} No puede ser vacio");
            });
            When(creator => creator.NickName != null, () =>
            {
                RuleFor(x => x.NickName).NotEmpty().WithMessage("{PropertyName} No puede ser vacio");
            });
            When(creator => creator.CreatorDescription != null, () =>
            {
                RuleFor(x => x.CreatorDescription).NotEmpty().WithMessage("{PropertyName} No puede ser vacio");
            });
            When(creator => creator.YoutubeLink != null, () =>
            {
                RuleFor(x => x.YoutubeLink).NotEmpty().WithMessage("{PropertyName} es un campo requerido");
            });

        }

        public async Task<bool> IdValido(int id)
        {
            var u = await _context.Users.Where(u => u.Id == id ).FirstOrDefaultAsync();
            if (u == null)  return false;
            return true;
        }
        public async Task<bool> ExisteCreador(int id)
        {
            var u = await _context.Users.Where(u => u.Id == id && u.CreatorId != null).FirstOrDefaultAsync();
            
            if (u != null)
            {
                return false;
            }
            return true;
            

        }
        public bool CategoriaValida(TipoCategory nam)
        {
            if (((int)nam) == 0 || nam.ToString() == "Arte" || nam.ToString() == "Comida" || nam.ToString() == "Trading" || nam.ToString() == "Música")
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
