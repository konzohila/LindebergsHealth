using MediatR;
using LindebergsHealth.Domain.Entities;
using System;

namespace LindebergsHealth.Application.Termine.Commands
{
    public class CreateTerminCommand : IRequest<Termin>
    {
        public string Titel { get; set; } = string.Empty;
        public string Beschreibung { get; set; } = string.Empty;
        public DateTime Datum { get; set; }
        public int DauerMinuten { get; set; }
        public Guid MitarbeiterId { get; set; }
        public Guid? PatientId { get; set; }
        public Guid RaumId { get; set; }
        public Guid KategorieId { get; set; }
        // ... weitere Felder nach Bedarf
    }
}
