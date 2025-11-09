// Spendnt.API/Application/Services/SaldoService.cs
using Microsoft.EntityFrameworkCore;
using Spendnt.API.Application.Interfaces;
using Spendnt.API.Data;
using Spendnt.Shared.DTOs.Saldo;
using Spendnt.Shared.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spendnt.API.Application.Services
{
    public class SaldoService : ISaldoService
    {
        private readonly IApplicationDbContext _context;

        public SaldoService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SaldoDto?> GetCurrentAsync(string userId)
        {
            var saldo = await _context.Saldo
                .Where(s => s.UserId == userId)
                .Include(s => s.Ingresos)
                    .ThenInclude(i => i.Categoria)
                .Include(s => s.Egresos)
                    .ThenInclude(e => e.Categoria)
                .FirstOrDefaultAsync();

            return saldo == null ? null : MapToDto(saldo);
        }

        public async Task<SaldoDto?> GetByIdAsync(string userId, int id)
        {
            var saldo = await _context.Saldo
                .Where(s => s.UserId == userId && s.Id == id)
                .Include(s => s.Ingresos)
                    .ThenInclude(i => i.Categoria)
                .Include(s => s.Egresos)
                    .ThenInclude(e => e.Categoria)
                .FirstOrDefaultAsync();

            return saldo == null ? null : MapToDto(saldo);
        }

        public async Task<SaldoDto?> UpdateAsync(string userId, int id, SaldoUpdateDto dto)
        {
            var saldo = await _context.Saldo
                .Where(s => s.UserId == userId && s.Id == id)
                .Include(s => s.Ingresos)
                    .ThenInclude(i => i.Categoria)
                .Include(s => s.Egresos)
                    .ThenInclude(e => e.Categoria)
                .FirstOrDefaultAsync();

            if (saldo == null)
            {
                return null;
            }

            saldo.TotalSaldo = dto.TotalSaldo;

            await _context.SaveChangesAsync();

            return MapToDto(saldo);
        }

        private static SaldoDto MapToDto(Saldo saldo)
        {
            var ingresos = saldo.Ingresos ?? new List<Ingresos>();
            var egresos = saldo.Egresos ?? new List<Egresos>();

            var totalIngresos = ingresos.Sum(i => i.Ingreso);
            var totalEgresos = egresos.Sum(e => e.Egreso);

            return new SaldoDto
            {
                Id = saldo.Id,
                TotalSaldo = saldo.TotalSaldo,
                TotalIngresos = totalIngresos,
                TotalEgresos = totalEgresos,
                TotalSaldoCalculado = totalIngresos - totalEgresos,
                Ingresos = ingresos.Select(i => new SaldoMovimientoDto
                {
                    Id = i.Id,
                    Monto = i.Ingreso,
                    Fecha = i.Fecha,
                    CategoriaNombre = i.Categoria?.Nombre
                }).ToList(),
                Egresos = egresos.Select(e => new SaldoMovimientoDto
                {
                    Id = e.Id,
                    Monto = e.Egreso,
                    Fecha = e.Fecha,
                    CategoriaNombre = e.Categoria?.Nombre
                }).ToList()
            };
        }
    }
}
