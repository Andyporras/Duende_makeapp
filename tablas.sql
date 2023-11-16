-- CREACIÓN DE LA BASE DE DATOS EN CASO DE NO EXISTIR
USE master
GO
IF NOT EXISTS (
    SELECT name
    FROM sys.databases
    WHERE name = N'DUENDEAPP'
)


CREATE DATABASE DUENDEAPP
GO
USE DUENDEAPP
GO

/*
DROP TABLE Imagen;
DROP TABLE Maquillaje;
DROP TABLE Tag;
DROP TABLE Categoria;
DROP TABLE Provincia;
DROP TABLE EstadoEnvio;
DROP TABLE Catalogo;
DROP TABLE Paquete;
DROP TABLE TipoUsuario;
DROP TABLE Subcategoria;
DROP TABLE Usuario;
DROP TABLE Carrito;
DROP TABLE Producto;
DROP TABLE Venta;
DROP TABLE imagenesXmaquillaje;
DROP TABLE TagsXImagen;
DROP TABLE Direccion;
DROP TABLE Envio;
DROP TABLE SubcategoriaXProducto;
DROP TABLE ProductosXCarrito;
DROP TABLE ProductosXCatalogo;
DROP TABLE ProductosXPaquete;
DROP TABLE PaqueteXCatalogo;
DROP TABLE PaqueteXCarrito;
*/
-- CREACIÓN DE TABLAS SI NO EXISTEN
 


 IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Imagen' and xtype='U')
