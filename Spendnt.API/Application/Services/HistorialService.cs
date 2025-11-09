// Spendnt.API/Application/Services/HistorialService.cs
using Microsoft.EntityFrameworkCore;
using Spendnt.API.Application.Interfaces;
using Spendnt.API.Data;
using Spendnt.Shared.DTOs.Historial;
using Spendnt.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spendnt.API.Application.Services
{
    public class HistorialService : IHistorialService
    {
        private readonly IApplicationDbContext _context;

        public HistorialService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HistorialDto>> GetAsync(string userId)
        {
            var saldoId = await GetSaldoIdAsync(userId);
            if (saldoId == 0)
            {
                return Enumerable.Empty<HistorialDto>();
            }

            var historiales = await _context.Historiales
                .Where(h => h.SaldoId == saldoId)
                .Include(h => h.Categoria)
                .OrderByDescending(h => h.Fecha)
                .ToListAsync();

            return historiales.Select(MapToDto);
        }

        public async Task<HistorialDto?> GetByIdAsync(string userId, int id)
        {
            var historial = await _context.Historiales
                .Include(h => h.Categoria)
                .FirstOrDefaultAsync(h => h.Id == id && h.Saldo != null && h.Saldo.UserId == userId);

            return historial == null ? null : MapToDto(historial);
        }

        public async Task<HistorialDto> CreateAsync(string userId, HistorialCreateDto dto)
        {
            var saldoId = await EnsureSaldoIdAsync(userId);

            var historial = new Historial
            {
                Fecha = dto.Fecha,
                Monto = dto.Monto,
                Tipo = dto.Tipo,
                Descripcion = dto.Descripcion,
                CategoriaId = dto.CategoriaId,
                SaldoId = saldoId
            };

            await _context.Historiales.AddAsync(historial);
            await _context.SaveChangesAsync();

            await _context.Entry(historial).Reference(nameof(Historial.Categoria)).LoadAsync();

            return MapToDto(historial);
        }

        public async Task<bool> UpdateAsync(string userId, int id, HistorialUpdateDto dto)
        {
            var historial = await _context.Historiales
                .Include(h => h.Saldo)
                .FirstOrDefaultAsync(h => h.Id == id && h.Saldo != null && h.Saldo.UserId == userId);

            if (historial == null)
            {
                return false;
            }

            historial.Fecha = dto.Fecha;
            historial.Monto = dto.Monto;
            historial.Tipo = dto.Tipo;
            historial.Descripcion = dto.Descripcion;
            historial.CategoriaId = dto.CategoriaId;

            _context.Historiales.Update(historial);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string userId, int id)
        {
            var historial = await _context.Historiales
                .Include(h => h.Saldo)
                .FirstOrDefaultAsync(h => h.Id == id && h.Saldo != null && h.Saldo.UserId == userId);

            if (historial == null)
            {
                return false;
            }

            _context.Historiales.Remove(historial);
            await _context.SaveChangesAsync();
            return true;
        }

        private static HistorialDto MapToDto(Historial historial) => new()
        {
            Id = historial.Id,
            Fecha = historial.Fecha,
            Monto = historial.Monto,
            Tipo = historial.Tipo,
            Descripcion = historial.Descripcion,
            CategoriaId = historial.CategoriaId,
            CategoriaNombre = historial.Categoria?.Nombre ?? string.Empty
        };

        private async Task<int> GetSaldoIdAsync(string userId)
        {
            return await _context.Saldo
                .Where(s => s.UserId == userId)
                .Select(s => s.Id)
                .FirstOrDefaultAsync();
        }

        private async Task<int> EnsureSaldoIdAsync(string userId)
        {
            var saldoId = await GetSaldoIdAsync(userId);
            if (saldoId == 0)
            {
                throw new InvalidOperationException("No se encontró un saldo principal para el usuario actual.");
            }

            return saldoId;
        }
    }
}
