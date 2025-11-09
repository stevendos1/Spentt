using Microsoft.EntityFrameworkCore;
using Spendnt.API.Application.Interfaces;
using Spendnt.API.Data;
using Spendnt.Shared.Entities;
using Spendnt.Shared.Responses;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Spendnt.API.Application.Services
{
    public class AhorroService : IAhorroService
    {
        private readonly IApplicationDbContext _context;

        public AhorroService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResponse> ContribuirAMeta(int metaId, decimal monto, string userId)
        {
            if (monto <= 0)
            {
                return new OperationResponse { Success = false, Message = "El monto de la contribución debe ser positivo." };
            }

            // Iniciar una transacción explícita para asegurar la atomicidad
            using var transaction = await _context.Database.BeginTransactionAsync();

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
                
                // 1. Actualizar la meta de ahorro
                meta.MontoActual += monto;
                meta.EstaCompletada = meta.MontoActual >= meta.MontoObjetivo;

                // 2. Crear la transacción de ahorro
                var transaccionAhorro = new TransaccionAhorro
                {
                    Monto = monto,
                    Fecha = DateTime.UtcNow,
                    MetaAhorroId = metaId,
                    Descripcion = $"Contribución a meta: {meta.Nombre}"
                };
                _context.TransaccionesAhorro.Add(transaccionAhorro);

                // 3. Crear el egreso correspondiente en el saldo principal
                var egreso = new Egresos
                {
                    Egreso = monto,
                    Fecha = DateTime.UtcNow,
                    SaldoId = saldoPrincipal.Id,
                    // Opcional: Asignar una categoría específica para ahorros
                };
                _context.Egresos.Add(egreso);

                // Guardar todos los cambios en la base de datos
                await _context.SaveChangesAsync();

                // Si todo fue exitoso, confirmar la transacción
                await transaction.CommitAsync();

                return new OperationResponse { Success = true, Message = "Contribución realizada exitosamente." };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // TODO: reemplazar Console por un logger inyectado
                Console.WriteLine($"[ContribuirAMeta] Error metaId={metaId}, monto={monto}: {ex.Message}");
                return new OperationResponse { Success = false, Message = "Ocurrió un error inesperado al procesar la contribución." };
            }
        }
    }
}
