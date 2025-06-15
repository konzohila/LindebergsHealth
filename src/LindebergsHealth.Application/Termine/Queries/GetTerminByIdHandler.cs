using MediatR;
using LindebergsHealth.Domain.Termine;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using LindebergsHealth.Domain.Entities;
using LindebergsHealth.Domain.Termine;

namespace LindebergsHealth.Application.Termine.Queries
{
    public class GetTerminByIdHandler : IRequestHandler<GetTerminByIdQuery, Termin>
    {
        private readonly ITermineRepository _termineRepository;
        public GetTerminByIdHandler(ITermineRepository termineRepository) => _termineRepository = termineRepository;
        public async Task<Termin> Handle(GetTerminByIdQuery request, CancellationToken cancellationToken)
            => await _termineRepository.GetTerminByIdAsync(request.Id);
    }
}
