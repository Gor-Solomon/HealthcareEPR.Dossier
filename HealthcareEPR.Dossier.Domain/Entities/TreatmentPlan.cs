namespace HealthcareEPR.Dossier.Domain.Entities;

public class TreatmentPlan
{
    public Guid Id { get; private set; }
    public Guid DossierId { get; private set; }
    public string Description { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }

    public TreatmentPlan(Guid id, Guid dossierId, string description, DateTime startDate)
    {
        Id = id;
        DossierId = dossierId;
        Description = description;
        StartDate = startDate;
    }
}
