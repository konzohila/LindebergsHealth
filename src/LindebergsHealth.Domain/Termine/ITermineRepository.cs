using LindebergsHealth.Domain.Entities;

namespace LindebergsHealth.Domain.Termine
{
    public interface ITermineRepository
    {
        Task<List<Termin>> GetAllTermineAsync();
        Task<List<Termin>> GetTermineByMitarbeiterIdAsync(Guid mitarbeiterId);
        Task<Termin> GetTerminByIdAsync(Guid id);
        Task<Termin> CreateTerminAsync(Termin termin);
        Task<Termin> UpdateTerminAsync(Termin termin);
        Task<bool> DeleteTerminAsync(Guid id);
    }
}


