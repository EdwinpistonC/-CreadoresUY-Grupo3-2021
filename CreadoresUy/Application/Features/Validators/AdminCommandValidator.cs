using Application.Interface;
using FluentValidation;
using Share.Dtos;
using Share.Entities;
using System.Linq;
namespace Application.Features.Validators
{
    public class AdminCommandValidator : AbstractValidator<User>
    {
        private readonly ICreadoresUyDbContext _context;

        public AdminCommandValidator(ICreadoresUyDbContext context)
        {
            _context = context;

            RuleFor(x => x.Name).NotEmpty().WithMessage("{PropertyName} No puede ser vacio")
                .MaximumLength(50).WithMessage("{PropertyName} Excede el limite de caracteres permitido {MaxLength} ");
            
            
            RuleFor(x => x.Password).NotEmpty().WithMessage("{PropertyName} No puede ser vacio")
                .MinimumLength(8).WithMessage("{PropertyName} Debe contener al menos de 8 caracteres");
        }


    }
}
