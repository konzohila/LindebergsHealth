using MediatR;
using LindebergsHealth.Domain.Entities;
using LindebergsHealth.Domain.Termine;
using System.Threading;
using System.Threading.Tasks;

namespace LindebergsHealth.Application.Termine.Commands
{
    public class CreateTerminHandler : IRequestHandler<CreateTerminCommand, Termin>
    {
        private readonly ITermineRepository _termineRepository;
        public CreateTerminHandler(ITermineRepository termineRepository) => _termineRepository = termineRepository;

        public async Task<Termin> Handle(CreateTerminCommand request, CancellationToken cancellationToken)
        {
            var termin = new Termin
            {
                Id = Guid.NewGuid(),
                Titel = request.Titel,
                Beschreibung = request.Beschreibung,
                Datum = request.Datum,
                DauerMinuten = request.DauerMinuten,
                MitarbeiterId = request.MitarbeiterId,
                PatientId = request.PatientId,
                RaumId = request.RaumId,
                KategorieId = request.KategorieId
                // ... weitere Felder
            };
            await _termineRepository.CreateTerminAsync(termin);
            return termin;
        }
    }
}
