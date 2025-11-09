// Spendnt.API/Application/Services/EgresoService.cs
using Microsoft.EntityFrameworkCore;
using Spendnt.API.Application.Interfaces;
using Spendnt.API.Data;
using Spendnt.Shared.DTOs.Egreso;
using Spendnt.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spendnt.API.Application.Services
{
    public class EgresoService : IEgresoService
    {
        private readonly IApplicationDbContext _context;

        public EgresoService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EgresoDto>> GetAsync(string userId)
        {
            var saldoId = await GetSaldoIdAsync(userId);
            if (saldoId == 0)
            {
                return Enumerable.Empty<EgresoDto>();
            }

            var egresos = await _context.Egresos
                .Where(e => e.SaldoId == saldoId)
                .Include(e => e.Categoria)
                .OrderByDescending(e => e.Fecha)
                .ToListAsync();

            return egresos.Select(MapToDto);
        }

        public async Task<EgresoDto?> GetByIdAsync(string userId, int id)
        {
            var egreso = await _context.Egresos
                .Include(e => e.Categoria)
                .FirstOrDefaultAsync(e => e.Id == id && e.Saldo != null && e.Saldo.UserId == userId);

            return egreso == null ? null : MapToDto(egreso);
        }

        public async Task<EgresoDto> CreateAsync(string userId, EgresoCreateDto dto)
        {
            var saldoId = await EnsureSaldoIdAsync(userId);

            var egreso = new Egresos
            {
                Egreso = dto.Monto,
                Fecha = dto.Fecha,
                CategoriaId = dto.CategoriaId,
                SaldoId = saldoId
            };

            await _context.Egresos.AddAsync(egreso);
            await _context.SaveChangesAsync();

            await _context.Entry(egreso).Reference(nameof(Egresos.Categoria)).LoadAsync();

            return MapToDto(egreso);
        }

        public async Task<bool> UpdateAsync(string userId, int id, EgresoUpdateDto dto)
        {
            var egreso = await _context.Egresos
                .Include(e => e.Saldo)
                .FirstOrDefaultAsync(e => e.Id == id && e.Saldo != null && e.Saldo.UserId == userId);

            if (egreso == null)
            {
                return false;
            }

            egreso.Egreso = dto.Monto;
            egreso.Fecha = dto.Fecha;
            egreso.CategoriaId = dto.CategoriaId;

            _context.Egresos.Update(egreso);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string userId, int id)
        {
            var egreso = await _context.Egresos
                .Include(e => e.Saldo)
                .FirstOrDefaultAsync(e => e.Id == id && e.Saldo != null && e.Saldo.UserId == userId);

            if (egreso == null)
            {
                return false;
            }

            _context.Egresos.Remove(egreso);
            await _context.SaveChangesAsync();
            return true;
        }

        private static EgresoDto MapToDto(Egresos egreso) => new()
        {
            Id = egreso.Id,
            Monto = egreso.Egreso,
            Fecha = egreso.Fecha,
            CategoriaId = egreso.CategoriaId,
            CategoriaNombre = egreso.Categoria?.Nombre ?? string.Empty
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
