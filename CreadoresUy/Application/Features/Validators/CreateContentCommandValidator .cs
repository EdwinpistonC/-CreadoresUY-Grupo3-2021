using Application.Interface;
using FluentValidation;
using Share.Dtos;
using Share.Enums;
using System.Linq;
namespace Application.Features.Validators
{
    public class CreateContentCommandValidator : AbstractValidator<ContentDto>
    {
        private readonly ICreadoresUyDbContext _context;

        public CreateContentCommandValidator(ICreadoresUyDbContext context)
        {
            _context = context;

            RuleFor(x => x.Title).NotEmpty().WithMessage("{PropertyName} No puede ser vacio")
                .MaximumLength(50).WithMessage("{PropertyName} Excede el limite de caracteres permitido {MaxLength} ");

            RuleFor(x => x.Link).NotEmpty().When(x => x.Type == TipoContent.Link || x.Type == TipoContent.Audio || x.Type == TipoContent.Video).WithMessage("{PropertyName} No puede ser vacio");

            RuleFor(x => x.Plans).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} No puede ser vacio")
            .When(x=>x.Plans != null).Must(collection => collection != null && collection.All(e => e > 0)).WithMessage("{PropertyName} con valor mayor a 0");

            RuleFor(x => x.Tags).Must(collection => collection.All(item => item.Name != null)).WithMessage("{PropertyName} Tag sin valor ");

            RuleFor(x => x.Description).Must(description => description != null).WithMessage("{PropertyName} No puede ser vacio")
           .MaximumLength(20000).WithMessage("{PropertyName} Excede el limite de caracteres permitido {MaxLength} ");

        }
    }
}
