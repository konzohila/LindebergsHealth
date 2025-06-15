using System;
using MediatR;
using LindebergsHealth.Domain.Entities;

namespace LindebergsHealth.Application.Termine.Queries
{
    public class GetTerminByIdQuery : IRequest<Termin>
    {
        public Guid Id { get; set; }
        public GetTerminByIdQuery(Guid id) => Id = id;
    }
}
