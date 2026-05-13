using HealthcareEPR.Dossier.Application.Common.Interfaces;
using HealthcareEPR.Dossier.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthcareEPR.Dossier.Infrastructure.Persistence.Repositories;

public class DossierRepository : IDossierRepository
{
    private readonly DossierDbContext _context;

    public DossierRepository(DossierDbContext context)
    {
        _context = context;
    }

    public async Task<PatientDossier?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Dossiers
            .Include(d => d.Notes)
            .Include(d => d.Plans)
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(PatientDossier dossier, CancellationToken cancellationToken = default)
    {
        _context.Dossiers.Update(dossier);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddNoteAsync(SessionNote note, CancellationToken cancellationToken = default)
    {
        _context.SessionNotes.Add(note);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
