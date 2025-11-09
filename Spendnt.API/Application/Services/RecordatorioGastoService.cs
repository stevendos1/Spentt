// Spendnt.API/Application/Services/RecordatorioGastoService.cs
using Microsoft.EntityFrameworkCore;
using Spendnt.API.Application.Interfaces;
using Spendnt.API.Data;
using Spendnt.Shared.DTOs.RecordatorioGasto;
using Spendnt.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spendnt.API.Application.Services
{
    public class RecordatorioGastoService : IRecordatorioGastoService
    {
        private readonly IApplicationDbContext _context;

        public RecordatorioGastoService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RecordatorioGastoDto>> GetAsync(string userId)
        {
            var recordatorios = await _context.RecordatoriosGasto
                .Where(r => r.UserId == userId)
                .OrderBy(r => r.FechaProgramada)
                .ToListAsync();

            return recordatorios.Select(MapToDto);
        }

        public async Task<RecordatorioGastoDto?> GetByIdAsync(string userId, int id)
        {
            var recordatorio = await _context.RecordatoriosGasto
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

            return recordatorio == null ? null : MapToDto(recordatorio);
        }

        public async Task<RecordatorioGastoDto> CreateAsync(string userId, RecordatorioGastoCreateDto dto)
        {
            var recordatorio = new RecordatorioGasto
            {
                Titulo = dto.Titulo,
                MontoEstimado = dto.MontoEstimado,
                FechaProgramada = dto.FechaProgramada,
                Notas = dto.Notas,
                UserId = userId
            };

            await _context.RecordatoriosGasto.AddAsync(recordatorio);
            await _context.SaveChangesAsync();

            return MapToDto(recordatorio);
        }

        public async Task<bool> UpdateAsync(string userId, int id, RecordatorioGastoUpdateDto dto)
        {
            var recordatorio = await _context.RecordatoriosGasto
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

            if (recordatorio == null)
            {
                return false;
            }

            recordatorio.Titulo = dto.Titulo;
            recordatorio.MontoEstimado = dto.MontoEstimado;
            recordatorio.FechaProgramada = dto.FechaProgramada;
            recordatorio.Notas = dto.Notas;

            _context.RecordatoriosGasto.Update(recordatorio);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string userId, int id)
        {
            var recordatorio = await _context.RecordatoriosGasto
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

            if (recordatorio == null)
            {
                return false;
            }

            _context.RecordatoriosGasto.Remove(recordatorio);
            await _context.SaveChangesAsync();
            return true;
        }

        private static RecordatorioGastoDto MapToDto(RecordatorioGasto recordatorio) => new()
        {
            Id = recordatorio.Id,
            Titulo = recordatorio.Titulo,
            MontoEstimado = recordatorio.MontoEstimado,
            FechaProgramada = recordatorio.FechaProgramada,
            Notas = recordatorio.Notas
        };
    }
}
