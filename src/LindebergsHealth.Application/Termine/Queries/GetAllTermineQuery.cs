using MediatR;
using LindebergsHealth.Domain.Entities;
using System.Collections.Generic;

namespace LindebergsHealth.Application.Termine.Queries
{
    public class GetAllTermineQuery : IRequest<List<Termin>> { }
}