CREATE TABLE Imagen (
    ImagenID INT IDENTITY(1,1),
    Nombre VARCHAR(50),
    Descripcion VARCHAR(100),
    Url VARCHAR(100),
    CONSTRAINT pk_imagen PRIMARY KEY (ImagenID)
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Maquillaje' AND xtype='U')
CREATE TABLE Maquillaje(
    MaquillajeID INT IDENTITY(1,1),
    Nombre VARCHAR(30),
    Descripcion VARCHAR(100),
    Estado bit,
    CONSTRAINT pk_maquillaje PRIMARY KEY (MaquillajeID)
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Tag' AND xtype='U')
CREATE TABLE Tag (
    TagID INT IDENTITY(1,1),
    Nombre VARCHAR(20),

    CONSTRAINT pk_tag PRIMARY KEY (TagID)
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Categoria' AND xtype='U')
CREATE TABLE Categoria (
    CategoriaID INT IDENTITY(1,1),
    Nombre VARCHAR(100),
    CONSTRAINT pk_categoria PRIMARY KEY (CategoriaID)
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Provincia' AND xtype='U')
CREATE TABLE Provincia (
    ProvinciaID INT IDENTITY(1,1),
    Nombre VARCHAR(15),
    CONSTRAINT pk_provincia PRIMARY KEY (ProvinciaID)
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='EstadoEnvio' AND xtype='U')
CREATE TABLE EstadoEnvio(
    EstadoID INT IDENTITY(1,1),
    Estado VARCHAR(20),
    CONSTRAINT pk_estado PRIMARY KEY (EstadoID)
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Catalogo' AND xtype='U')
CREATE TABLE Catalogo (
    CatalogoID INT IDENTITY(1,1),
    Nombre VARCHAR(30),
    Descripcion VARCHAR(100),
    Estado bit,

    CONSTRAINT pk_catalogo PRIMARY KEY (CatalogoID)
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Paquete' AND xtype='U')
CREATE TABLE Paquete (
    PaqueteID INT IDENTITY(1,1),
    Nombre VARCHAR(100),
    Descripcion VARCHAR(100),
    Precio DECIMAL(10, 2),
    CantidadDisponible INT, 
    Estado bit,

    CONSTRAINT pk_paquete PRIMARY KEY (PaqueteID)
);
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='TipoUsuario' AND xtype='U')
CREATE TABLE TipoUsuario (
    TipoUsarioID INT IDENTITY(1,1),
    Tipo VARCHAR(100),
    CONSTRAINT pk_tipoUsuario PRIMARY KEY (TipoUsarioID)
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Subcategoria' AND xtype='U')
CREATE TABLE Subcategoria(
    SubcategoriaID INT IDENTITY(1,1),
    Nombre VARCHAR(50),
    CONSTRAINT pk_subcategoria PRIMARY KEY (SubcategoriaID)
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Usuario' AND xtype='U')
CREATE TABLE Usuario (
    UsuarioID INT IDENTITY(1,1),
    Nombre VARCHAR(20),
    Apellido VARCHAR(50),
    Correo VARCHAR(50),
    Usuario VARCHAR(20),
    clave VARCHAR(500),
    TipoID INT
    CONSTRAINT pk_usuario PRIMARY key (UsuarioID),
    CONSTRAINT fk_tipo FOREIGN KEY (TipoID)
        REFERENCES TipoUsuario(TipoUsarioID)
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Carrito' AND xtype='U')
CREATE TABLE Carrito (
    CarritoID INT IDENTITY(1,1),
    UsuarioID INT,
    CONSTRAINT pk_carrito PRIMARY KEY (CarritoID),
    CONSTRAINT fk_usuario FOREIGN KEY (UsuarioID)
        REFERENCES Usuario(UsuarioID)
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Producto' AND xtype='U')
CREATE TABLE Producto (
    ProductoID INT IDENTITY(1,1),
    Nombre VARCHAR(30),
    Descripcion VARCHAR(200),
    Precio DECIMAL(10, 2),
    Cantidad INT,
	CategoriaID INT,
    Estado bit,
    ImagenID INT,
    CONSTRAINT pk_producto PRIMARY KEY (ProductoID),
	CONSTRAINT fk_categoria FOREIGN KEY (CategoriaID)
        REFERENCES Categoria(CategoriaID),
	CONSTRAINT fk_imagen FOREIGN KEY (ImagenID)
		REFERENCES Imagen(ImagenID)
);



IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Venta' AND xtype='U')
CREATE TABLE Venta (
    VentaID INT IDENTITY(1,1),
    monto decimal(10,2),
	imgComprobante int,
    CarritoID INT,
	codPostal int,
	fechaEntrega date,
	fechaPedido date,
	direccion varchar(500),
	estado int,
	ProvinciaID int,
    CONSTRAINT pk_ventaID PRIMARY KEY (VentaID),
    CONSTRAINT fk_carrito FOREIGN KEY (CarritoID)
        REFERENCES Carrito(CarritoID),
	CONSTRAINT fk_comprobante FOREIGN KEY (imgComprobante)
		REFERENCES Imagen(ImagenID),
	CONSTRAINT fk_provinciaVenta FOREIGN KEY (ProvinciaID)
        REFERENCES Provincia(ProvinciaID)
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='imagenesXmaquillaje' AND xtype='U')
CREATE TABLE imagenesXmaquillaje (
    ImagenID INT,
    MaquillajeID INT,
    PRIMARY KEY (ImagenID, MaquillajeID),
    CONSTRAINT fk_imagenesXmaquillaje_imagen FOREIGN KEY (ImagenID) REFERENCES Imagen(ImagenID),
    CONSTRAINT fk_imagenesXmaquillaje_maquillaje FOREIGN KEY (MaquillajeID) REFERENCES Maquillaje(MaquillajeID)
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='TagsXImagen' AND xtype='U')
CREATE TABLE TagsXImagen (
    TagID INT,
    ImagenID INT,
    PRIMARY KEY (TagID, ImagenID),
    CONSTRAINT fk_tagsXimagen_tag FOREIGN KEY (TagID) REFERENCES Tag(TagID),
    CONSTRAINT fk_tagsXimagen_imagen FOREIGN KEY (ImagenID) REFERENCES Imagen(ImagenID)
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Direccion' AND xtype='U')
CREATE TABLE Direccion (
    DireccionID INT IDENTITY(1,1),
    CodigoPostal INT,
    Detalle VARCHAR(100),
    ProvinciaID INT,
    CONSTRAINT pk_direccion PRIMARY KEY (DireccionID),
    CONSTRAINT fk_provincia FOREIGN KEY (ProvinciaID)
        REFERENCES Provincia(ProvinciaID)
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Envio' AND xtype='U')
CREATE TABLE Envio (
    EnvioID INT IDENTITY(1,1),
    FechaPedido date,
    FechaEntrega date,
    EstadoID INT,
    CarritoID INT,
    DireccionID INT,
    CONSTRAINT pk_envio PRIMARY KEY (EnvioID),
    CONSTRAINT fk_envio_estado FOREIGN KEY (EstadoID) REFERENCES EstadoEnvio(EstadoID),
    CONSTRAINT fk_envio_carrito FOREIGN KEY (CarritoID) REFERENCES Carrito(CarritoID),
    CONSTRAINT fk_envio_direccion FOREIGN KEY (DireccionID) REFERENCES Direccion(DireccionID)
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='SubcategoriaXProducto' AND xtype='U')
CREATE TABLE SubcategoriaXProducto (
    SubcategoriaID INT,
    ProductoID INT,
    PRIMARY KEY (SubcategoriaID, ProductoID),
    CONSTRAINT fk_subcategoriaXproducto_subcategoria FOREIGN KEY (SubcategoriaID) REFERENCES Subcategoria(SubcategoriaID),
    CONSTRAINT fk_subcategoriaXproducto_producto FOREIGN KEY (ProductoID) REFERENCES Producto(ProductoID)
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ProductosXCarrito' AND xtype='U')
CREATE TABLE ProductosXCarrito (
    ProductoID INT,
    CarritoID INT,
    PRIMARY KEY (ProductoID, CarritoID),
    CONSTRAINT fk_productosXcarrito_producto FOREIGN KEY (ProductoID) REFERENCES Producto(ProductoID),
    CONSTRAINT fk_productosXcarrito_carrito FOREIGN KEY (CarritoID) REFERENCES Carrito(CarritoID)
);    
ALTER TABLE ProductosXCarrito
ADD Cantidad int;


IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ProductosXCatalogo' AND xtype='U')
CREATE TABLE ProductosXCatalogo (
    ProductoID INT,
    CatalogoID INT,
    PRIMARY KEY (ProductoID, CatalogoID),
    CONSTRAINT fk_productosXcatalogo_producto FOREIGN KEY (ProductoID) REFERENCES Producto(ProductoID),
    CONSTRAINT fk_productosXcatalogo_catalogo FOREIGN KEY (CatalogoID) REFERENCES Catalogo(CatalogoID)
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ProductosXPaquete' AND xtype='U')
CREATE TABLE ProductosXPaquete (
    ProductoID INT,
    PaqueteID INT,
    PRIMARY KEY (ProductoID, PaqueteID),
    CONSTRAINT fk_productosXpaquete_producto FOREIGN KEY (ProductoID) REFERENCES Producto(ProductoID),
    CONSTRAINT fk_productosXpaquete_paquete FOREIGN KEY (PaqueteID) REFERENCES Paquete(PaqueteID)
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PaqueteXCatalogo' AND xtype='U')
CREATE TABLE PaqueteXCatalogo (
    PaqueteID INT,
    CatalogoID INT,
    PRIMARY KEY (PaqueteID, CatalogoID),
    CONSTRAINT fk_paquetesXcatalogo_paquete FOREIGN KEY (PaqueteID) REFERENCES Paquete(PaqueteID),
    CONSTRAINT fk_paquetesXcatalogo_catalogo FOREIGN KEY (CatalogoID) REFERENCES Catalogo(CatalogoID)
);    


IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PaqueteXCarrito' AND xtype='U')
CREATE TABLE PaqueteXCarrito (
    PaqueteID INT,
    CarritoID INT,
    PRIMARY KEY (PaqueteID, CarritoID),
    CONSTRAINT fk_paqueteXcarrito_paquete FOREIGN KEY (PaqueteID) REFERENCES Paquete(PaqueteID),
    CONSTRAINT fk_paqueteXcarrito_carrito FOREIGN KEY (CarritoID) REFERENCES Carrito(CarritoID)
);

insert into TipoUsuario values ('admin'), ('cliente');
INSERT into Usuario values ('admin', 'admin', 'admin@correo.com', 'adminPro', '12345', 1);

-- modificacion de la tabla carrito
alter table carrito add estado bit

select * from Venta