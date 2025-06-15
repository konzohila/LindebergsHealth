using MediatR;
using LindebergsHealth.Domain.Entities;

namespace LindebergsHealth.Application.Termine.Commands
{
    public class UpdateTerminCommand : IRequest<Termin>
    {
        public Termin Termin { get; set; }
        public UpdateTerminCommand(Termin termin) => Termin = termin;
    }
}
