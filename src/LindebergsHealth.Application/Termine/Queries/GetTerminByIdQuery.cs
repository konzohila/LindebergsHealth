using System;
using MediatR;
using LindebergsHealth.Application.Termine.Dto;
using LindebergsHealth.Domain.Entities;

namespace LindebergsHealth.Application.Termine.Queries
{
    public class GetTerminByIdQuery : IRequest<TerminDetailDto>
    {
        public Guid Id { get; set; }
        public GetTerminByIdQuery(Guid id) => Id = id;
    }
}
