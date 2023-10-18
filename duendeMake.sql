-- CREACIÓN DE LA BASE DE DATOS EN CASO DE NO EXISTIR
USE master
GO
IF NOT EXISTS (
    SELECT name
    FROM sys.databases
    WHERE name = N'DUENDEAPP'
)
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
    imgComprobante int,
    CarritoID INT,
    CONSTRAINT pk_ventaID PRIMARY KEY (VentaID),
    CONSTRAINT fk_carrito FOREIGN KEY (CarritoID)
        REFERENCES Carrito(CarritoID),
	CONSTRAINT fk_comprobante FOREIGN KEY (imgComprobante)
		REFERENCES Imagen(ImagenID)
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

----------------------------------------------------------------- PROCEDIMIENTOS ALMACENADOS
CREATE PROCEDURE InsertarCategoria
    @Nombre VARCHAR(100)
AS
BEGIN

    IF EXISTS (SELECT 1 FROM Categoria WHERE CategoriaID = @CategoriaID)
    BEGIN
        PRINT 'La CategoriaID no existe en la tabla Categoria. No se puede insertar el registro.';
        RETURN;
    END

    INSERT INTO Categoria (Nombre)
    VALUES (@Nombre);
    PRINT 'Registro insertado exitosamente.';
END;
GO

CREATE PROCEDURE InsertarProducto 
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(100),
    @Precio DECIMAL(10, 2),
    @Cantidad INT,
    @CategoriaID INT,
    @Estado bit
AS
BEGIN

    IF EXISTS (SELECT 1 FROM Producto WHERE ProductoID = @ProductoID)
    BEGIN
        PRINT 'El ProductoID ya existe. No se puede insertar el registro.';
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM Categoria WHERE CategoriaID = @CategoriaID)
    BEGIN
        PRINT 'La CategoriaID no existe en la tabla Categoria. No se puede insertar el registro.';
        RETURN;
    END

    INSERT INTO Producto (Nombre, Descripcion, Precio, Cantidad, CategoriaID, Estado)
    VALUES (@Nombre, @Descripcion, @Precio, @Cantidad, @CategoriaID, @Estado);

    PRINT 'Registro insertado exitosamente.';
END;
GO

CREATE PROCEDURE InsertarImagen
    @Nombre VARCHAR(30),
    @Descripcion VARCHAR(100),
    @Url VARCHAR(100)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Imagen WHERE ImagenID = @ImagenID)
    BEGIN
        PRINT 'El ImagenID ya existe. No se puede insertar el registro.';
        RETURN;
    END

    INSERT INTO Imagen (Nombre, Descripcion, Url)
    VALUES (@Nombre, @Descripcion, @Url);

    PRINT 'Registro insertado exitosamente.';
END;
GO

CREATE PROCEDURE InsertarTag 
    @Nombre VARCHAR(20)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Tag WHERE TagID = @TagID)
    BEGIN
        PRINT 'El TagID ya existe. No se puede insertar el registro.';
        RETURN;
    END

    INSERT INTO Tag (Nombre)
    VALUES (@Nombre);

    PRINT 'Registro insertado exitosamente.';
END;
GO

CREATE PROCEDURE InsertarSubcategoria
    @Nombre VARCHAR(50)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Subcategoria WHERE SubcategoriaID = @SubcategoriaID)
    BEGIN
        PRINT 'El SubcategoriaID ya existe. No se puede insertar el registro.';
        RETURN;
    END

    INSERT INTO Subcategoria (Nombre)
    VALUES (@Nombre);

    PRINT 'Registro insertado exitosamente.';
END;
GO

CREATE PROCEDURE insertarPaquete
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(100),
    @Precio DECIMAL(10, 2),
    @CantidadDisponible INT,
    @Estado bit
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Paquete WHERE PaqueteID = @PaqueteID)
    BEGIN
        PRINT 'El PaqueteID ya existe. No se puede insertar el registro.';
        RETURN;
    END

    INSERT INTO Paquete (Nombre, Descripcion, Precio, CantidadDisponible, Estado)
    VALUES (@Nombre, @Descripcion, @Precio, @CantidadDisponible, @Estado);

    PRINT 'Registro insertado exitosamente.';
