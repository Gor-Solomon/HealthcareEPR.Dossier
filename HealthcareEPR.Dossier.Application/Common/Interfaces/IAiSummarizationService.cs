namespace HealthcareEPR.Dossier.Application.Common.Interfaces;

public interface IAiSummarizationService
{
    Task<string> SummarizeNoteAsync(string rawContent, CancellationToken cancellationToken = default);
}
