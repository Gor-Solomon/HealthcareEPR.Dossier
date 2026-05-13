using FluentAssertions;
using HealthcareEPR.Dossier.Application.Dossiers.Commands.AddSessionNote;
using MediatR;
using Moq;
using Xunit;
using HealthcareEPR.Dossier.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareEPR.Dossier.UnitTests.Api.Controllers;

public class DossierControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly DossierController _controller;

    public DossierControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new DossierController(_mediatorMock.Object);
    }

    [Fact]
    public async Task AddNote_ShouldReturnOk_WithNoteId()
    {
        // Arrange
        var dossierId = Guid.NewGuid();
        var content = "Patient is recovering well.";
        var expectedNoteId = Guid.NewGuid();

        _mediatorMock.Setup(m => m.Send(It.Is<AddSessionNoteCommand>(c => 
            c.DossierId == dossierId && c.RawContent == content), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedNoteId);

        // Act
        var result = await _controller.AddNote(dossierId, content);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().Be(expectedNoteId);
    }
}