END;
GO

CREATE PROCEDURE InsertarCatalogo
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(100),
    @Estado bit
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Catalogo WHERE CatalogoID = @CatalogoID)
    BEGIN
        PRINT 'El CatalogoID ya existe. No se puede insertar el registro.';
        RETURN;
    END

    INSERT INTO Catalogo (Nombre, Descripcion, Estado)
    VALUES (@Nombre, @Descripcion, @Estado);

    PRINT 'Registro insertado exitosamente.';
END;
GO

CREATE PROCEDURE InsertarDireccion
    @ProvinciaiD VARCHAR(20),
    @Detalle VARCHAR(20)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Direccion WHERE DireccionID = @DireccionID)
    BEGIN
        PRINT 'El DireccionID ya existe. No se puede insertar el registro.';
        RETURN;
    END
    INSERT INTO Direccion (ProvinciaiD, Detalle)
    VALUES (@ProvinciaiD, @Detalle);

    PRINT 'Registro insertado exitosamente.';
END;
GO

CREATE PROCEDURE InsertarEstado
    @Estado VARCHAR(20)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Estado WHERE EstadoID = @EstadoID)
    BEGIN
        PRINT 'El EstadoID ya existe. No se puede insertar el registro.';
        RETURN;
    END

    INSERT INTO EstadoEnvio(Estado)
    VALUES (@Estado);

    PRINT 'Registro insertado exitosamente.';
END;


GO
CREATE PROCEDURE InsertarEnvio
    @FechaPedido date,
    @FechaEntrega date,
    @EstadoID INT,
    @CarritoID INT,
    @DireccionID INT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Envio WHERE EnvioID = @EnvioID)
    BEGIN
        PRINT 'El EnvioID ya existe. No se puede insertar el registro.';
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM Estado WHERE EstadoID = @EstadoID)
    BEGIN
        PRINT 'El EstadoID no existe en la tabla Estado. No se puede insertar el registro.';
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM Carrito WHERE CarritoID = @CarritoID)
    BEGIN
        PRINT 'El CarritoID no existe en la tabla Carrito. No se puede insertar el registro.';
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM Direccion WHERE DireccionID = @DireccionID)
    BEGIN
        PRINT 'El DireccionID no existe en la tabla Direccion. No se puede insertar el registro.';
        RETURN;
    END

    INSERT INTO Envio (FechaPedido, FechaEntrega, EstadoID, CarritoID, DireccionID)
    VALUES (@FechaPedido, @FechaEntrega, @EstadoID, @CarritoID, @DireccionID);

    PRINT 'Registro insertado exitosamente.';
END;


GO
CREATE PROCEDURE agregarCarrito
    @UsuarioID INT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Carrito WHERE CarritoID = @CarritoID)
    BEGIN
        PRINT 'El CarritoID ya existe. No se puede insertar el registro.';
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM Usuario WHERE UsuarioID = @UsuarioID)
    BEGIN
        PRINT 'El UsuarioID no existe en la tabla Usuario. No se puede insertar el registro.';
        RETURN;
    END

    INSERT INTO Carrito (UsuarioID)
    VALUES (@UsuarioID);

    PRINT 'Registro insertado exitosamente.';
END;



GO
CREATE PROCEDURE InsertarProductosXCatalogo
    @ProductoID INT,
    @CatalogoID INT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Producto WHERE ProductoID = @ProductoID)
    BEGIN
        PRINT 'El ProductoID no existe en la tabla Producto. No se puede insertar el registro.';
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM Catalogo WHERE CatalogoID = @CatalogoID)
    BEGIN
        PRINT 'El CatalogoID no existe en la tabla Catalogo. No se puede insertar el registro.';
        RETURN;
    END

    INSERT INTO ProductosXCatalogo (ProductoID, CatalogoID)
    VALUES (@ProductoID, @CatalogoID);

    PRINT 'Registro insertado exitosamente.';
END;

