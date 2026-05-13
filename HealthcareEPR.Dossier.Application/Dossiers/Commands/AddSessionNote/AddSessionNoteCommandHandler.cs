using HealthcareEPR.Dossier.Application.Common.Interfaces;
using HealthcareEPR.Dossier.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HealthcareEPR.Dossier.Application.Dossiers.Commands.AddSessionNote;

public class AddSessionNoteCommandHandler : IRequestHandler<AddSessionNoteCommand, Guid>
{
    private readonly IDossierRepository _repository;
    private readonly IAiSummarizationService _aiService;
    private readonly ILogger<AddSessionNoteCommandHandler> _logger;

    public AddSessionNoteCommandHandler(
        IDossierRepository repository, 
        IAiSummarizationService aiService,
        ILogger<AddSessionNoteCommandHandler> logger)
    {
        _repository = repository;
        _aiService = aiService;
        _logger = logger;
    }

    public async Task<Guid> Handle(AddSessionNoteCommand request, CancellationToken cancellationToken)
    {
        var dossier = await _repository.GetByIdAsync(request.DossierId, cancellationToken);
        if (dossier == null)
            throw new Exception("Dossier not found");

        var note = new SessionNote(Guid.NewGuid(), dossier.Id, request.RawContent);

        try 
        {
            var summary = await _aiService.SummarizeNoteAsync(request.RawContent, cancellationToken);
            note.SetAiSummary(summary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AI Summarization failed for note {NoteId}. Proceeding without summary.", note.Id);
        }

        // Use the new repository method to save the note directly
        // This avoids the aggregate update issue with EF In-Memory
        await _repository.AddNoteAsync(note, cancellationToken);

        return note.Id;
    }
}
