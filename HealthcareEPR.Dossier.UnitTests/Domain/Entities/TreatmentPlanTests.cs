using FluentAssertions;
using HealthcareEPR.Dossier.Domain.Entities;
using Xunit;

namespace HealthcareEPR.Dossier.UnitTests.Domain.Entities;

public class TreatmentPlanTests
{
    [Fact]
    public void Constructor_ShouldInitializeCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dossierId = Guid.NewGuid();
        var description = "Post-op recovery plan";
        var startDate = DateTime.UtcNow;

        // Act
        var plan = new TreatmentPlan(id, dossierId, description, startDate);

        // Assert
        plan.Id.Should().Be(id);
        plan.DossierId.Should().Be(dossierId);
        plan.Description.Should().Be(description);
        plan.StartDate.Should().Be(startDate);
        plan.EndDate.Should().BeNull();
    }
}
