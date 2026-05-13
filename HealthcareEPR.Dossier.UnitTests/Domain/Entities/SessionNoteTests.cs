using FluentAssertions;
using HealthcareEPR.Dossier.Domain.Entities;
using Xunit;

namespace HealthcareEPR.Dossier.UnitTests.Domain.Entities;

public class SessionNoteTests
{
    [Fact]
    public void Constructor_ShouldInitializeCorrectly_WithValidData()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dossierId = Guid.NewGuid();
        var doctorId = Guid.NewGuid();
        var content = "Patient reported mild symptoms.";

        // Act
        var note = new SessionNote(id, dossierId, doctorId, content);

        // Assert
        note.Id.Should().Be(id);
        note.DossierId.Should().Be(dossierId);
        note.DoctorId.Should().Be(doctorId);
        note.RawContent.Should().Be(content);
        note.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_ShouldThrowArgumentException_WhenContentIsInvalid(string? invalidContent)
    {
        // Arrange
        var id = Guid.NewGuid();
        var dossierId = Guid.NewGuid();
        var doctorId = Guid.NewGuid();

        // Act
        Action act = () => new SessionNote(id, dossierId, doctorId, invalidContent!);

        // Assert
        act.Should().Throw<ArgumentException>()
           .WithMessage("*Note content cannot be empty*");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenDoctorIdIsEmpty()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dossierId = Guid.NewGuid();
        var doctorId = Guid.Empty;

        // Act
        Action act = () => new SessionNote(id, dossierId, doctorId, "Valid content");

        // Assert
        act.Should().Throw<ArgumentException>()
           .WithMessage("*DoctorId cannot be empty*");
    }

    [Fact]
    public void SetAiSummary_ShouldUpdateStateCorrectly()
    {
        // Arrange
        var note = new SessionNote(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Content");
        var summary = "Structured summary";

        // Act
        note.SetAiSummary(summary);

        // Assert
        note.AiSummary.Should().Be(summary);
    }
}
