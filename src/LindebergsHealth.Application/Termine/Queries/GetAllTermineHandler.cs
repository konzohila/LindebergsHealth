using MediatR;
using LindebergsHealth.Domain.Entities;
using LindebergsHealth.Domain.Termine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LindebergsHealth.Application.Termine.Dto;
using Mapster;

namespace LindebergsHealth.Application.Termine.Queries
{
    public class GetAllTermineHandler : IRequestHandler<GetAllTermineQuery, List<TerminListDto>>
    {
        private readonly ITermineRepository _termineRepository;

        public GetAllTermineHandler(ITermineRepository termineRepository)
        {
            _termineRepository = termineRepository;
        }

        public async Task<List<TerminListDto>> Handle(GetAllTermineQuery request, CancellationToken cancellationToken)
        {
            var termine = await _termineRepository.GetAllTermineAsync();
            return termine.Adapt<List<TerminListDto>>();
        }
    }
}
