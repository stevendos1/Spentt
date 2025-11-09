// Spendnt.API/Application/Services/MetaAhorroService.cs
using Microsoft.EntityFrameworkCore;
using Spendnt.API.Application.Interfaces;
using Spendnt.API.Data;
using Spendnt.Shared.DTOs.MetaAhorro;
using Spendnt.Shared.Entities;
using Spendnt.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spendnt.API.Application.Services
{
    public class MetaAhorroService : IMetaAhorroService
    {
        private readonly IApplicationDbContext _context;

        public MetaAhorroService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MetaAhorroDto>> GetAsync(string userId)
        {
            var metas = await _context.MetasAhorro
                .Where(m => m.UserId == userId)
                .OrderByDescending(m => m.FechaCreacion)
                .ToListAsync();

            return metas.Select(MapToDto);
        }

        public async Task<MetaAhorroDto?> GetByIdAsync(string userId, int id)
        {
            var meta = await _context.MetasAhorro
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            return meta == null ? null : MapToDto(meta);
        }

        public async Task<MetaAhorroDto> CreateAsync(string userId, MetaAhorroCreateDto dto)
        {
            var meta = new MetaAhorro
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                MontoObjetivo = dto.MontoObjetivo,
                MontoActual = dto.MontoActual,
                FechaObjetivo = dto.FechaObjetivo,
                EstaCompletada = dto.EstaCompletada,
                FechaCreacion = DateTime.UtcNow,
                UserId = userId
            };

            await _context.MetasAhorro.AddAsync(meta);
            await _context.SaveChangesAsync();

            return MapToDto(meta);
        }

        public async Task<bool> UpdateAsync(string userId, int id, MetaAhorroUpdateDto dto)
        {
            var meta = await _context.MetasAhorro
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (meta == null)
            {
                return false;
            }

            meta.Nombre = dto.Nombre;
            meta.Descripcion = dto.Descripcion;
            meta.MontoObjetivo = dto.MontoObjetivo;
            meta.MontoActual = dto.MontoActual;
            meta.FechaObjetivo = dto.FechaObjetivo;
            meta.EstaCompletada = dto.EstaCompletada;

            _context.MetasAhorro.Update(meta);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string userId, int id)
        {
            var meta = await _context.MetasAhorro
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (meta == null)
            {
                return false;
            }

            _context.MetasAhorro.Remove(meta);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<OperationResponse> ContribuirAsync(int metaId, decimal monto, string userId)
        {
            if (monto <= 0)
            {
                return new OperationResponse { Success = false, Message = "El monto de la contribución debe ser positivo." };
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var meta = await _context.MetasAhorro
                    .FirstOrDefaultAsync(m => m.Id == metaId && m.UserId == userId);

                if (meta == null)
                {
                    return new OperationResponse { Success = false, Message = "La meta de ahorro no fue encontrada o no pertenece al usuario." };
                }

                var saldoPrincipal = await _context.Saldo
                    .FirstOrDefaultAsync(s => s.UserId == userId);

                if (saldoPrincipal == null)
                {
                    return new OperationResponse { Success = false, Message = "No se encontró el saldo principal del usuario." };
                }

                meta.MontoActual += monto;
                meta.EstaCompletada = meta.MontoActual >= meta.MontoObjetivo;

                var transaccionAhorro = new TransaccionAhorro
                {
                    Monto = monto,
                    Fecha = DateTime.UtcNow,
                    MetaAhorroId = metaId,
                    Descripcion = $"Contribución a meta: {meta.Nombre}"
                };
                await _context.TransaccionesAhorro.AddAsync(transaccionAhorro);

                var categoriaContribucion = await _context.Categorias
                    .FirstOrDefaultAsync(c => c.Nombre == "Metas de Ahorro");

                if (categoriaContribucion == null)
                {
                    categoriaContribucion = new Categoria
                    {
                        Nombre = "Metas de Ahorro",
                        Descripcion = "Movimientos asociados a contribuciones de metas de ahorro"
                    };
                    await _context.Categorias.AddAsync(categoriaContribucion);
                }

                var egreso = new Egresos
                {
                    Egreso = monto,
                    Fecha = DateTime.UtcNow,
                    SaldoId = saldoPrincipal.Id,
                    Categoria = categoriaContribucion
                };
                await _context.Egresos.AddAsync(egreso);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new OperationResponse { Success = true, Message = "Contribución realizada exitosamente." };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return new OperationResponse { Success = false, Message = "Ocurrió un error inesperado al procesar la contribución." };
            }
        }

        private static MetaAhorroDto MapToDto(MetaAhorro meta) => new()
        {
            Id = meta.Id,
            Nombre = meta.Nombre,
            Descripcion = meta.Descripcion,
            MontoObjetivo = meta.MontoObjetivo,
            MontoActual = meta.MontoActual,
            FechaObjetivo = meta.FechaObjetivo,
            FechaCreacion = meta.FechaCreacion,
            EstaCompletada = meta.EstaCompletada
        };
    }
}
