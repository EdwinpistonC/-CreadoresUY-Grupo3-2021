using Application.Interface;
using FluentValidation;
using Share.Dtos;
using System.Linq;
namespace Application.Features.Validators
{
    public class UserSignUpCommandValidator : AbstractValidator<CreateUserDto>
    {
        private readonly ICreadoresUyDbContext _context;

        public UserSignUpCommandValidator(ICreadoresUyDbContext context)
        {
            _context = context;

            RuleFor(x => x.Name).NotEmpty().WithMessage("{PropertyName} No puede ser vacio")
                .MaximumLength(50).WithMessage("{PropertyName} Excede el limite de caracteres permitido {MaxLength} ");
            
            RuleFor(x => x.Email).NotEmpty().WithMessage("{PropertyName} No puede ser vacio")
                .EmailAddress().WithMessage("{PropertyName} Debe ser una direccion valida")
                .Must(ExisteCorreo).WithMessage("Ya existe un usuario ingresado en el sistema con el mismo Email");
            
            RuleFor(x => x.Password).NotEmpty().WithMessage("{PropertyName} No puede ser vacio")
                .MinimumLength(8).WithMessage("{PropertyName} Debe contener al menos de 8 caracteres");
        }

        public bool ExisteCorreo(string correo)
        {
            var u =_context.Users.Where(u => u.Email == correo).FirstOrDefault();
            if(u != null)
            {
                return false;
            }
            return true;
        }
    }
}
