// Spendnt.API/Application/Services/IngresoService.cs
using Microsoft.EntityFrameworkCore;
using Spendnt.API.Application.Interfaces;
using Spendnt.API.Data;
using Spendnt.Shared.DTOs.Ingreso;
using Spendnt.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spendnt.API.Application.Services
{
    public class IngresoService : IIngresoService
    {
        private readonly IApplicationDbContext _context;

        public IngresoService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<IngresoDto>> GetAsync(string userId)
        {
            var saldoId = await GetSaldoIdAsync(userId);
            if (saldoId == 0)
            {
                return Enumerable.Empty<IngresoDto>();
            }

            var ingresos = await _context.Ingresos
                .Where(i => i.SaldoId == saldoId)
                .Include(i => i.Categoria)
                .OrderByDescending(i => i.Fecha)
                .ToListAsync();

            return ingresos.Select(MapToDto);
        }

        public async Task<IngresoDto?> GetByIdAsync(string userId, int id)
        {
            var ingreso = await _context.Ingresos
                .Include(i => i.Categoria)
                .FirstOrDefaultAsync(i => i.Id == id && i.Saldo != null && i.Saldo.UserId == userId);

            return ingreso == null ? null : MapToDto(ingreso);
        }

        public async Task<IngresoDto> CreateAsync(string userId, IngresoCreateDto dto)
        {
            var saldoId = await EnsureSaldoIdAsync(userId);

            var ingreso = new Ingresos
            {
                Ingreso = dto.Monto,
                Fecha = dto.Fecha,
                CategoriaId = dto.CategoriaId,
                SaldoId = saldoId
            };

            await _context.Ingresos.AddAsync(ingreso);
            await _context.SaveChangesAsync();

            await _context.Entry(ingreso).Reference(nameof(Ingresos.Categoria)).LoadAsync();

            return MapToDto(ingreso);
        }

        public async Task<bool> UpdateAsync(string userId, int id, IngresoUpdateDto dto)
        {
            var ingreso = await _context.Ingresos
                .Include(i => i.Saldo)
                .FirstOrDefaultAsync(i => i.Id == id && i.Saldo != null && i.Saldo.UserId == userId);

            if (ingreso == null)
            {
                return false;
            }

            ingreso.Ingreso = dto.Monto;
            ingreso.Fecha = dto.Fecha;
            ingreso.CategoriaId = dto.CategoriaId;

            _context.Ingresos.Update(ingreso);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string userId, int id)
        {
            var ingreso = await _context.Ingresos
                .Include(i => i.Saldo)
                .FirstOrDefaultAsync(i => i.Id == id && i.Saldo != null && i.Saldo.UserId == userId);

            if (ingreso == null)
            {
                return false;
            }

            _context.Ingresos.Remove(ingreso);
            await _context.SaveChangesAsync();
            return true;
        }

        private static IngresoDto MapToDto(Ingresos ingreso) => new()
        {
            Id = ingreso.Id,
            Monto = ingreso.Ingreso,
            Fecha = ingreso.Fecha,
            CategoriaId = ingreso.CategoriaId,
            CategoriaNombre = ingreso.Categoria?.Nombre ?? string.Empty
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
