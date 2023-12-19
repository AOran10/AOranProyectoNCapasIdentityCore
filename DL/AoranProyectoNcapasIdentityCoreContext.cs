using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DL;

public partial class AoranProyectoNcapasIdentityCoreContext : DbContext
{
    public AoranProyectoNcapasIdentityCoreContext()
    {
    }

    public AoranProyectoNcapasIdentityCoreContext(DbContextOptions<AoranProyectoNcapasIdentityCoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Carrito> Carritos { get; set; }

    public virtual DbSet<Departamento> Departamentos { get; set; }

    public virtual DbSet<DetalleCarrito> DetalleCarritos { get; set; }

    public virtual DbSet<DetallePedido> DetallePedidos { get; set; }

    public virtual DbSet<Estatus> Estatuses { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<ProductoView> ProductoViews { get; set; }

    public virtual DbSet<RolesAsignado> RolesAsignados { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.; Database= AOranProyectoNCapasIdentityCore; TrustServerCertificate=True; User ID=sa; Password=pass@word1;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.IdArea).HasName("PK__Area__2FC141AA23FF00F1");

            entity.ToTable("Area");

            entity.HasIndex(e => e.Nombre, "UQ__Area__75E3EFCF2C4B50C7").IsUnique();

            entity.Property(e => e.Descripcion).HasMaxLength(250);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Carrito>(entity =>
        {
            entity.HasKey(e => e.IdCarrito).HasName("PK__Carrito__8B4A618C20F7472D");

            entity.ToTable("Carrito");

            entity.Property(e => e.Fecha).HasColumnType("date");
            entity.Property(e => e.IdUsuario).HasMaxLength(450);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Carritos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Carrito__IdUsuar__4E88ABD4");
        });

        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.HasKey(e => e.IdDepartamento).HasName("PK__Departam__787A433D63DB9B09");

            entity.ToTable("Departamento");

            entity.Property(e => e.Descripcion).HasMaxLength(250);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdAreaNavigation).WithMany(p => p.Departamentos)
                .HasForeignKey(d => d.IdArea)
                .HasConstraintName("FK__Departame__IdAre__37A5467C");
        });

        modelBuilder.Entity<DetalleCarrito>(entity =>
        {
            entity.HasKey(e => e.IdDetalleCarrito).HasName("PK__DetalleC__27A5F83B0A4CD609");

            entity.ToTable("DetalleCarrito");

            entity.Property(e => e.SubTotal).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.IdCarritoNavigation).WithMany(p => p.DetalleCarritos)
                .HasForeignKey(d => d.IdCarrito)
                .HasConstraintName("FK__DetalleCa__IdCar__5535A963");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetalleCarritos)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK__DetalleCa__IdPro__5629CD9C");
        });

        modelBuilder.Entity<DetallePedido>(entity =>
        {
            entity.HasKey(e => e.IdDetallePedido).HasName("PK__DetalleP__48AFFD95246EE84E");

            entity.ToTable("DetallePedido");

            entity.Property(e => e.SubTotal).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.DetallePedidos)
                .HasForeignKey(d => d.IdPedido)
                .HasConstraintName("FK__DetallePe__IdPed__4316F928");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetallePedidos)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK__DetallePe__IdPro__440B1D61");
        });

        modelBuilder.Entity<Estatus>(entity =>
        {
            entity.HasKey(e => e.IdEstatus).HasName("PK__Estatus__B32BA1C7243C1B10");

            entity.ToTable("Estatus");

            entity.HasIndex(e => e.Estatus1, "UQ__Estatus__D692F36E549456A3").IsUnique();

            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Estatus1)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Estatus");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.IdPedido).HasName("PK__Pedido__9D335DC3150E9CF3");

            entity.ToTable("Pedido");

            entity.Property(e => e.Fecha).HasColumnType("date");
            entity.Property(e => e.IdUsuario).HasMaxLength(450);
            entity.Property(e => e.Total).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdEstatus)
                .HasConstraintName("FK__Pedido__IdEstatu__403A8C7D");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Pedido__IdUsuari__3F466844");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__Producto__098892100FBCE058");

            entity.ToTable("Producto");

            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<ProductoView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ProductoView");

            entity.Property(e => e.NombreArea)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreDepartamento)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreProducto)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<RolesAsignado>(entity =>
        {
            entity.HasKey(e => e.IdRolAsignado).HasName("PK__RolesAsi__29855E40EE3B0B91");

            entity.Property(e => e.IdRol).HasMaxLength(450);
            entity.Property(e => e.IdUsuario).HasMaxLength(450);

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.RolesAsignados)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__RolesAsig__IdRol__29572725");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.RolesAsignados)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__RolesAsig__IdUsu__2A4B4B5E");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
