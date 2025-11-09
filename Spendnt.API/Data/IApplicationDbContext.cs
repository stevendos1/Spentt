using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Spendnt.Shared.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Spendnt.API.Data
{
    // Abstracción de alto nivel para desacoplar controladores de la implementación concreta de EF Core
    public interface IApplicationDbContext
    {
        DbSet<Saldo> Saldo { get; }
        DbSet<Ingresos> Ingresos { get; }
        DbSet<Egresos> Egresos { get; }
        DbSet<Categoria> Categorias { get; }
        DbSet<Historial> Historiales { get; }
        DbSet<RecordatorioGasto> RecordatoriosGasto { get; }
        DbSet<MetaAhorro> MetasAhorro { get; }
        DbSet<TransaccionAhorro> TransaccionesAhorro { get; }
        DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        EntityEntry Entry(object entity);
    }
}
