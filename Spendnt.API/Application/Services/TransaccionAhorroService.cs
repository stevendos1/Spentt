// Spendnt.API/Application/Services/TransaccionAhorroService.cs
using Microsoft.EntityFrameworkCore;
using Spendnt.API.Application.Interfaces;
using Spendnt.API.Data;
using Spendnt.Shared.DTOs.TransaccionAhorro;
using Spendnt.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spendnt.API.Application.Services
{
    public class TransaccionAhorroService : ITransaccionAhorroService
    {
        private readonly IApplicationDbContext _context;

        public TransaccionAhorroService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TransaccionAhorroDto>?> GetAsync(string userId, int metaAhorroId)
        {
            var meta = await GetMetaAsync(userId, metaAhorroId);
            if (meta == null)
            {
                return null;
            }

            return await _context.TransaccionesAhorro
                .Where(t => t.MetaAhorroId == metaAhorroId)
                .OrderByDescending(t => t.Fecha)
                .Select(t => new TransaccionAhorroDto
                {
                    Id = t.Id,
                    MetaAhorroId = t.MetaAhorroId,
                    Monto = t.Monto,
                    Fecha = t.Fecha,
                    Descripcion = t.Descripcion
                })
                .ToListAsync();
        }

        public async Task<TransaccionAhorroDto?> CreateAsync(string userId, int metaAhorroId, TransaccionAhorroCreateDto dto)
        {
            var meta = await GetMetaAsync(userId, metaAhorroId);
            if (meta == null)
            {
                return null;
            }

            var transaccion = new TransaccionAhorro
            {
                MetaAhorroId = metaAhorroId,
                Monto = dto.Monto,
                Descripcion = dto.Descripcion ?? string.Empty,
                Fecha = DateTime.UtcNow
            };

            await _context.TransaccionesAhorro.AddAsync(transaccion);

            meta.MontoActual += dto.Monto;
            if (meta.MontoActual < 0)
            {
                meta.MontoActual = 0;
            }

            meta.EstaCompletada = meta.MontoActual >= meta.MontoObjetivo;

            await _context.SaveChangesAsync();

            return new TransaccionAhorroDto
            {
                Id = transaccion.Id,
                MetaAhorroId = transaccion.MetaAhorroId,
                Monto = transaccion.Monto,
                Fecha = transaccion.Fecha,
                Descripcion = string.IsNullOrWhiteSpace(transaccion.Descripcion) ? null : transaccion.Descripcion
            };
        }

        private Task<MetaAhorro?> GetMetaAsync(string userId, int metaAhorroId)
        {
            return _context.MetasAhorro
                .FirstOrDefaultAsync(m => m.Id == metaAhorroId && m.UserId == userId);
        }
    }
}
