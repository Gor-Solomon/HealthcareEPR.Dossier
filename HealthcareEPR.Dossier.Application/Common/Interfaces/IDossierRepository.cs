using HealthcareEPR.Dossier.Domain.Entities;

namespace HealthcareEPR.Dossier.Application.Common.Interfaces;

public interface IDossierRepository
{
    Task<PatientDossier?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(PatientDossier dossier, CancellationToken cancellationToken = default);
    Task AddNoteAsync(SessionNote note, CancellationToken cancellationToken = default);
}
