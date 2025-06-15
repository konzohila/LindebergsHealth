using MediatR;
using LindebergsHealth.Domain.Termine;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using LindebergsHealth.Domain.Entities;
using LindebergsHealth.Domain.Termine;

namespace LindebergsHealth.Application.Termine.Commands
{
    public class UpdateTerminHandler : IRequestHandler<UpdateTerminCommand, Termin>
    {
        private readonly ITermineRepository _termineRepository;
        public UpdateTerminHandler(ITermineRepository termineRepository) => _termineRepository = termineRepository;
        public async Task<Termin> Handle(UpdateTerminCommand request, CancellationToken cancellationToken)
            => await _termineRepository.UpdateTerminAsync(request.Termin);
    }
}
