using DemoBlazorMovil.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CineApi.Data
{
    public partial class CineDBContext : DbContext
    {
        public CineDBContext()
        {
        }

        public CineDBContext(DbContextOptions<CineDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Articulo> Articulos { get; set; }

        public virtual DbSet<DetalleVenta> DetallesVenta { get; set; }

        public virtual DbSet<Movie> Movies { get; set; }

        public virtual DbSet<Rol> Roles { get; set; }

        public virtual DbSet<Sala> Salas { get; set; }

        public virtual DbSet<Showtime> Showtimes { get; set; }

        public virtual DbSet<Ticket> Tickets { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Venta> Ventas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Server=db28192.public.databaseasp.net; Database=db28192; User Id=db28192; Password=5p!DbP6%X2+o; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Articulo>(entity =>
            {
                entity.ToTable("Articulo");

                entity.Property(e => e.Categoria).HasMaxLength(50);
                entity.Property(e => e.Nombre).HasMaxLength(50);
                entity.Property(e => e.Precio).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<DetalleVenta>(entity =>
            {
                entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Articulo).WithMany(p => p.DetallesVenta)
                    .HasForeignKey(d => d.ArticuloId)
                    .HasConstraintName("FK_DetalleVenta_Articulo");

                entity.HasOne(d => d.Ticket).WithMany(p => p.DetallesVenta)
                    .HasForeignKey(d => d.TicketId)
                    .HasConstraintName("FK_DetalleVenta_Ticket");

                entity.HasOne(d => d.Venta).WithMany(p => p.DetallesVenta)
                    .HasForeignKey(d => d.VentaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DetalleVenta_Venta");
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.Property(e => e.Genre).HasMaxLength(50);
                entity.Property(e => e.Title).HasMaxLength(50);
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.ToTable("Rol");

                entity.Property(e => e.Nombre).HasMaxLength(50);
            });

            modelBuilder.Entity<Sala>(entity =>
            {
                entity.ToTable("Sala");

                entity.Property(e => e.Nombre).HasMaxLength(10);
            });

            modelBuilder.Entity<Showtime>(entity =>
            {
                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Movie).WithMany(p => p.Showtimes)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Showtimes_Movies");

                entity.HasOne(d => d.Sala).WithMany(p => p.Showtimes)
                    .HasForeignKey(d => d.SalaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Showtimes_Sala");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("Ticket");

                entity.HasOne(d => d.Showtime).WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.ShowtimeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ticket_Showtimes");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email).HasMaxLength(50);
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.Password).HasMaxLength(50);

                entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdRol)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Rol");
            });

            modelBuilder.Entity<Venta>(entity =>
            {
                entity.Property(e => e.Codigo).HasMaxLength(50);
                entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.User).WithMany(p => p.Ventas)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Venta_Users");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}