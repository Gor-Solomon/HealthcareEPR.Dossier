namespace HealthcareEPR.Dossier.Domain.Entities;

public class PatientDossier
{
    public Guid Id { get; private set; }
    public Guid PatientId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private readonly List<SessionNote> _notes = new();
    public IReadOnlyCollection<SessionNote> Notes => _notes.AsReadOnly();

    private readonly List<TreatmentPlan> _plans = new();
    public IReadOnlyCollection<TreatmentPlan> Plans => _plans.AsReadOnly();

    public PatientDossier(Guid id, Guid patientId)
    {
        Id = id;
        PatientId = patientId;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddNote(SessionNote note)
    {
        _notes.Add(note);
    }

    public void AddTreatmentPlan(TreatmentPlan plan)
    {
        _plans.Add(plan);
    }
}
