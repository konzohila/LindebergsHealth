using System;
using System.Threading;
using System.Threading.Tasks;
using LindebergsHealth.Application.Termine.Dto;
using LindebergsHealth.Domain.Entities;
using LindebergsHealth.Domain.Termine;
using MediatR;
using Mapster;

namespace LindebergsHealth.Application.Termine.Commands
{
    public class CreateTerminHandler : IRequestHandler<CreateTerminCommand, TerminDetailDto>
    {
        private readonly ITermineRepository _termineRepository;
        public CreateTerminHandler(ITermineRepository termineRepository) => _termineRepository = termineRepository;

        public async Task<TerminDetailDto> Handle(CreateTerminCommand request, CancellationToken cancellationToken)
        {
            var termin = request.Termin.Adapt<Termin>();
            termin.Id = Guid.NewGuid();
            await _termineRepository.CreateTerminAsync(termin);
            return termin.Adapt<TerminDetailDto>();
        }
    }
}
