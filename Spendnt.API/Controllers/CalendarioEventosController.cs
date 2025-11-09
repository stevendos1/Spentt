// Spendnt.API/Controllers/CalendarioEventosController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spendnt.API.Application.Interfaces;
using Spendnt.Shared.DTOs.CalendarioEvento;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

#nullable enable

namespace Spendnt.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/calendarioeventos")]
    public class CalendarioEventosController : ControllerBase
    {
        private readonly ICalendarioEventosService _calendarioEventosService;

        public CalendarioEventosController(ICalendarioEventosService calendarioEventosService)
        {
            _calendarioEventosService = calendarioEventosService;
        }

        private string? GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CalendarioEventoDto>>> GetEventos(
            [FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                var eventos = await _calendarioEventosService.GetEventosAsync(userId, fechaInicio, fechaFin);
                return Ok(eventos);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al recuperar los eventos del calendario.");
            }
        }
    }
}