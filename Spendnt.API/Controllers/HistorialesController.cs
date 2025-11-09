// Spendnt.API/Controllers/HistorialesController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spendnt.API.Application.Interfaces;
using Spendnt.Shared.DTOs.Historial;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

#nullable enable

namespace Spendnt.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class HistorialesController : ControllerBase
    {
        private readonly IHistorialService _historialService;

        public HistorialesController(IHistorialService historialService)
        {
            _historialService = historialService;
        }

        private string? GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HistorialDto>>> GetHistoriales()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var historiales = await _historialService.GetAsync(userId);
            return Ok(historiales);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<HistorialDto>> GetHistorial(int id)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var historial = await _historialService.GetByIdAsync(userId, id);
            if (historial == null)
            {
                return NotFound();
            }
            return Ok(historial);
        }

        [HttpPost]
        public async Task<ActionResult<HistorialDto>> PostHistorial(HistorialCreateDto historialDto)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                var created = await _historialService.CreateAsync(userId, historialDto);
                return CreatedAtAction(nameof(GetHistorial), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al guardar el historial.");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutHistorial(int id, HistorialUpdateDto historialDto)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var updated = await _historialService.UpdateAsync(userId, id, historialDto);
            if (!updated)
            {
                return NotFound("Historial no encontrado o no pertenece al usuario.");
            }
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteHistorial(int id)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var deleted = await _historialService.DeleteAsync(userId, id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}