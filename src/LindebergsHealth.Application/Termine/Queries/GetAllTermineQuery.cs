using MediatR;
using System.Collections.Generic;
using LindebergsHealth.Application.Termine.Dto;
using LindebergsHealth.Domain.Entities;

namespace LindebergsHealth.Application.Termine.Queries
{
    public class GetAllTermineQuery : IRequest<List<TerminListDto>> { }
}
