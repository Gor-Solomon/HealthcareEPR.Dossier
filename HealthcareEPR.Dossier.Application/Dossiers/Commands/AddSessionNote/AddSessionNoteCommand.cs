using MediatR;

namespace HealthcareEPR.Dossier.Application.Dossiers.Commands.AddSessionNote;

public record AddSessionNoteCommand(Guid DossierId, string RawContent) : IRequest<Guid>;