GO
CREATE PROCEDURE InsertarProductosXPaquete
    @ProductoID INT,
    @PaqueteID INT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Producto WHERE ProductoID = @ProductoID)
    BEGIN
        PRINT 'El ProductoID no existe en la tabla Producto. No se puede insertar el registro.';
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM Paquete WHERE PaqueteID = @PaqueteID)
    BEGIN
        PRINT 'El PaqueteID no existe en la tabla Paquete. No se puede insertar el registro.';
        RETURN;
    END

    INSERT INTO ProductosXPaquete (ProductoID, PaqueteID)
    VALUES (@ProductoID, @PaqueteID);

    PRINT 'Registro insertado exitosamente.';
END;


GO
CREATE PROCEDURE InsertarPaqueteXCatalogo
    @PaqueteID INT,
    @CatalogoID INT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Paquete WHERE PaqueteID = @PaqueteID)
    BEGIN
        PRINT 'El PaqueteID no existe en la tabla Paquete. No se puede insertar el registro.';
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM Catalogo WHERE CatalogoID = @CatalogoID)
    BEGIN
        PRINT 'El CatalogoID no existe en la tabla Catalogo. No se puede insertar el registro.';
        RETURN;
    END

    INSERT INTO PaqueteXCatalogo (PaqueteID, CatalogoID)
    VALUES (@PaqueteID, @CatalogoID);

    PRINT 'Registro insertado exitosamente.';
END;


GO
CREATE PROCEDURE InsertarPaqueteXCarrito
    @PaqueteID INT,
    @CarritoID INT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Paquete WHERE PaqueteID = @PaqueteID)
    BEGIN
        PRINT 'El PaqueteID no existe en la tabla Paquete. No se puede insertar el registro.';
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM Carrito WHERE CarritoID = @CarritoID)
    BEGIN
        PRINT 'El CarritoID no existe en la tabla Carrito. No se puede insertar el registro.';
        RETURN;
    END

    INSERT INTO PaqueteXCarrito (PaqueteID, CarritoID)
    VALUES (@PaqueteID, @CarritoID);

    PRINT 'Registro insertado exitosamente.';
END;


GO
CREATE PROCEDURE InsertarSubcategoriaXProducto
    @SubcategoriaID INT,
    @ProductoID INT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Subcategoria WHERE SubcategoriaID = @SubcategoriaID)
    BEGIN
        PRINT 'El Sub|ID no existe en la tabla Subcategoria. No se puede insertar el registro.';
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM Producto WHERE ProductoID = @ProductoID)
    BEGIN
        PRINT 'El ProductoID no existe en la tabla Producto. No se puede insertar el registro.';
        RETURN;
    END

    INSERT INTO SubcategoriaXProducto (SubcategoriaID, ProductoID)
    VALUES (@SubcategoriaID, @ProductoID);

    PRINT 'Registro insertado exitosamente.';
END;


GO
CREATE PROCEDURE InsertarProductosXCarrito
    @ProductoID INT,
    @CarritoID INT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Producto WHERE ProductoID = @ProductoID)
    BEGIN
        PRINT 'El ProductoID no existe en la tabla Producto. No se puede insertar el registro.';
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM Carrito WHERE CarritoID = @CarritoID)
    BEGIN
        PRINT 'El CarritoID no existe en la tabla Carrito. No se puede insertar el registro.';
        RETURN;
    END

    INSERT INTO ProductosXCarrito (ProductoID, CarritoID)
    VALUES (@ProductoID, @CarritoID);

    PRINT 'Registro insertado exitosamente.';
END;




GO
CREATE PROCEDURE InsertarMaquillaje
    @Nombre VARCHAR(30),
    @Descripcion VARCHAR(100),
    @Estado bit
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Maquillaje WHERE MaquillajeID = @MaquillajeID)
    BEGIN
        PRINT 'El MaquillajeID ya existe. No se puede insertar el registro.';
        RETURN;
    END

    INSERT INTO Maquillaje (Nombre, Descripcion, Estado)
    VALUES (@Nombre, @Descripcion, @Estado);

    PRINT 'Registro insertado exitosamente.';
END;


create procedure registrarUsuario
@cedula varchar(9),
@nombre varchar (20),
@apellido1 varchar (50),
@correo varchar (50),
@usuario varchar (20),
@clave varchar (500),
@TipoUsarioID int,
@patron varchar(100)
as
IF EXISTS (select cedula from usuario where cedula = @cedula)
    BEGIN
        return -1;
    END
