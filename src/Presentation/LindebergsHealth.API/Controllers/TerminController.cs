using LindebergsHealth.Application.Termine.Commands;
using LindebergsHealth.Application.Termine.Dto;
using LindebergsHealth.Application.Termine.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mapster;
using LindebergsHealth.Domain.Entities;


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
        public async Task<ActionResult<List<TerminListDto>>> Get()
        {
            var result = await _mediator.Send(new GetAllTermineQuery());
            return Ok(result);
        }

        // Termin nach Id
        [HttpGet("{id}")]
        public async Task<ActionResult<TerminDetailDto>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetTerminByIdQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }

        // Termin anlegen
        [HttpPost]
        public async Task<ActionResult<TerminDetailDto>> Post([FromBody] CreateTerminDto dto)
        {
            var command = new CreateTerminCommand(dto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // Termin aktualisieren
        [HttpPut("{id}")]
        public async Task<ActionResult<TerminDetailDto>> Put(Guid id, [FromBody] UpdateTerminDto dto)
        {
            if (dto == null || dto.Id != id)
                return BadRequest("Id mismatch");
            var termin = dto.Adapt<Termin>();
            var command = new UpdateTerminCommand(termin);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // Termin löschen
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _mediator.Send(new DeleteTerminCommand(id));
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}
