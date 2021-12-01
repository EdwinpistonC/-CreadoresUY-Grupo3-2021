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

            RuleFor(x => x.CreatorName).NotEmpty().WithMessage("{PropertyName} No puede ser vacio");
            RuleFor(x => x.NickName).NotEmpty().WithMessage("{PropertyName} No puede ser vacio")
                .Must(NickNameValido).WithMessage("{PropertyName} No valido, ya existe un creador registrado");
            RuleFor(x => x.ContentDescription).NotEmpty().WithMessage("{PropertyName} No puede ser vacio");
            RuleFor(x => x.Biography).NotEmpty().WithMessage("{PropertyName} No puede ser vacio");
            RuleFor(x => x.YoutubeLink).NotEmpty().WithMessage("{PropertyName} es un campo requerido");
            RuleFor(x => x.Category1).Must(CategoriaValida).WithMessage("{PropertyName} No es una categoria Valida");
            RuleFor(x => x.Category2).Must(CategoriaValida).WithMessage("{PropertyName} No es una categoria Valida");
            //Validacion de datos bancarios
            RuleFor(x => x.InfoPago.NombreEntidadFinanciera).NotEmpty().WithMessage("{PropertyName} No pude ser vacio")
                .Must(EntidadFinancieraValida).WithMessage("{PropertyName} No es valida");
            RuleFor(x => x.InfoPago.NombreTitular).NotEmpty().WithMessage("{PropertyName} No pude ser vacio");
            RuleFor(x => x.InfoPago.NumeroDeCuenta).NotEmpty().WithMessage("{PropertyName} No pude ser vacio");

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


        public bool NickNameValido(string nickname)
        {
            var c = _context.Creators.Where(c => c.NickName == nickname).FirstOrDefault();
            if (c == null)
            {
                return true;
            }
            return false;
        }
        public bool EntidadFinancieraValida(string nombre)
        {
            var u = _context.FinancialEntities.Where(f => f.Name == nombre).FirstOrDefault();
            if (u == null)
            {
                return false;
            }
            return true;
        }

        public bool CategoriaValida(string nomb)
        {
            if(nomb != "") { 
            var c = _context.Categorys.Where(u => u.Name == nomb).FirstOrDefault();
            if (c != null)
            {
                return true;
            }
            return false;
            }
            return true;
        }
    }
}