ELSE
begin
BEGIN TRY
    insert into usuario(cedula, nombre, apellido1, correo, usuario, clave, TipoUsarioID)
    values(@cedula, @nombre, @apellido1, @correo, @usuario, ENCRYPTBYPASSPHRASE (@patron, @clave), @TipoUsarioID)
    RETURN 0
END TRY
BEGIN CATCH
    RETURN -2
END CATCH
end


go
create procedure validarUsuario
@usuario varchar(20),
@clave varchar(30),
@patron varchar(20)
as
BEGIN TRY
	select * from Usuario where nombre = @usuario and CONVERT(varchar(30), DECRYPTBYPASSPHRASE(@patron, clave))= @clave
	return 0
END TRY
BEGIN CATCH
	return -1
END CATCH;


CREATE TRIGGER Before_Delete_Producto
BEFORE DELETE ON Producto
FOR EACH ROW
BEGIN
    -- Cancelar la eliminación
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'No se puede eliminar, solo se puede cambiar el estado.';

    -- Actualizar el estado a 0 en lugar de eliminar
    UPDATE Producto
    SET Estado = 0
    WHERE ProductoID = OLD.ProductoID;
END;

CREATE TRIGGER Before_Delete_Paquete
BEFORE DELETE ON Paquete
FOR EACH ROW
BEGIN
    -- Cancelar la eliminación
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'No se puede eliminar, solo se puede cambiar el estado.';

    -- Actualizar el estado a 0 en lugar de eliminar
    UPDATE Paquete
    SET Estado = 0
    WHERE PaqueteID = OLD.PaqueteID;
END;


CREATE TRIGGER Before_Delete_Catalogo
BEFORE DELETE ON Catalogo
FOR EACH ROW
BEGIN
    -- Cancelar la eliminación
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'No se puede eliminar, solo se puede cambiar el estado.';

    -- Actualizar el estado a 0 en lugar de eliminar
    UPDATE Catalogo
    SET Estado = 0
    WHERE CatalogoID = OLD.CatalogoID;
END;

CREATE TRIGGER Before_Delete_Maquillaje
BEFORE DELETE ON Maquillaje
FOR EACH ROW
BEGIN
    -- Cancelar la eliminación
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'No se puede eliminar, solo se puede cambiar el estado.';

    -- Actualizar el estado a 0 en lugar de eliminar
    UPDATE Maquillaje
    SET Estado = 0
    WHERE MaquillajeID = OLD.MaquillajeID;
END;


CREATE PROCEDURE EliminarProducto
    @ProductoID INT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Producto WHERE ProductoID = @ProductoID)
    BEGIN
        PRINT 'El ProductoID no existe en la tabla Producto. No se puede eliminar el registro.';
        RETURN;
    END

    DELETE FROM Producto
    WHERE ProductoID = @ProductoID;

    PRINT 'Registro eliminado exitosamente.';
END;


CREATE PROCEDURE EliminarPaquete
    @PaqueteID INT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Paquete WHERE PaqueteID = @PaqueteID)
    BEGIN
        PRINT 'El PaqueteID no existe en la tabla Paquete. No se puede eliminar el registro.';
        RETURN;
    END

    DELETE FROM Paquete
    WHERE PaqueteID = @PaqueteID;

    PRINT 'Registro eliminado exitosamente.';
END;

CREATE PROCEDURE EliminarCatalogo
    @CatalogoID INT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Catalogo WHERE CatalogoID = @CatalogoID)
    BEGIN
        PRINT 'El CatalogoID no existe en la tabla Catalogo. No se puede eliminar el registro.';
        RETURN;
    END

    DELETE FROM Catalogo
    WHERE CatalogoID = @CatalogoID;

    PRINT 'Registro eliminado exitosamente.';
END;

CREATE PROCEDURE EliminarMaquillaje
    @MaquillajeID INT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Maquillaje WHERE MaquillajeID = @MaquillajeID)
    BEGIN
        PRINT 'El MaquillajeID no existe en la tabla Maquillaje. No se puede eliminar el registro.';
        RETURN;
    END

    DELETE FROM Maquillaje
    WHERE MaquillajeID = @MaquillajeID;

    PRINT 'Registro eliminado exitosamente.';
END;


