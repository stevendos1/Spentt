// Spendnt.API/Application/Interfaces/ICalendarioEventosService.cs
using Spendnt.Shared.DTOs.CalendarioEvento;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spendnt.API.Application.Interfaces
{
    public interface ICalendarioEventosService
    {
        Task<IEnumerable<CalendarioEventoDto>> GetEventosAsync(string userId, DateTime fechaInicio, DateTime fechaFin);
    }
}
