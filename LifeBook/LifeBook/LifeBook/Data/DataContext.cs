using LifeBook.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LifeBook.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<So> Sos { get; set; }
        public DbSet<Attack> Attacks { get; set; }
        public DbSet<Tool> Tools { get; set; }
        public DbSet<Protocol> Protocols { get; set; } // Agregar DbSet para Protocol

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar índice único para el nombre de So
            modelBuilder.Entity<So>()
                .HasIndex(so => so.Name)
                .IsUnique();

            // Configuración de la relación entre So y Attack
            modelBuilder.Entity<So>()
                .HasMany(so => so.Attacks)
                .WithOne(a => a.So)
                .HasForeignKey(a => a.SoId)
                .OnDelete(DeleteBehavior.Restrict);  // Cambiado a Restrict para evitar cascada

            // Configuración de la relación entre Attack y Tool
            modelBuilder.Entity<Attack>()
                .HasMany(a => a.Tools)
                .WithOne(t => t.Attack)
                .HasForeignKey(t => t.AttackId)
                .OnDelete(DeleteBehavior.Restrict);  // Cambiado a Restrict para evitar cascada

            // Configuración de la relación directa entre So y Tool
            modelBuilder.Entity<So>()
                .HasMany(so => so.Tools)
                .WithOne(t => t.So)
                .HasForeignKey(t => t.SoId)
                .OnDelete(DeleteBehavior.Restrict);  // Cambiado a Restrict para evitar cascada

            // Configuración de la entidad Protocol
            modelBuilder.Entity<Protocol>()
                .HasIndex(p => p.Name) // Configurar índice único para el nombre de Protocol
                .IsUnique();
        }
    }
}
