--create database DUENDEAPP
--go


--use DUENDEAPP
--go



CREATE TABLE Imagen (
    ImagenID INT PRIMARY KEY,
    Nombre VARCHAR(30),
    Descripcion VARCHAR(100),
    Url VARCHAR(100)
);

CREATE TABLE TipoUsuario (
    TipoUsarioID INT PRIMARY KEY,
    Tipo VARCHAR(100).
);

CREATE TABLE Categoria (
    CategoriaID INT PRIMARY KEY,
    Nombre VARCHAR(100)

);

CREATE TABLE Subcategoria(
    SubcategoriaID INT PRIMARY KEY,
    Nombre VARCHAR(50)
);


CREATE TABLE Maquillaje(
    MaquillajeID INT PRIMARY KEY,
    Nombre VARCHAR(30),
    Descripcion VARCHAR(100),
    Estado bit
);


CREATE TABLE Producto (
    ProductoID INT PRIMARY KEY,
    Nombre VARCHAR(30),
    Descripcion VARCHAR(200),
    Precio DECIMAL(10, 2),
    Cantidad INT,
	CategoriaID INT,
    Estado bit,
    Imagen VARCHAR(100),
	CONSTRAINT FK_CategoriaID FOREIGN KEY (CategoriaID)
    REFERENCES Categoria(CategoriaID)
);


CREATE TABLE Catalogo (
    CatalogoID INT PRIMARY KEY,
    Nombre VARCHAR(30),
    Descripcion VARCHAR(100),
    Estado bit
);

CREATE TABLE Paquete (
    PaqueteID INT PRIMARY KEY,
    Nombre VARCHAR(100),
    Descripcion VARCHAR(100),
    Precio DECIMAL(10, 2),
    CantidadDisponible INT, 
    Estado bit
);

CREATE TABLE Tag (
    TagID INT PRIMARY KEY,
    Nombre VARCHAR(20)       
);



CREATE TABLE Estado(
    EstadoID INT PRIMARY KEY,
    Estado VARCHAR(20)
);

CREATE TABLE Provincia (
    ProvinciaID INT PRIMARY KEY,
    Nombre VARCHAR(15)
);


CREATE TABLE Direccion (
    DireccionID INT PRIMARY KEY,
    Provincia VARCHAR(20),
    CodigoPostal INT,
    Detalle VARCHAR(100),
    ProvinciaID INT,
    CONSTRAINT FK_ProvinciaID FOREIGN KEY (ProvinciaID)
        REFERENCES Provincia(ProvinciaID)
);

CREATE TABLE Usuario (
    UsuarioID INT PRIMARY KEY,
    Nombre VARCHAR(20),
    Apellido VARCHAR(50),
    Correo VARCHAR(50),
    Usuario VARCHAR(20),
    clave VARCHAR(500),
    TipoID INT,
    CONSTRAINT FK_TipoID FOREIGN KEY (TipoID)
        REFERENCES TipoUsuario(TipoUsarioID)
);



CREATE TABLE Carrito (
    CarritoID INT PRIMARY KEY,
    UsuarioID INT,
    CONSTRAINT FK_UsuarioID FOREIGN KEY (UsuarioID)
        REFERENCES Usuario(UsuarioID)
);


CREATE TABLE Venta (
    VentaID INT PRIMARY KEY,
    imgComprobante VARCHAR(50),
    CarritoID INT
    CONSTRAINT FK_CarritoID FOREIGN KEY (CarritoID)
        REFERENCES Carrito(CarritoID)
);


CREATE TABLE ProductosXCatalogo (
    ProductoID INT,
    CatalogoID INT,
    PRIMARY KEY (ProductoID, CatalogoID),
    FOREIGN KEY (ProductoID) REFERENCES Producto(ProductoID),
    FOREIGN KEY (CatalogoID) REFERENCES Catalogo(CatalogoID)
);

CREATE TABLE ProductosXPaquete (
    ProductoID INT,
    PaqueteID INT,
    PRIMARY KEY (ProductoID, PaqueteID),
    FOREIGN KEY (ProductoID) REFERENCES Producto(ProductoID),
    FOREIGN KEY (PaqueteID) REFERENCES Paquete(PaqueteID)
);


