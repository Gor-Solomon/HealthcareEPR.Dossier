namespace HealthcareEPR.Dossier.Domain.Entities;

public class SessionNote
{
    public Guid Id { get; private set; }
    public Guid DossierId { get; private set; }
    public string RawContent { get; private set; }
    public string? AiSummary { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public SessionNote(Guid id, Guid dossierId, string rawContent)
    {
        if (string.IsNullOrWhiteSpace(rawContent))
            throw new ArgumentException("Note content cannot be empty", nameof(rawContent));

        Id = id;
        DossierId = dossierId;
        RawContent = rawContent;
        CreatedAt = DateTime.UtcNow;
    }

    public void SetAiSummary(string summary)
    {
        AiSummary = summary;
    }
}
