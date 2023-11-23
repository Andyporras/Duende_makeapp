using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace duendeMakeApp.Models;

public partial class DuendeappContext : DbContext
{
    public DuendeappContext()
    {
    }

    public DuendeappContext(DbContextOptions<DuendeappContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Carrito> Carritos { get; set; }

    public virtual DbSet<Catalogo> Catalogos { get; set; }

    public virtual DbSet<Categoria> Categoria { get; set; }

    public virtual DbSet<Direccion> Direccions { get; set; }

    public virtual DbSet<Envio> Envios { get; set; }

    public virtual DbSet<EstadoEnvio> EstadoEnvios { get; set; }

    public virtual DbSet<Imagen> Imagens { get; set; }

    public virtual DbSet<Maquillaje> Maquillajes { get; set; }

    public virtual DbSet<Paquete> Paquetes { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Provincia> Provincia { get; set; }

    public virtual DbSet<Subcategoria> Subcategoria { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<TipoUsuario> TipoUsuarios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }
    
    public List<IObservable> Subscribers = new List<IObservable>();
    

    public virtual DbSet<Venta> Venta { get; set; }

    public virtual DbSet<Notificacion> Notificaciones { get; set; }

    public virtual DbSet<AgendaEntry> Agenda { get; set; }

    public virtual DbSet<TipoEntradaAgenda> TipoEntradaAgenda { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        //        => optionsBuilder.UseSqlServer("server=localhost; database=DUENDEAPP; User=sa; Password=miltonials; TrustServerCertificate=False; Encrypt=False;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Carrito>(entity =>
        {
            entity.HasKey(e => e.CarritoId).HasName("pk_carrito");

            entity.ToTable("Carrito");

            entity.Property(e => e.CarritoId).HasColumnName("CarritoID");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Carritos)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("fk_usuario");
        });

        modelBuilder.Entity<Catalogo>(entity =>
        {
            entity.HasKey(e => e.CatalogoId).HasName("pk_catalogo");

            entity.ToTable("Catalogo");

            entity.Property(e => e.CatalogoId).HasColumnName("CatalogoID");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.CategoriaId).HasName("pk_categoria");

            entity.Property(e => e.CategoriaId).HasColumnName("CategoriaID");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Direccion>(entity =>
        {
            entity.HasKey(e => e.DireccionId).HasName("pk_direccion");

            entity.ToTable("Direccion");

            entity.Property(e => e.DireccionId).HasColumnName("DireccionID");
            entity.Property(e => e.Detalle)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ProvinciaId).HasColumnName("ProvinciaID");

            entity.HasOne(d => d.Provincia).WithMany(p => p.Direccions)
                .HasForeignKey(d => d.ProvinciaId)
                .HasConstraintName("fk_provincia");
        });

        modelBuilder.Entity<Envio>(entity =>
        {
            entity.HasKey(e => e.EnvioId).HasName("pk_envio");

            entity.ToTable("Envio");

            entity.Property(e => e.EnvioId).HasColumnName("EnvioID");
            entity.Property(e => e.CarritoId).HasColumnName("CarritoID");
            entity.Property(e => e.DireccionId).HasColumnName("DireccionID");
            entity.Property(e => e.EstadoId).HasColumnName("EstadoID");
            entity.Property(e => e.FechaEntrega).HasColumnType("date");
            entity.Property(e => e.FechaPedido).HasColumnType("date");

            entity.HasOne(d => d.Carrito).WithMany(p => p.Envios)
                .HasForeignKey(d => d.CarritoId)
                .HasConstraintName("fk_envio_carrito");

            entity.HasOne(d => d.Direccion).WithMany(p => p.Envios)
                .HasForeignKey(d => d.DireccionId)
                .HasConstraintName("fk_envio_direccion");

            entity.HasOne(d => d.Estado).WithMany(p => p.Envios)
                .HasForeignKey(d => d.EstadoId)
                .HasConstraintName("fk_envio_estado");
        });

        modelBuilder.Entity<EstadoEnvio>(entity =>
        {
            entity.HasKey(e => e.EstadoId).HasName("pk_estado");

            entity.ToTable("EstadoEnvio");

            entity.Property(e => e.EstadoId).HasColumnName("EstadoID");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Imagen>(entity =>
        {
            entity.HasKey(e => e.ImagenId).HasName("pk_imagen");

            entity.ToTable("Imagen");

            entity.Property(e => e.ImagenId).HasColumnName("ImagenID");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Url)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasMany(d => d.Maquillajes).WithMany(p => p.Imagens)
                .UsingEntity<Dictionary<string, object>>(
                    "ImagenesXmaquillaje",
                    r => r.HasOne<Maquillaje>().WithMany()
                        .HasForeignKey("MaquillajeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_imagenesXmaquillaje_maquillaje"),
                    l => l.HasOne<Imagen>().WithMany()
                        .HasForeignKey("ImagenId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_imagenesXmaquillaje_imagen"),
                    j =>
                    {
                        j.HasKey("ImagenId", "MaquillajeId").HasName("PK__imagenes__2B035CA8B6289A80");
                        j.ToTable("imagenesXmaquillaje");
                        j.IndexerProperty<int>("ImagenId").HasColumnName("ImagenID");
                        j.IndexerProperty<int>("MaquillajeId").HasColumnName("MaquillajeID");
                    });
        });

        modelBuilder.Entity<Maquillaje>(entity =>
        {
            entity.HasKey(e => e.MaquillajeId).HasName("pk_maquillaje");

            entity.ToTable("Maquillaje");

            entity.Property(e => e.MaquillajeId).HasColumnName("MaquillajeID");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Paquete>(entity =>
        {
            entity.HasKey(e => e.PaqueteId).HasName("pk_paquete");

            entity.ToTable("Paquete");

            entity.Property(e => e.PaqueteId).HasColumnName("PaqueteID");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");

            entity.HasMany(d => d.Carritos).WithMany(p => p.Paquetes)
                .UsingEntity<Dictionary<string, object>>(
                    "PaqueteXcarrito",
                    r => r.HasOne<Carrito>().WithMany()
                        .HasForeignKey("CarritoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_paqueteXcarrito_carrito"),
                    l => l.HasOne<Paquete>().WithMany()
                        .HasForeignKey("PaqueteId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_paqueteXcarrito_paquete"),
                    j =>
                    {
                        j.HasKey("PaqueteId", "CarritoId").HasName("PK__PaqueteX__DCE7F85245FF36CA");
                        j.ToTable("PaqueteXCarrito");
                        j.IndexerProperty<int>("PaqueteId").HasColumnName("PaqueteID");
                        j.IndexerProperty<int>("CarritoId").HasColumnName("CarritoID");
                    });

            entity.HasMany(d => d.Catalogos).WithMany(p => p.Paquetes)
                .UsingEntity<Dictionary<string, object>>(
                    "PaqueteXcatalogo",
                    r => r.HasOne<Catalogo>().WithMany()
                        .HasForeignKey("CatalogoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_paquetesXcatalogo_catalogo"),
                    l => l.HasOne<Paquete>().WithMany()
                        .HasForeignKey("PaqueteId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_paquetesXcatalogo_paquete"),
                    j =>
                    {
                        j.HasKey("PaqueteId", "CatalogoId").HasName("PK__PaqueteX__341017FBF76657B9");
                        j.ToTable("PaqueteXCatalogo");
                        j.IndexerProperty<int>("PaqueteId").HasColumnName("PaqueteID");
                        j.IndexerProperty<int>("CatalogoId").HasColumnName("CatalogoID");
                    });
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.ProductoId).HasName("pk_producto");

            entity.ToTable("Producto");

            entity.Property(e => e.ProductoId).HasColumnName("ProductoID");
            entity.Property(e => e.CategoriaId).HasColumnName("CategoriaID");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ImagenId).HasColumnName("ImagenID");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Productos)
                .HasForeignKey(d => d.CategoriaId)
                .HasConstraintName("fk_categoria");

            entity.HasOne(d => d.Imagen).WithMany(p => p.Productos)
                .HasForeignKey(d => d.ImagenId)
                .HasConstraintName("fk_imagen");

            entity.HasMany(d => d.Carritos).WithMany(p => p.Productos)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductosXcarrito",
                    r => r.HasOne<Carrito>().WithMany()
                        .HasForeignKey("CarritoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_productosXcarrito_carrito"),
                    l => l.HasOne<Producto>().WithMany()
                        .HasForeignKey("ProductoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_productosXcarrito_producto"),
                    j =>
                    {
                        j.HasKey("ProductoId", "CarritoId").HasName("PK__Producto__03487B03CC07B6B5");
                        j.ToTable("ProductosXCarrito");
                        j.IndexerProperty<int>("ProductoId").HasColumnName("ProductoID");
                        j.IndexerProperty<int>("CarritoId").HasColumnName("CarritoID");
                    });

            entity.HasMany(d => d.Catalogos).WithMany(p => p.Productos)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductosXcatalogo",
                    r => r.HasOne<Catalogo>().WithMany()
                        .HasForeignKey("CatalogoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_productosXcatalogo_catalogo"),
                    l => l.HasOne<Producto>().WithMany()
                        .HasForeignKey("ProductoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_productosXcatalogo_producto"),
                    j =>
                    {
                        j.HasKey("ProductoId", "CatalogoId").HasName("PK__Producto__EBBF94AA76F1DDE6");
                        j.ToTable("ProductosXCatalogo");
                        j.IndexerProperty<int>("ProductoId").HasColumnName("ProductoID");
                        j.IndexerProperty<int>("CatalogoId").HasColumnName("CatalogoID");
                    });

            entity.HasMany(d => d.Paquetes).WithMany(p => p.Productos)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductosXpaquete",
                    r => r.HasOne<Paquete>().WithMany()
                        .HasForeignKey("PaqueteId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_productosXpaquete_paquete"),
                    l => l.HasOne<Producto>().WithMany()
                        .HasForeignKey("ProductoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_productosXpaquete_producto"),
                    j =>
                    {
                        j.HasKey("ProductoId", "PaqueteId").HasName("PK__Producto__93895C5E60E01A87");
                        j.ToTable("ProductosXPaquete");
                        j.IndexerProperty<int>("ProductoId").HasColumnName("ProductoID");
                        j.IndexerProperty<int>("PaqueteId").HasColumnName("PaqueteID");
                    });
        });

        modelBuilder.Entity<Provincia>(entity =>
        {
            entity.HasKey(e => e.ProvinciaId).HasName("pk_provincia");

            entity.Property(e => e.ProvinciaId).HasColumnName("ProvinciaID");
            entity.Property(e => e.Nombre)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Subcategoria>(entity =>
        {
            entity.HasKey(e => e.SubcategoriaId).HasName("pk_subcategoria");

            entity.Property(e => e.SubcategoriaId).HasColumnName("SubcategoriaID");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasMany(d => d.Productos).WithMany(p => p.Subcategoria)
                .UsingEntity<Dictionary<string, object>>(
                    "SubcategoriaXproducto",
                    r => r.HasOne<Producto>().WithMany()
                        .HasForeignKey("ProductoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_subcategoriaXproducto_producto"),
                    l => l.HasOne<Subcategoria>().WithMany()
                        .HasForeignKey("SubcategoriaId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_subcategoriaXproducto_subcategoria"),
                    j =>
                    {
                        j.HasKey("SubcategoriaId", "ProductoId").HasName("PK__Subcateg__05A8B1EAAC156225");
                        j.ToTable("SubcategoriaXProducto");
                        j.IndexerProperty<int>("SubcategoriaId").HasColumnName("SubcategoriaID");
                        j.IndexerProperty<int>("ProductoId").HasColumnName("ProductoID");
                    });
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagId).HasName("pk_tag");

            entity.ToTable("Tag");

            entity.Property(e => e.TagId).HasColumnName("TagID");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasMany(d => d.Imagens).WithMany(p => p.Tags)
                .UsingEntity<Dictionary<string, object>>(
                    "TagsXimagen",
                    r => r.HasOne<Imagen>().WithMany()
                        .HasForeignKey("ImagenId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_tagsXimagen_imagen"),
                    l => l.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_tagsXimagen_tag"),
                    j =>
                    {
                        j.HasKey("TagId", "ImagenId").HasName("PK__TagsXIma__05BB28418B94863D");
                        j.ToTable("TagsXImagen");
                        j.IndexerProperty<int>("TagId").HasColumnName("TagID");
                        j.IndexerProperty<int>("ImagenId").HasColumnName("ImagenID");
                    });
        });

        modelBuilder.Entity<TipoUsuario>(entity =>
        {
            entity.HasKey(e => e.TipoUsarioId).HasName("pk_tipoUsuario");

            entity.ToTable("TipoUsuario");

            entity.Property(e => e.TipoUsarioId).HasColumnName("TipoUsarioID");
            entity.Property(e => e.Tipo)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("pk_usuario");

            entity.ToTable("Usuario");

            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Clave)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("clave");
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TipoId).HasColumnName("TipoID");
            entity.Property(e => e.Usuario1)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Usuario");

            entity.HasOne(d => d.Tipo).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.TipoId)
                .HasConstraintName("fk_tipo");
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.VentaId).HasName("pk_ventaID");

            entity.Property(e => e.VentaId).HasColumnName("VentaID");
            entity.Property(e => e.CarritoId).HasColumnName("CarritoID");
            entity.Property(e => e.ImgComprobante).HasColumnName("imgComprobante");

            entity.HasOne(d => d.Carrito).WithMany(p => p.Venta)
                .HasForeignKey(d => d.CarritoId)
                .HasConstraintName("fk_carrito");

            entity.HasOne(d => d.ImgComprobanteNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.ImgComprobante)
                .HasConstraintName("fk_comprobante");
        });

        modelBuilder.Entity<Notificacion>(entity =>
        {
            entity.HasKey(e => e.NotificacionId).HasName("pk_notificacion");

            entity.ToTable("Notificaciones");

            entity.Property(e => e.NotificacionId).HasColumnName("notificacion_id");
            entity.Property(e => e.Titulo).HasColumnName("titulo");
            entity.Property(e => e.Mensaje).HasColumnName("mensaje");
            entity.Property(e => e.Titulo).HasMaxLength(100).IsUnicode(false);
            entity.Property(e => e.Mensaje).HasMaxLength(255).IsUnicode(false);
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");
            entity.Property(e => e.FechaEnvio).HasColumnName("fecha_envio");
            entity.Property(e => e.FechaEnvio).HasColumnType("datetime");
            entity.Property(e => e.Visto).HasColumnName("visto");
            entity.Property(e => e.Visto).HasColumnType("bit");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Notificaciones)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("fk_notificacion_usuario");
        });

        // Configuración para AgendaEntry
        modelBuilder.Entity<AgendaEntry>(entity =>
        {
            entity.HasKey(e => e.AgendaID).HasName("pk_agenda");

            entity.ToTable("Agenda");

            entity.Property(e => e.AgendaID).HasColumnName("AgendaID");
            entity.Property(e => e.UsuarioID).HasColumnName("UsuarioID");
            entity.Property(e => e.Asunto).HasMaxLength(100).IsUnicode(false);
            entity.Property(e => e.FechaInicio).HasColumnType("datetime");
            entity.Property(e => e.DuracionMinutos).HasColumnName("DuracionMinutos");
            entity.Property(e => e.TipoEntrada).HasMaxLength(50).IsUnicode(false);

            // Relación con Usuario
            entity.HasOne(d => d.Usuario)
                .WithMany(p => p.Agenda)
                .HasForeignKey(d => d.UsuarioID)
                .HasConstraintName("fk_agenda_usuario");
        });

        // Configuración para TipoEntradaAgenda
        modelBuilder.Entity<TipoEntradaAgenda>(entity =>
        {
            entity.HasKey(e => e.TipoEntrada).HasName("pk_tipoEntradaAgenda");

            entity.ToTable("TipoEntradaAgenda");

            entity.Property(e => e.TipoEntrada).HasMaxLength(50).IsUnicode(false);
            entity.Property(e => e.PermiteTraslape).HasColumnName("PermiteTraslape");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
