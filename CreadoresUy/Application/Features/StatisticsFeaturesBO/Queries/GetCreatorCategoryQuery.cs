using Application.Interface;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Share.Dtos;
using Share.Dtos.BackOffice;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.StatisticsFeaturesBO.Queries
{      
    
    //cuantos pagos por mes

    public class GetCreatorCategoryQuery : IRequest<Response<List<StatisticsBODto<String, float>>>>
    {

        public class GetCreatorCategoryQueryHandler : IRequestHandler<GetCreatorCategoryQuery, Response<List<StatisticsBODto<String, float>>>>
        {

            private readonly ICreadoresUyDbContext _context;

            public GetCreatorCategoryQueryHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }

            public async Task<Response<List<StatisticsBODto<String, float>>>> Handle(GetCreatorCategoryQuery query, CancellationToken cancellationToken)
            {
                Response<List<StatisticsBODto<String, float>>> response = new();

                var categorias1 = await _context.Creators.GroupBy(p =>new { p.Category1 }).Select(p => new { Categoria = p.Key.Category1, Creadores = p.Count()}).ToListAsync();
                var categorias2 = await _context.Creators.GroupBy(p => new { p.Category2 }).Select(p => new { Categoria = p.Key.Category2, Creadores = p.Count() }).ToListAsync();
                
                
                List<StatisticsBODto<String, float>> listPagos = new();

                foreach (var item in categorias1)
                {
                    var sum = 0;
                    sum += item.Creadores;

                    foreach (var item2 in categorias2)
                    {
                        if (item.Categoria == item2.Categoria)
                        {
                            sum += item2.Creadores;
                        }
                    }

                    listPagos.Add(new StatisticsBODto<String, float> { XValue= item.Categoria , YValue = sum });
                }
                foreach (var item in categorias2)
                {
                    var sum = 0;
                    sum += item.Creadores;
                    var contiene = false;
                    foreach (var item2 in categorias1)
                    {
                        if (item.Categoria == item2.Categoria)
                        {
                            contiene = true;
                        }
                    }
                    if (!contiene)
                    {
                        listPagos.Add(new StatisticsBODto<String, float> { XValue = item.Categoria, YValue = sum });
                    }
                }



                response.Obj = listPagos;
                response.CodStatus = HttpStatusCode.OK;
                response.Success = true;
                response.Message = new();
                var msj1 = "Ok";
                response.Message.Add(msj1);

                return response;
            }

        }
    }
}
