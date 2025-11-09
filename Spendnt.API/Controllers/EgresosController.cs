// Spendnt.API/Controllers/EgresosController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spendnt.API.Application.Interfaces;
using Spendnt.Shared.DTOs.Egreso;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

#nullable enable

namespace Spendnt.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/Egresos")]
    public class EgresosController : ControllerBase
    {
        private readonly IEgresoService _egresoService;

        public EgresosController(IEgresoService egresoService)
        {
            _egresoService = egresoService;
        }

        private string? GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EgresoDto>>> Get()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var egresos = await _egresoService.GetAsync(userId);
            return Ok(egresos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EgresoDto>> GetById(int id)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var egreso = await _egresoService.GetByIdAsync(userId, id);
            if (egreso == null)
            {
                return NotFound();
            }

            return Ok(egreso);
        }

        [HttpPost]
        public async Task<ActionResult<EgresoDto>> Post(EgresoCreateDto egresoDto)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                var created = await _egresoService.CreateAsync(userId, egresoDto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al guardar el egreso.");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, EgresoUpdateDto egresoDto)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var updated = await _egresoService.UpdateAsync(userId, id, egresoDto);
            if (!updated)
            {
                return NotFound("Egreso no encontrado o no pertenece al usuario.");
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

            var deleted = await _egresoService.DeleteAsync(userId, id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}