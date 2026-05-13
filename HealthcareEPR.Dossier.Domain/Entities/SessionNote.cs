namespace HealthcareEPR.Dossier.Domain.Entities;

public class SessionNote
{
    public Guid Id { get; private set; }
    public Guid DossierId { get; private set; }
    public Guid DoctorId { get; private set; }
    public string RawContent { get; private set; }
    public string? AiSummary { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public SessionNote(Guid id, Guid dossierId, Guid doctorId, string rawContent)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty", nameof(id));
        if (dossierId == Guid.Empty) throw new ArgumentException("DossierId cannot be empty", nameof(dossierId));
        if (doctorId == Guid.Empty) throw new ArgumentException("DoctorId cannot be empty", nameof(doctorId));
        if (string.IsNullOrWhiteSpace(rawContent))
            throw new ArgumentException("Note content cannot be empty", nameof(rawContent));

        Id = id;
        DossierId = dossierId;
        DoctorId = doctorId;
        RawContent = rawContent;
        CreatedAt = DateTime.UtcNow;
    }

    public void SetAiSummary(string summary)
    {
        AiSummary = summary;
    }
}
