using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LindebergsHealth.Domain.Entities;
using LindebergsHealth.Domain.Termine;
using LindebergsHealth.Infrastructure.Data;

namespace LindebergsHealth.Infrastructure.Repositories
{
    public class TermineRepository : ITermineRepository
    {
        private readonly LindebergsHealthDbContext _db;

        public TermineRepository(LindebergsHealthDbContext db)
        {
            _db = db;
        }

        public async Task<List<Termin>> GetAllTermineAsync()
        {
            return await _db.Termine.ToListAsync();
        }

        public async Task<List<Termin>> GetTermineByMitarbeiterIdAsync(Guid mitarbeiterId)
        {
            return await _db.Termine.Where(t => t.MitarbeiterId == mitarbeiterId).ToListAsync();
        }

        public async Task<Termin> GetTerminByIdAsync(Guid id)
        {
            return await _db.Termine.FindAsync(id);
        }

        public async Task<Termin> CreateTerminAsync(Termin termin)
        {
            _db.Termine.Add(termin);
            await _db.SaveChangesAsync();
            return termin;
        }

        public async Task<Termin> UpdateTerminAsync(Termin termin)
        {
            _db.Termine.Update(termin);
            await _db.SaveChangesAsync();
            return termin;
        }

        public async Task<bool> DeleteTerminAsync(Guid id)
        {
            var termin = await _db.Termine.FindAsync(id);
            if (termin is null)
            {
                return false;
            }
            _db.Termine.Remove(termin);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
