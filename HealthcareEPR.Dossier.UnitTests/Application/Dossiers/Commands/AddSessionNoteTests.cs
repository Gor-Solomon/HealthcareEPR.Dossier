using FluentAssertions;
using HealthcareEPR.Dossier.Application.Common.Interfaces;
using HealthcareEPR.Dossier.Application.Dossiers.Commands.AddSessionNote;
using HealthcareEPR.Dossier.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace HealthcareEPR.Dossier.UnitTests.Application.Dossiers.Commands;

public class AddSessionNoteTests
{
    private readonly Mock<IDossierRepository> _repositoryMock;
    private readonly Mock<IAiSummarizationService> _aiServiceMock;
    private readonly Mock<ILogger<AddSessionNoteCommandHandler>> _loggerMock;
    private readonly AddSessionNoteCommandHandler _handler;

    public AddSessionNoteTests()
    {
        _repositoryMock = new Mock<IDossierRepository>();
        _aiServiceMock = new Mock<IAiSummarizationService>();
        _loggerMock = new Mock<ILogger<AddSessionNoteCommandHandler>>();
        _handler = new AddSessionNoteCommandHandler(_repositoryMock.Object, _aiServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddNote_WhenDossierExists()
    {
        // Arrange
        var dossierId = Guid.NewGuid();
        var dossier = new PatientDossier(dossierId, Guid.NewGuid());
        var command = new AddSessionNoteCommand(dossierId, "Sample note content");
        var summary = "Structured summary";

        _repositoryMock.Setup(x => x.GetByIdAsync(dossierId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(dossier);
        _aiServiceMock.Setup(x => x.SummarizeNoteAsync(command.RawContent, It.IsAny<CancellationToken>()))
            .ReturnsAsync(summary);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        
        // Verify AddNoteAsync was called with the correct note data
        _repositoryMock.Verify(x => x.AddNoteAsync(
            It.Is<SessionNote>(n => 
                n.DossierId == dossierId && 
                n.RawContent == command.RawContent && 
                n.AiSummary == summary), 
            It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldHandleAiFailureGracefully_AndStillSaveNote()
    {
        // Arrange
        var dossierId = Guid.NewGuid();
        var dossier = new PatientDossier(dossierId, Guid.NewGuid());
        var command = new AddSessionNoteCommand(dossierId, "Sample note content");

        _repositoryMock.Setup(x => x.GetByIdAsync(dossierId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(dossier);
        _aiServiceMock.Setup(x => x.SummarizeNoteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("AI Service Down"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        
        // Verify AddNoteAsync was still called even if AI failed
        _repositoryMock.Verify(x => x.AddNoteAsync(
            It.Is<SessionNote>(n => 
                n.DossierId == dossierId && 
                n.RawContent == command.RawContent && 
                n.AiSummary == null), 
            It.IsAny<CancellationToken>()), 
            Times.Once);
        
        // Verify logger was called
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("AI Summarization failed")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenDossierDoesNotExist()
    {
        // Arrange
        var dossierId = Guid.NewGuid();
        var command = new AddSessionNoteCommand(dossierId, "Sample note content");

        _repositoryMock.Setup(x => x.GetByIdAsync(dossierId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PatientDossier?)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Dossier not found");
    }
}
