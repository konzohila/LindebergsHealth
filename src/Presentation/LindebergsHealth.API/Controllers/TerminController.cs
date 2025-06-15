using LindebergsHealth.Application.Termine.Commands;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using LindebergsHealth.Domain.Entities;
using LindebergsHealth.Application.Termine.Commands;
using LindebergsHealth.Application.Termine.Queries;

namespace LindebergsHealth.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TerminController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TerminController(IMediator mediator) => _mediator = mediator;

        // Alle Termine
        [HttpGet]
        public async Task<ActionResult<List<Termin>>> Get()
        {
            var result = await _mediator.Send(new GetAllTermineQuery());
            return Ok(result);
        }

        // Termin nach Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Termin>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetTerminByIdQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }

        // Termin anlegen
        [HttpPost]
        public async Task<ActionResult<Termin>> Post([FromBody] CreateTerminCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // Termin aktualisieren
        [HttpPut("{id}")]
        public async Task<ActionResult<Termin>> Put(Guid id, [FromBody] UpdateTerminCommand command)
        {
            if (command.Termin == null || command.Termin.Id != id)
                return BadRequest("Id mismatch");
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // Termin löschen
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteTerminCommand(id));
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
