// Spendnt.API/Controllers/MetasAhorroController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spendnt.API.Application.Interfaces;
using Spendnt.Shared.DTOs.MetaAhorro;
using Spendnt.Shared.Responses;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

#nullable enable

namespace Spendnt.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MetasAhorroController : ControllerBase
    {
        private readonly IMetaAhorroService _metaAhorroService;

        public MetasAhorroController(IMetaAhorroService metaAhorroService)
        {
            _metaAhorroService = metaAhorroService;
        }

        private string? GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MetaAhorroDto>>> GetMetasAhorro()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Usuario no autenticado.");
            }

            var metas = await _metaAhorroService.GetAsync(userId);
            return Ok(metas);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MetaAhorroDto>> GetMetaAhorro(int id)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Usuario no autenticado.");
            }

            var metaAhorro = await _metaAhorroService.GetByIdAsync(userId, id);
            if (metaAhorro == null)
            {
                return NotFound($"No se encontró la meta de ahorro con ID {id} para el usuario actual.");
            }
            return Ok(metaAhorro);
        }

        [HttpPost]
        public async Task<ActionResult<MetaAhorroDto>> PostMetaAhorro(MetaAhorroCreateDto metaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("No se pudo identificar al usuario para crear la meta.");
            }

            try
            {
                var created = await _metaAhorroService.CreateAsync(userId, metaDto);
                return CreatedAtAction(nameof(GetMetaAhorro), new { id = created.Id }, created);
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al guardar la meta de ahorro.");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutMetaAhorro(int id, MetaAhorroUpdateDto metaAhorroUpdateData)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Usuario no autenticado.");
            }

            var updated = await _metaAhorroService.UpdateAsync(userId, id, metaAhorroUpdateData);
            if (!updated)
            {
                return NotFound($"No se encontró la meta de ahorro con ID {id} para el usuario actual, o no tienes permiso para modificarla.");
            }
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMetaAhorro(int id)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Usuario no autenticado.");
            }

            var deleted = await _metaAhorroService.DeleteAsync(userId, id);
            if (!deleted)
            {
                return NotFound($"No se encontró la meta de ahorro con ID {id} para el usuario actual, o no tienes permiso para eliminarla.");
            }
            return NoContent();
        }

        [HttpPost("{id:int}/contribuir")]
        public async Task<ActionResult<OperationResponse>> ContribuirMetaAhorro(int id, [FromBody] decimal monto)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Usuario no autenticado.");
            }

            var response = await _metaAhorroService.ContribuirAsync(id, monto, userId);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}