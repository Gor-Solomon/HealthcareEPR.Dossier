using HealthcareEPR.Dossier.Application.Dossiers.Commands.AddSessionNote;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareEPR.Dossier.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DossierController : ControllerBase
{
    private readonly IMediator _mediator;

    public DossierController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{id}/notes")]
    public async Task<IActionResult> AddNote(Guid id, [FromBody] string content)
    {
        var command = new AddSessionNoteCommand(id, content);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
