using System;
using MediatR;

namespace LindebergsHealth.Application.Termine.Commands
{
    public class DeleteTerminCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public DeleteTerminCommand(Guid id) => Id = id;
    }
}
