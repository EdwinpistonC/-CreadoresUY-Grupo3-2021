using Application.Features.Validators;
using Application.Interface;
using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Share.Dtos;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.CategoryFeaturesBO.Commands
{
    public class CreateCategoryCommandBO : IRequest<Response<String>>
    {
        public string Name { get; set; }
        public class CreateCategoryCommandBOHandler : IRequestHandler<CreateCategoryCommandBO, Response<String>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;

            public CreateCategoryCommandBOHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<String>> Handle(CreateCategoryCommandBO command, CancellationToken cancellationToken)
            {
                var findCategory = _context.Categorys.Where(c => c.Name==command.Name).FirstOrDefault();
                Response<string> res = new Response<String>
                {
                    Obj = "",
                    Message = new List<String>()
                };
                if (findCategory!=null)
                {
                    res.Obj = "";
                    res.CodStatus = HttpStatusCode.BadRequest;
                    res.Success = false;
                    var msj = "Categoría ya existe";
                    res.Message.Add(msj);
                    return res;
                }



                Category category = new Category(command.Name);
                _context.Categorys.Add(category);
                await _context.SaveChangesAsync();
                res.CodStatus = HttpStatusCode.Created;
                res.Success = true;
                var msg1 = "Category ingresado correctamente";
                res.Message.Add(msg1);
                return res;
            }
        }
    }
}

