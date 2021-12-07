using Application.Interface;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Share.Dtos;
using Share.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Validators
{
    public class UpdateContentCommandValidator : AbstractValidator<ContentDto>
    {
        private readonly ICreadoresUyDbContext _context;
        public string Nickname { get; set; }
        public int IdCre { get; set; }
        public int IdContent {  get; set; }
        public bool Publico { get; set; }

        public UpdateContentCommandValidator(ICreadoresUyDbContext context, string nickname, int idCre, int idc, bool publico)
        {
            _context = context;
            Nickname = nickname;
            IdCre = idCre;
            IdContent = idc;
            Publico = publico;

            RuleFor(x => x.Id).Must(IdContentValido).WithMessage("{PropertyName} el Id ingresado no fue encontrado en relacion al creador o ingreso Id 0");
            RuleFor(x => x.Dato).NotEmpty().WithMessage("{PropertyName} no puede ser vacio");
            RuleFor(x => x.Description).NotEmpty().WithMessage("{PropertyName} no puede ser vacio");
            RuleFor(x => x.IdCreator).Must(IsValid1).WithMessage("{PropertyName} No se ha encontrado el creador de id: {PropertyValue} o el id ingresado fue 0")
                .Must(IdNicknameCorresponden).WithMessage("El Id ingresado no corresponde al Nickname ingresado");
            RuleFor(x => x.NickName).NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
               .Must(IsValid3).WithMessage("{PropertyName} No se ha encontrado el creador con el Nickname: {PropertyValue} ");
            RuleFor(x => x.Plans).Must(IsValid).WithMessage("{PropertyName} no puede ser vacio")
                .Must(CorrespondePlan).WithMessage("No se ha encontrado relacion con el creador de alguno/s de los planes ingresados  ");
            RuleFor(x => x.Title).NotEmpty().WithMessage("{PropertyName} no puede ser vacio");
            RuleFor(x => x.Type).Must(IsValid2).WithMessage("{PropertyName} no es un valido");
            RuleFor(x => x.Draft).Must(ExisteDraft).WithMessage("Ya existe un contenido almacenado como Borrador");
        }
        public bool IdContentValido(int id)
        {
            var cre = _context.Creators.Where(c => c.Id == IdCre && c.NickName == Nickname).Include(x => x.Plans).ThenInclude(p => p.ContentPlans).ThenInclude(p => p.Content).FirstOrDefault();
            foreach(var pl in cre.Plans)
            {
                foreach (var contp in pl.ContentPlans)
                {
                    if (contp.Content.Id == id && contp.Content.Deleted == false)
                    {
                        return true;
                    }
                }
            }
            return false;
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
            if (cre == null) return false;
            return true;
        }
        public bool IsValid2(TipoContent d)
        {
            if ((int)d <= 0 || (int)d > 5) return false;
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

        public bool IdNicknameCorresponden(int id)
        {
            var cre = _context.Creators.Where(c => c.Id == IdCre && c.NickName == Nickname).Include(x => x.Plans).ThenInclude(p => p.ContentPlans).ThenInclude(p => p.Content).FirstOrDefault();
            if (cre == null) return false;

            return true;
        }

        public bool CorrespondePlan(ICollection<int> dto)
        {
            if(Publico == false) { 
            var cre = _context.Creators.Where(c => c.Id == IdCre && c.NickName == Nickname).Include(x => x.Plans).ThenInclude(p => p.ContentPlans).ThenInclude(p => p.Content).FirstOrDefault();
            var aux = new List<int>();
            if (cre != null)
            {
                foreach (var item in cre.Plans)
                {
                    aux.Add(item.Id);
                }
                foreach (var item in dto)
                {
                    if (!aux.Contains(item))
                    {
                        return false;
                    }
                }
            }
            return true;
            }
            return true;
        }

        public bool ExisteDraft(bool b)
        {
            var cre = _context.Creators.Where(c => c.Id == IdCre && c.NickName == Nickname).Include(x => x.Plans).ThenInclude(p => p.ContentPlans).ThenInclude(p => p.Content).FirstOrDefault();
            if (cre != null && b == true)
            {
                foreach (var pl in cre.Plans)
                {
                    foreach (var contp in pl.ContentPlans)
                    {
                        if (contp.Content.Draft == true && contp.Content.Id != IdContent &&  contp.Content.Deleted == false)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}
