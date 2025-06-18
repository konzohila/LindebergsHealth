using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using LindebergsHealth.Application.Termine.Dto;
using LindebergsHealth.Domain.Entities;
using LindebergsHealth.Domain.Termine;
using Mapster;

namespace LindebergsHealth.Application.Termine.Queries
{
    public class GetTerminByIdHandler : IRequestHandler<GetTerminByIdQuery, TerminDetailDto>
    {
        private readonly ITermineRepository _termineRepository;
        public GetTerminByIdHandler(ITermineRepository termineRepository) => _termineRepository = termineRepository;
        public async Task<TerminDetailDto> Handle(GetTerminByIdQuery request, CancellationToken cancellationToken)
        {
            var t = await _termineRepository.GetTerminByIdAsync(request.Id);
            if (t == null) return null;
            return t.Adapt<TerminDetailDto>();
        }
    }
}
