// Spendnt.API/Controllers/TransaccionesAhorroController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spendnt.API.Application.Interfaces;
using Spendnt.Shared.DTOs.TransaccionAhorro;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

#nullable enable

namespace Spendnt.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/metasahorro/{metaAhorroId}/transacciones")]
    public class TransaccionesAhorroController : ControllerBase
    {
        private readonly ITransaccionAhorroService _transaccionAhorroService;

        public TransaccionesAhorroController(ITransaccionAhorroService transaccionAhorroService)
        {
            _transaccionAhorroService = transaccionAhorroService;
        }

        private string? GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransaccionAhorroDto>>> GetTransaccionesAhorro(int metaAhorroId)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var transacciones = await _transaccionAhorroService.GetAsync(userId, metaAhorroId);
            if (transacciones == null)
            {
                return NotFound("Meta no encontrada o no pertenece al usuario.");
            }

            return Ok(transacciones);
        }

        [HttpPost]
        public async Task<ActionResult<TransaccionAhorroDto>> PostTransaccionAhorro(int metaAhorroId, TransaccionAhorroCreateDto transaccionDto)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var created = await _transaccionAhorroService.CreateAsync(userId, metaAhorroId, transaccionDto);
            if (created == null)
            {
                return NotFound("Meta no encontrada o no pertenece al usuario.");
            }

            return CreatedAtAction(nameof(GetTransaccionesAhorro), new { metaAhorroId }, created);
        }
    }
}