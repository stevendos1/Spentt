// Spendnt.API/Data/DataContext.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Spendnt.Shared.Entities;

namespace Spendnt.API.Data
{
    public class DataContext : IdentityDbContext<
        User,
        IdentityRole,
        string,
        IdentityUserClaim<string>,
        IdentityUserRole<string>,
        IdentityUserLogin<string>,
        IdentityRoleClaim<string>,
        IdentityUserToken<string>>, IApplicationDbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Saldo> Saldo { get; set; }
        public DbSet<Ingresos> Ingresos { get; set; }
        public DbSet<Egresos> Egresos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Historial> Historiales { get; set; }
        public DbSet<RecordatorioGasto> RecordatoriosGasto { get; set; }
        public DbSet<MetaAhorro> MetasAhorro { get; set; }
        public DbSet<TransaccionAhorro> TransaccionesAhorro { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ingresos>().Property(i => i.Ingreso).HasPrecision(18, 2);
            modelBuilder.Entity<Egresos>().Property(e => e.Egreso).HasPrecision(18, 2);
            modelBuilder.Entity<Historial>().Property(h => h.Monto).HasPrecision(18, 2);
            modelBuilder.Entity<RecordatorioGasto>().Property(r => r.MontoEstimado).HasPrecision(18, 2);
            modelBuilder.Entity<MetaAhorro>().Property(m => m.MontoObjetivo).HasPrecision(18, 2);
            modelBuilder.Entity<MetaAhorro>().Property(m => m.MontoActual).HasPrecision(18, 2);
            modelBuilder.Entity<TransaccionAhorro>().Property(t => t.Monto).HasPrecision(18, 2);

            modelBuilder.Entity<User>()
                .HasOne(u => u.SaldoPrincipal)
                .WithOne(s => s.User)
                .HasForeignKey<Saldo>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.MetasAhorro)
                .WithOne(m => m.User)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.RecordatoriosGasto)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Saldo>()
                .HasMany(s => s.Ingresos)
                .WithOne(i => i.Saldo)
                .HasForeignKey(i => i.SaldoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Saldo>()
                .HasMany(s => s.Egresos)
                .WithOne(e => e.Saldo)
                .HasForeignKey(e => e.SaldoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Saldo>()
                .HasMany<Historial>()
                .WithOne(h => h.Saldo)
                .HasForeignKey(h => h.SaldoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Categoria>()
                .HasMany<Ingresos>()
                .WithOne(i => i.Categoria)
                .HasForeignKey(i => i.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Categoria>()
                .HasMany<Egresos>()
                .WithOne(e => e.Categoria)
                .HasForeignKey(e => e.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Categoria>()
                .HasMany<Historial>()
                .WithOne(h => h.Categoria)
                .HasForeignKey(h => h.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MetaAhorro>()
                .HasMany<TransaccionAhorro>()
                .WithOne(t => t.MetaAhorro)
                .HasForeignKey(t => t.MetaAhorroId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}