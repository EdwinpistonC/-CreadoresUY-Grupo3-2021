using Application.Interface;
using FluentValidation;
using Share.Dtos;
using Share.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Validators
{
    public class CreateNewContentCommandValidator : AbstractValidator<ContentDto>
    {
        private readonly ICreadoresUyDbContext _context;
        public CreateNewContentCommandValidator(ICreadoresUyDbContext context)
        {
            _context = context;
            RuleFor(x => x.Dato).NotEmpty().WithMessage("{PropertyName} no puede ser vacio");
            RuleFor(x => x.Description).NotEmpty().WithMessage("{PropertyName} no puede ser vacio");
            RuleFor(x => x.IdCreator).Must(IsValid1).WithMessage("{PropertyName} No se ha encontrado el creador de id: {PropertyValue} o el id ingresado fue 0");
            RuleFor(x => x.NickName).NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
               .Must(IsValid3).WithMessage("{PropertyName} No se ha encontrado el creador con el Nickname: {PropertyValue} ");
            RuleFor(x => x.Plans).Must(IsValid).WithMessage("{PropertyName} no puede ser vacio");
            RuleFor(x => x.Title).NotEmpty().WithMessage("{PropertyName} no puede ser vacio");
            RuleFor(x => x.Type).Must(IsValid2).WithMessage("{PropertyName} no es un valido");
        }
        public bool IsValid(ICollection<int> col)
        {
            if (col.Count == 0) return false;
            return true;
        }
        public bool IsValid1(int id)
        {
            if (id == 0) return false;
            var cre = _context.Creators.Where(c => c.Id == id).FirstOrDefault();
            if(cre == null) return false;
            return true;
        }
        public bool IsValid2(TipoContent d)
        {
            if((int)d <= 0 || (int)d > 5) return false;
            return true;
        }
        public bool IsValid3(string ni)
        {
            if (ni != string.Empty)
            {
                var cre = _context.Creators.Where(c => c.NickName == ni).FirstOrDefault();
                if (cre == null) return false;
            }
            return true;
        }
    }
}
