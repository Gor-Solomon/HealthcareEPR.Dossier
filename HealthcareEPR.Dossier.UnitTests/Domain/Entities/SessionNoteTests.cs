using FluentAssertions;
using HealthcareEPR.Dossier.Domain.Entities;
using Xunit;

namespace HealthcareEPR.Dossier.UnitTests.Domain.Entities;

public class SessionNoteTests
{
    private readonly Guid _validId = Guid.NewGuid();
    private readonly Guid _validDossierId = Guid.NewGuid();
    private readonly Guid _validDoctorId = Guid.NewGuid();
    private const string ValidContent = "Patient reported mild symptoms.";

    [Fact]
    public void Constructor_ShouldInitializeCorrectly_WithValidData()
    {
        // Act
        var note = new SessionNote(_validId, _validDossierId, _validDoctorId, ValidContent);

        // Assert
        note.Id.Should().Be(_validId);
        note.DossierId.Should().Be(_validDossierId);
        note.DoctorId.Should().Be(_validDoctorId);
        note.RawContent.Should().Be(ValidContent);
        note.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000", "3fa85f64-5717-4562-b3fc-2c963f66afa6", "3fa85f64-5717-4562-b3fc-2c963f66afa6", "Content", "Id cannot be empty")]
    [InlineData("3fa85f64-5717-4562-b3fc-2c963f66afa6", "00000000-0000-0000-0000-000000000000", "3fa85f64-5717-4562-b3fc-2c963f66afa6", "Content", "DossierId cannot be empty")]
    [InlineData("3fa85f64-5717-4562-b3fc-2c963f66afa6", "3fa85f64-5717-4562-b3fc-2c963f66afa6", "00000000-0000-0000-0000-000000000000", "Content", "DoctorId cannot be empty")]
    [InlineData("3fa85f64-5717-4562-b3fc-2c963f66afa6", "3fa85f64-5717-4562-b3fc-2c963f66afa6", "3fa85f64-5717-4562-b3fc-2c963f66afa6", "", "Note content cannot be empty")]
    [InlineData("3fa85f64-5717-4562-b3fc-2c963f66afa6", "3fa85f64-5717-4562-b3fc-2c963f66afa6", "3fa85f64-5717-4562-b3fc-2c963f66afa6", " ", "Note content cannot be empty")]
    [InlineData("3fa85f64-5717-4562-b3fc-2c963f66afa6", "3fa85f64-5717-4562-b3fc-2c963f66afa6", "3fa85f64-5717-4562-b3fc-2c963f66afa6", null, "Note content cannot be empty")]
    public void Constructor_ShouldThrowArgumentException_WhenInputIsInvalid(string idStr, string dossierIdStr, string doctorIdStr, string? content, string expectedMessage)
    {
        // Arrange
        var id = Guid.Parse(idStr);
        var dossierId = Guid.Parse(dossierIdStr);
        var doctorId = Guid.Parse(doctorIdStr);

        // Act
        Action act = () => new SessionNote(id, dossierId, doctorId, content!);

        // Assert
        act.Should().Throw<ArgumentException>()
           .WithMessage($"*{expectedMessage}*");
    }

    [Fact]
    public void SetAiSummary_ShouldUpdateStateCorrectly()
    {
        // Arrange
        var note = new SessionNote(_validId, _validDossierId, _validDoctorId, ValidContent);
        var summary = "Structured summary";

        // Act
        note.SetAiSummary(summary);

        // Assert
        note.AiSummary.Should().Be(summary);
    }
}