CREATE TABLE PaqueteXCatalogo (
    PaqueteID INT,
    CatalogoID INT,
    PRIMARY KEY (PaqueteID, CatalogoID),
    FOREIGN KEY (PaqueteID) REFERENCES Paquete(PaqueteID),
    FOREIGN KEY (CatalogoID) REFERENCES Catalogo(CatalogoID)
);    


CREATE TABLE PaqueteXCarrito (
    PaqueteID INT,
    CarritoID INT,
    PRIMARY KEY (PaqueteID, CarritoID),
    FOREIGN KEY (PaqueteID) REFERENCES Paquete(PaqueteID),
    FOREIGN KEY (CarritoID) REFERENCES Carrito(CarritoID)
);    


CREATE TABLE SubcategoriaXProducto (
    SubcategoriaID INT,
    ProductoID INT,
    PRIMARY KEY (SubcategoriaID, ProductoID),
    FOREIGN KEY (SubcategoriaID) REFERENCES Subcategoria(SubcategoriaID),
    FOREIGN KEY (ProductoID) REFERENCES Producto(ProductoID)
);    


CREATE TABLE ProductosXCarrito (
    ProductoID INT,
    CarritoID INT,
    PRIMARY KEY (ProductoID, CarritoID),
    FOREIGN KEY (ProductoID) REFERENCES Producto(ProductoID),
    FOREIGN KEY (CarritoID) REFERENCES Carrito(CarritoID)
);    


CREATE TABLE ImagenXProducto (
    ImagenID INT,
    ProductoID INT,
    PRIMARY KEY (ImagenID, ProductoID),
    FOREIGN KEY (ImagenID) REFERENCES Imagen(ImagenID),
    FOREIGN KEY (ProductoID) REFERENCES Producto(ProductoID)
);    



CREATE TABLE TagsXImagen (
    TagID INT,
    ImagenID INT,
    PRIMARY KEY (TagID, ImagenID),
    FOREIGN KEY (TagID) REFERENCES Tag(TagID),
    FOREIGN KEY (ImagenID) REFERENCES Imagen(ImagenID)
);    



CREATE TABLE imagenesXmaquillaje (
    ImagenID INT,
    MaquillajeID INT,
    PRIMARY KEY (ImagenID, MaquillajeID),
    FOREIGN KEY (ImagenID) REFERENCES Imagen(ImagenID),
    FOREIGN KEY (MaquillajeID) REFERENCES Maquillaje(MaquillajeID)
);

CREATE TABLE Envio (
    EnvioID INT PRIMARY KEY,
    FechaPedido date,
    FechaEntrega date,
    EstadoID INT,
    CarritoID INT,
    DireccionID INT,
    FOREIGN KEY (EstadoID) REFERENCES Estado(EstadoID),
    FOREIGN KEY (CarritoID) REFERENCES Carrito(CarritoID),
    FOREIGN KEY (DireccionID) REFERENCES Direccion(DireccionID)
);    


GO
CREATE PROCEDURE InsertarCategoria
    @CategoriaID INT,
    @Nombre VARCHAR(100)
AS
BEGIN

    IF EXISTS (SELECT 1 FROM Categoria WHERE CategoriaID = @CategoriaID)
    BEGIN
        PRINT 'La CategoriaID no existe en la tabla Categoria. No se puede insertar el registro.';
        RETURN;
    END

    INSERT INTO Categoria (CategoriaID, Nombre)
    VALUES (@CategoriaID, @Nombre);
    PRINT 'Registro insertado exitosamente.';
END;


GO
CREATE PROCEDURE InsertarProducto 
    @ProductoID INT,
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(100),
    @Precio DECIMAL(10, 2),
    @Cantidad INT,
    @CategoriaID INT,
    @Estado bit,
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

    INSERT INTO Producto (ProductoID, Nombre, Descripcion, Precio, Cantidad, CategoriaID, Estado)
    VALUES (@ProductoID, @Nombre, @Descripcion, @Precio, @Cantidad, @CategoriaID, @Estado);

    PRINT 'Registro insertado exitosamente.';
