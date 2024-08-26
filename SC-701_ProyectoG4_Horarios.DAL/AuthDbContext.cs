using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SC_701_ProyectoG4_Horarios.DAL
{
    public class AuthDbContext : IdentityDbContext<Usuario>
    {
        public AuthDbContext() { }

        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Reservacion>()
                .Ignore(r => r.UsuarioCreacion);

            modelBuilder.Entity<Reservacion>()
                .Ignore(r => r.UsuarioModificacion);

            modelBuilder.Entity<Usuario>()
                .Ignore(u => u.ReservacionesCreacion);

            modelBuilder.Entity<Usuario>()
                .Ignore(u => u.ReservacionesModificacion);

            modelBuilder.Entity<Usuario>()
                .Ignore(u => u.Profesor);
        }
    }
}
