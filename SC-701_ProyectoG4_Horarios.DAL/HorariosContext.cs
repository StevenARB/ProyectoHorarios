using Microsoft.EntityFrameworkCore;

namespace SC_701_ProyectoG4_Horarios.DAL
{
    public class HorariosContext : DbContext
    {
        public HorariosContext() {}
        public HorariosContext(DbContextOptions<HorariosContext> options) : base(options) {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer();
        }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Profesor> Profesores { get; set; }

        public DbSet<Aula> Aulas { get; set; }

        public DbSet<Clase> Clases { get; set; }

        public DbSet<Reservacion> Reservaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación Usuario - Profesor (uno a uno)
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Profesor)
                .WithOne(p => p.Usuario)
                .HasForeignKey<Profesor>(p => p.UsuarioId)
                .IsRequired();

            // Relación Aula - Reservacion (uno a muchos)
            modelBuilder.Entity<Aula>()
                .HasMany(a => a.Reservaciones)
                .WithOne(r => r.Aula)
                .HasForeignKey(r => r.AulaId)
                .IsRequired();

            // Relación Profesor - Reservacion (uno a muchos)
            modelBuilder.Entity<Profesor>()
                .HasMany(p => p.Reservaciones)
                .WithOne(r => r.Profesor)
                .HasForeignKey(r => r.ProfesorId)
                .IsRequired();

            // Relación Clase - Reservacion (uno a muchos, opcional)
            modelBuilder.Entity<Clase>()
                .HasMany(c => c.Reservaciones)
                .WithOne(r => r.Clase)
                .HasForeignKey(r => r.ClaseId)
                .OnDelete(DeleteBehavior.SetNull);

            // Relación Usuario (creación) - Reservacion (uno a muchos)
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.ReservacionesCreacion)
                .WithOne(r => r.UsuarioCreacion)
                .HasForeignKey(r => r.UsuarioCreacionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Usuario (modificación) - Reservacion (uno a muchos, opcional)
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.ReservacionesModificacion)
                .WithOne(r => r.UsuarioModificacion)
                .HasForeignKey(r => r.UsuarioModificacionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
