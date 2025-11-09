// Spendnt.API/Application/Services/CalendarioEventosService.cs
using Microsoft.EntityFrameworkCore;
using Spendnt.API.Application.Interfaces;
using Spendnt.API.Data;
using Spendnt.Shared.DTOs.CalendarioEvento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spendnt.API.Application.Services
{
    public class CalendarioEventosService : ICalendarioEventosService
    {
        private readonly IApplicationDbContext _context;

        public CalendarioEventosService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CalendarioEventoDto>> GetEventosAsync(string userId, DateTime fechaInicio, DateTime fechaFin)
        {
            var eventos = new List<CalendarioEventoDto>();

            var saldoId = await _context.Saldo
                .Where(s => s.UserId == userId)
                .Select(s => s.Id)
                .FirstOrDefaultAsync();

            if (saldoId == 0)
            {
                return eventos;
            }

            var ingresos = await _context.Ingresos
                .Where(i => i.SaldoId == saldoId && i.Fecha >= fechaInicio && i.Fecha <= fechaFin)
                .Select(i => new
                {
                    i.Id,
                    i.Fecha,
                    i.Ingreso,
                    CategoriaNombre = i.Categoria != null ? i.Categoria.Nombre : null
                })
                .ToListAsync();

            eventos.AddRange(ingresos.Select(i => new CalendarioEventoDto
            {
                Id = $"ingreso-{i.Id}",
                Title = $"Ingreso: {i.CategoriaNombre ?? "Sin categoría"} ({i.Ingreso:C})",
                Start = i.Fecha,
                Tipo = "ingreso",
                Monto = i.Ingreso,
                Color = "green"
            }));

            var egresos = await _context.Egresos
                .Where(e => e.SaldoId == saldoId && e.Fecha >= fechaInicio && e.Fecha <= fechaFin)
                .Select(e => new
                {
                    e.Id,
                    e.Fecha,
                    e.Egreso,
                    CategoriaNombre = e.Categoria != null ? e.Categoria.Nombre : null
                })
                .ToListAsync();

            eventos.AddRange(egresos.Select(e => new CalendarioEventoDto
            {
                Id = $"egreso-{e.Id}",
                Title = $"Egreso: {e.CategoriaNombre ?? "Sin categoría"} ({e.Egreso:C})",
                Start = e.Fecha,
                Tipo = "egreso",
                Monto = e.Egreso,
                Color = "red"
            }));

            var recordatorios = await _context.RecordatoriosGasto
                .Where(r => r.UserId == userId && r.FechaProgramada >= fechaInicio && r.FechaProgramada <= fechaFin)
                .Select(r => new
                {
                    r.Id,
                    r.Titulo,
                    r.MontoEstimado,
                    r.FechaProgramada
                })
                .ToListAsync();

            eventos.AddRange(recordatorios.Select(r => new CalendarioEventoDto
            {
                Id = $"recordatorio-{r.Id}",
                Title = $"Recordatorio: {r.Titulo} ({r.MontoEstimado:C})",
                Start = r.FechaProgramada,
                Tipo = "recordatorio",
                Monto = r.MontoEstimado,
                Color = "orange"
            }));

            var metas = await _context.MetasAhorro
                .Where(m => m.UserId == userId && m.FechaObjetivo.HasValue && m.FechaObjetivo.Value >= fechaInicio && m.FechaObjetivo.Value <= fechaFin)
                .Select(m => new
                {
                    m.Id,
                    m.Nombre,
                    m.MontoObjetivo,
                    m.FechaObjetivo
                })
                .ToListAsync();

            eventos.AddRange(metas.Select(m => new CalendarioEventoDto
            {
                Id = $"meta-{m.Id}",
                Title = $"Meta: {m.Nombre} (Objetivo: {m.MontoObjetivo:C})",
                Start = m.FechaObjetivo!.Value,
                Tipo = "meta",
                Monto = m.MontoObjetivo,
                Color = "blue"
            }));

            return eventos.OrderBy(e => e.Start).ToList();
        }
    }
}
