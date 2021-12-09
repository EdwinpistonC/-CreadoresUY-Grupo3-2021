using Application.Interface;
using Application.Service;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Share.Dtos;
using Share.Dtos.BackOffice;
using Share.NoSql;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.StatisticsFeaturesBO.Queries
{
    public class GetLogsQuery : IRequest<Response<List<StatisticsBODto<string, List<int>>>>>
    {
        public class GetLogsQueryHandler : IRequestHandler<GetLogsQuery, Response<List<StatisticsBODto<string, List<int>>>>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly NoSQLConnection _noSQLConnection;

            public GetLogsQueryHandler(ICreadoresUyDbContext context , NoSQLConnection nosql)
            {
                _context = context;
               _noSQLConnection = nosql;
            }

            public async  Task<Response<List<StatisticsBODto<string, List<int>>>>> Handle(GetLogsQuery request, CancellationToken cancellationToken)
            {
                Response<List<StatisticsBODto<string, List<int>>>> response = new();

                List<LogDto> loginDtos = _noSQLConnection.Get();

                var objsExito = loginDtos.Where(l => l.intent == true).GroupBy(l =>  l.User).Select(l => new { user = l.Key, cantidad = l.Count()});
                var objsError = loginDtos.Where(l=>l.intent==false).GroupBy(l => l.User).Select(l => new { user = l.Key, cantidad = l.Count() });


                List<StatisticsBODto<string, List<int>>> listCreadores = new();




                foreach (var o in objsExito)
                {
                    var lista = new List<int>();

                    lista.Add(o.cantidad);
                    listCreadores.Add(new StatisticsBODto<string, List<int>> { XValue = o.user, YValue = lista });
                }
                
                foreach (var o in objsError)
                {
                    var ingresa = false;
                    foreach (var o2 in listCreadores)
                    {
                        if (o.user== o2.XValue)
                        {
                            ingresa= true;
                            o2.YValue.Add(o.cantidad);
                        }

                    }
                    if(!ingresa)
                    {
                        var lista = new List<int>();
                        lista.Add(0);
                        lista.Add(o.cantidad);
                        listCreadores.Add(new StatisticsBODto<string, List<int>> { XValue = o.user, YValue = lista });
                    }
                }

                foreach (var item in listCreadores) if(item.YValue.Count == 1)item.YValue.Add(0);
                



                response.Obj = listCreadores;
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
