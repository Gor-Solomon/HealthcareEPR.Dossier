using HealthcareEPR.Dossier.Application.Common.Interfaces;

namespace HealthcareEPR.Dossier.Infrastructure.Services;

public class AiSummarizationService : IAiSummarizationService
{
    public async Task<string> SummarizeNoteAsync(string rawContent, CancellationToken cancellationToken = default)
    {
        // Simulate AI processing delay
        await Task.Delay(500, cancellationToken);

        // Dummy logic to simulate summarization
        return $"[AI SUMMARY]: Patient presented with {rawContent.Length} characters of messy notes. Key takeaway: Follow-up required.";
    }
}
