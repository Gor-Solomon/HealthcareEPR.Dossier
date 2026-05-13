using FluentAssertions;
using HealthcareEPR.Dossier.Domain.Entities;
using Xunit;

namespace HealthcareEPR.Dossier.UnitTests.Domain.Entities;

public class PatientTests
{
    [Fact]
    public void Constructor_ShouldInitializeCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var firstName = "Jane";
        var lastName = "Doe";
        var dob = new DateTime(1990, 1, 1);

        // Act
        var patient = new Patient(id, firstName, lastName, dob);

        // Assert
        patient.Id.Should().Be(id);
        patient.FirstName.Should().Be(firstName);
        patient.LastName.Should().Be(lastName);
        patient.DateOfBirth.Should().Be(dob);
        patient.Dossiers.Should().BeEmpty();
    }

    [Fact]
    public void AddDossier_ShouldAddDossierToCollection()
    {
        // Arrange
        var patient = new Patient(Guid.NewGuid(), "Jane", "Doe", new DateTime(1990, 1, 1));
        var dossier = new PatientDossier(Guid.NewGuid(), patient.Id);

        // Act
        patient.AddDossier(dossier);

        // Assert
        patient.Dossiers.Should().HaveCount(1);
        patient.Dossiers.Should().Contain(dossier);
    }
}
