using MediatR;
using System;
using LindebergsHealth.Application.Termine.Dto;
using MediatR;

namespace LindebergsHealth.Application.Termine.Commands
{
    public class CreateTerminCommand : IRequest<TerminDetailDto>
    {
        public CreateTerminDto Termin { get; set; }
        public CreateTerminCommand(CreateTerminDto termin)
        {
            Termin = termin;
        }
    }
}
