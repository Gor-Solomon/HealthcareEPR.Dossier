using FluentAssertions;
using HealthcareEPR.Dossier.Domain.Entities;
using Xunit;

namespace HealthcareEPR.Dossier.UnitTests.Domain.Entities;

public class PatientDossierTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithCreatedAtDate()
    {
        // Arrange
        var id = Guid.NewGuid();
        var patientId = Guid.NewGuid();

        // Act
        var dossier = new PatientDossier(id, patientId);

        // Assert
        dossier.Id.Should().Be(id);
        dossier.PatientId.Should().Be(patientId);
        dossier.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        dossier.Notes.Should().BeEmpty();
        dossier.Plans.Should().BeEmpty();
    }

    [Fact]
    public void AddNote_ShouldSuccessfullyAddNoteToCollection()
    {
        // Arrange
        var dossier = new PatientDossier(Guid.NewGuid(), Guid.NewGuid());
        var note = new SessionNote(Guid.NewGuid(), dossier.Id, Guid.NewGuid(), "Some clinical notes");

        // Act
        dossier.AddNote(note);

        // Assert
        dossier.Notes.Should().HaveCount(1);
        dossier.Notes.Should().Contain(note);
    }

    [Fact]
    public void AddTreatmentPlan_ShouldSuccessfullyAddPlanToCollection()
    {
        // Arrange
        var dossier = new PatientDossier(Guid.NewGuid(), Guid.NewGuid());
        var plan = new TreatmentPlan(Guid.NewGuid(), dossier.Id, "Weekly therapy", DateTime.UtcNow);

        // Act
        dossier.AddTreatmentPlan(plan);

        // Assert
        dossier.Plans.Should().HaveCount(1);
        dossier.Plans.Should().Contain(plan);
    }
}
