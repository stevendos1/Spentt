// Spendnt.API/Controllers/RecordatoriosGastoController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spendnt.API.Application.Interfaces;
using Spendnt.Shared.DTOs.RecordatorioGasto;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

#nullable enable

namespace Spendnt.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RecordatoriosGastoController : ControllerBase
    {
        private readonly IRecordatorioGastoService _recordatorioGastoService;

        public RecordatoriosGastoController(IRecordatorioGastoService recordatorioGastoService)
        {
            _recordatorioGastoService = recordatorioGastoService;
        }

        private string? GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecordatorioGastoDto>>> Get()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var recordatorios = await _recordatorioGastoService.GetAsync(userId);
            return Ok(recordatorios);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RecordatorioGastoDto>> Get(int id)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var recordatorio = await _recordatorioGastoService.GetByIdAsync(userId, id);
            if (recordatorio == null)
            {
                return NotFound();
            }
            return Ok(recordatorio);
        }

        [HttpPost]
        public async Task<ActionResult<RecordatorioGastoDto>> Post(RecordatorioGastoCreateDto recordatorioDto)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var created = await _recordatorioGastoService.CreateAsync(userId, recordatorioDto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, RecordatorioGastoUpdateDto recordatorioDto)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var updated = await _recordatorioGastoService.UpdateAsync(userId, id, recordatorioDto);
            if (!updated)
            {
                return NotFound("Recordatorio no encontrado o no pertenece al usuario.");
            }
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var deleted = await _recordatorioGastoService.DeleteAsync(userId, id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}