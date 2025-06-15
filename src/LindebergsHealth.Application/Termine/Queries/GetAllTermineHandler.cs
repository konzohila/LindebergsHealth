using MediatR;
using LindebergsHealth.Domain.Entities;
using LindebergsHealth.Domain.Termine;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LindebergsHealth.Application.Termine.Queries
{
    public class GetAllTermineHandler : IRequestHandler<GetAllTermineQuery, List<Termin>>
    {
        private readonly ITermineRepository _termineRepository;

        public GetAllTermineHandler(ITermineRepository termineRepository)
        {
            _termineRepository = termineRepository;
        }

        public async Task<List<Termin>> Handle(GetAllTermineQuery request, CancellationToken cancellationToken)
        {
            return await _termineRepository.GetAllTermineAsync();
        }
    }
}
