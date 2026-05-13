namespace HealthcareEPR.Dossier.Domain.Entities;

public class Patient
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public DateTime DateOfBirth { get; private set; }

    private readonly List<PatientDossier> _dossiers = new();
    public IReadOnlyCollection<PatientDossier> Dossiers => _dossiers.AsReadOnly();

    public Patient(Guid id, string firstName, string lastName, DateTime dateOfBirth)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
    }

    public void AddDossier(PatientDossier dossier)
    {
        _dossiers.Add(dossier);
    }
}
