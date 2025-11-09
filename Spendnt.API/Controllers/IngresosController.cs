// Spendnt.API/Controllers/IngresosController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spendnt.API.Application.Interfaces;
using Spendnt.Shared.DTOs.Ingreso;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

#nullable enable

namespace Spendnt.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/Ingresos")]
    public class IngresosController : ControllerBase
    {
        private readonly IIngresoService _ingresoService;

        public IngresosController(IIngresoService ingresoService)
        {
            _ingresoService = ingresoService;
        }

        private string? GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngresoDto>>> Get()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var ingresos = await _ingresoService.GetAsync(userId);
            return Ok(ingresos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<IngresoDto>> Get(int id)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var ingreso = await _ingresoService.GetByIdAsync(userId, id);
            if (ingreso == null)
            {
                return NotFound();
            }
            return Ok(ingreso);
        }

        [HttpPost]
        public async Task<ActionResult<IngresoDto>> Post(IngresoCreateDto ingresoDto)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                var created = await _ingresoService.CreateAsync(userId, ingresoDto);
                return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al guardar el ingreso.");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, IngresoUpdateDto ingresoDto)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var updated = await _ingresoService.UpdateAsync(userId, id, ingresoDto);
            if (!updated)
            {
                return NotFound("Ingreso no encontrado o no pertenece al usuario.");
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

            var deleted = await _ingresoService.DeleteAsync(userId, id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}