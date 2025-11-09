// Spendnt.API/Controllers/SaldoController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spendnt.API.Application.Interfaces;
using Spendnt.Shared.DTOs.Saldo;
using System.Security.Claims;
using System.Threading.Tasks;

#nullable enable

namespace Spendnt.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/Saldo")]
    public class SaldoController : ControllerBase
    {
        private readonly ISaldoService _saldoService;

        public SaldoController(ISaldoService saldoService)
        {
            _saldoService = saldoService;
        }

        private string? GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpGet("actual")]
        public async Task<ActionResult<SaldoDto>> GetCurrentSaldo()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var saldo = await _saldoService.GetCurrentAsync(userId);
            if (saldo == null)
            {
                return NotFound("No se encontró un registro de saldo principal para el usuario actual.");
            }

            return Ok(saldo);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SaldoDto>> GetSaldoById(int id)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var saldo = await _saldoService.GetByIdAsync(userId, id);
            if (saldo == null)
            {
                return NotFound();
            }

            return Ok(saldo);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<SaldoDto>> PutSaldo(int id, SaldoUpdateDto saldoDto)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var updated = await _saldoService.UpdateAsync(userId, id, saldoDto);
            if (updated == null)
            {
                return NotFound("El saldo a actualizar no fue encontrado o no pertenece al usuario.");
            }

            return Ok(updated);
        }
    }
}