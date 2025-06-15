using MediatR;
using LindebergsHealth.Domain.Termine;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using LindebergsHealth.Domain.Termine;

namespace LindebergsHealth.Application.Termine.Commands
{
    public class DeleteTerminHandler : IRequestHandler<DeleteTerminCommand, bool>
    {
        private readonly ITermineRepository _termineRepository;
        public DeleteTerminHandler(ITermineRepository termineRepository) => _termineRepository = termineRepository;
        public async Task<bool> Handle(DeleteTerminCommand request, CancellationToken cancellationToken)
            => await _termineRepository.DeleteTerminAsync(request.Id);
    }
}