END;


GO
CREATE PROCEDURE InsertarImagen
    @ImagenID INT,
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

    INSERT INTO Imagen (ImagenID, Nombre, Descripcion, Url)
    VALUES (@ImagenID, @Nombre, @Descripcion, @Url);

    PRINT 'Registro insertado exitosamente.';
END;

GO
CREATE PROCEDURE InsertarTag 
    @TagID INT,
    @Nombre VARCHAR(20)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Tag WHERE TagID = @TagID)
    BEGIN
        PRINT 'El TagID ya existe. No se puede insertar el registro.';
        RETURN;
    END

    INSERT INTO Tag (TagID, Nombre)
    VALUES (@TagID, @Nombre);

    PRINT 'Registro insertado exitosamente.';
END;

GO
CREATE PROCEDURE InsertarSubcategoria
    @SubcategoriaID INT,
    @Nombre VARCHAR(50)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Subcategoria WHERE SubcategoriaID = @SubcategoriaID)
    BEGIN
        PRINT 'El SubcategoriaID ya existe. No se puede insertar el registro.';
        RETURN;
    END

    INSERT INTO Subcategoria (SubcategoriaID, Nombre)
    VALUES (@SubcategoriaID, @Nombre);

    PRINT 'Registro insertado exitosamente.';
END;


GO
CREATE PROCEDURE insertarPaquete
    @PaqueteID INT,
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

    INSERT INTO Paquete (PaqueteID, Nombre, Descripcion, Precio, CantidadDisponible, Estado)
    VALUES (@PaqueteID, @Nombre, @Descripcion, @Precio, @CantidadDisponible, @Estado);

    PRINT 'Registro insertado exitosamente.';
END;


GO
CREATE PROCEDURE InsertarCatalogo
    @CatalogoID INT,
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

    INSERT INTO Catalogo (CatalogoID, Nombre, Descripcion, Estado)
    VALUES (@CatalogoID, @Nombre, @Descripcion, @Estado);

    PRINT 'Registro insertado exitosamente.';
END;


GO
CREATE PROCEDURE InsertarDireccion
    @DireccionID INT,
    @Provincia VARCHAR(20),
    @Canton VARCHAR(20),
    @Distrito VARCHAR(20),
    @Detalle VARCHAR(20)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Direccion WHERE DireccionID = @DireccionID)
    BEGIN
        PRINT 'El DireccionID ya existe. No se puede insertar el registro.';
        RETURN;
    END

    INSERT INTO Direccion (DireccionID, Provincia, Canton, Distrito, Detalle)
    VALUES (@DireccionID, @Provincia, @Canton, @Distrito, @Detalle);

    PRINT 'Registro insertado exitosamente.';
END;



GO
CREATE PROCEDURE InsertarEstado
    @EstadoID INT,
    @Estado VARCHAR(20)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Estado WHERE EstadoID = @EstadoID)
    BEGIN
        PRINT 'El EstadoID ya existe. No se puede insertar el registro.';
        RETURN;
    END

    INSERT INTO Estado (EstadoID, Estado)
    VALUES (@EstadoID, @Estado);

    PRINT 'Registro insertado exitosamente.';
END;


GO
CREATE PROCEDURE InsertarEnvio
    @EnvioID INT,
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

    INSERT INTO Envio (EnvioID, FechaPedido, FechaEntrega, EstadoID, CarritoID, DireccionID)
    VALUES (@EnvioID, @FechaPedido, @FechaEntrega, @EstadoID, @CarritoID, @DireccionID);

    PRINT 'Registro insertado exitosamente.';
END;


GO
CREATE PROCEDURE agregarCarrito
    @CarritoID INT,
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

    INSERT INTO Carrito (CarritoID, UsuarioID)
    VALUES (@CarritoID, @UsuarioID);

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
    @MaquillajeID INT,
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

    INSERT INTO Maquillaje (MaquillajeID, Nombre, Descripcion, Estado)
    VALUES (@MaquillajeID, @Nombre, @Descripcion, @Estado);

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



