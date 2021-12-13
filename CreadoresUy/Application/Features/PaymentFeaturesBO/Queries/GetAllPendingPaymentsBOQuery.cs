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
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserFeaturesBO.Queries
{
    public class GetAllPendingPaymentsBOQuery : IRequest<Response<List<PaymentBODto>>>
    {

        public class GetAllPendingPaymentsBOQueryHandler : IRequestHandler<GetAllPendingPaymentsBOQuery, Response<List<PaymentBODto>>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;
            public GetAllPendingPaymentsBOQueryHandler(ICreadoresUyDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<List<PaymentBODto>>> Handle(GetAllPendingPaymentsBOQuery query, CancellationToken cancellationToken)
            {
                Response<List<PaymentBODto>> res = new();
                res.Message = new List<string>();
                var pagos = new List<PaymentBODto>();

//              var resultados = _context.PagosCreador.GroupBy(x => x.IdCreator).Select(c => new { IdCre = c.Key, Nickname = c.Nickname, Monto = c.Amount }).ToListAsync(); 
                                                                        



                res.Obj = pagos;
                res.CodStatus = HttpStatusCode.OK;
                res.Success = true;
                var msj1 = "Ok";
                res.Message.Add(msj1);
                return res;

            }
        }
    }
}

