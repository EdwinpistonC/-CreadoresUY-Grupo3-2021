using Application.Interface;
using MediatR;
using Share.Dtos;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.StatisticsFeaturesBO.Queries
{
    public class GetUnsubscribersQuery  : IRequest<Response<List<StatisticsBODto<int>>>>
    {
        public class GetUnsubscribersQueryHandler : IRequestHandler<GetUnsubscribersQuery, Response<List<StatisticsBODto<int>>>>
        {
            private readonly ICreadoresUyDbContext _context;

            public GetUnsubscribersQueryHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }

            public async  Task<Response<List<StatisticsBODto<int>>>> Handle(GetUnsubscribersQuery request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
