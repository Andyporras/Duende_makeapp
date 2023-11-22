go
CREATE or alter PROCEDURE InsertarCategoria
    @Nombre VARCHAR(100)
AS
BEGIN
    INSERT INTO Categoria (Nombre)
    VALUES (@Nombre);
    PRINT 'Registro insertado exitosamente.';
END;
GO

CREATE or alter PROCEDURE InsertarProducto 
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(100),
    @Precio DECIMAL(10, 2),
    @Cantidad INT,
    @CategoriaID INT,
    @Estado bit
AS
BEGIN
    INSERT INTO Producto (Nombre, Descripcion, Precio, Cantidad, CategoriaID, Estado)
    VALUES (@Nombre, @Descripcion, @Precio, @Cantidad, @CategoriaID, @Estado);

    PRINT 'Registro insertado exitosamente.';
END;
GO

CREATE or alter PROCEDURE InsertarImagen
    @Nombre VARCHAR(30),
    @Descripcion VARCHAR(100),
    @Url VARCHAR(100)
AS
BEGIN

    INSERT INTO Imagen (Nombre, Descripcion, Url)
    VALUES (@Nombre, @Descripcion, @Url);

    PRINT 'Registro insertado exitosamente.';
END;
GO

CREATE or alter PROCEDURE InsertarTag 
    @Nombre VARCHAR(20)
AS
BEGIN

    INSERT INTO Tag (Nombre)
    VALUES (@Nombre);

    PRINT 'Registro insertado exitosamente.';
END;
GO

CREATE or alter PROCEDURE InsertarSubcategoria
    @Nombre VARCHAR(50)
AS
BEGIN

    INSERT INTO Subcategoria (Nombre)
    VALUES (@Nombre);

    PRINT 'Registro insertado exitosamente.';
END;
GO

CREATE or alter PROCEDURE insertarPaquete
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(100),
    @Precio DECIMAL(10, 2),
    @CantidadDisponible INT,
    @Estado bit
AS
BEGIN

    INSERT INTO Paquete (Nombre, Descripcion, Precio, CantidadDisponible, Estado)
    VALUES (@Nombre, @Descripcion, @Precio, @CantidadDisponible, @Estado);

    PRINT 'Registro insertado exitosamente.';
END;
GO

CREATE or alter PROCEDURE InsertarCatalogo
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(100),
    @Estado bit
AS
BEGIN

    INSERT INTO Catalogo (Nombre, Descripcion, Estado)
    VALUES (@Nombre, @Descripcion, @Estado);

    PRINT 'Registro insertado exitosamente.';
END;
GO

CREATE or alter PROCEDURE InsertarDireccion
    @ProvinciaiD VARCHAR(20),
    @Detalle VARCHAR(20)
AS
BEGIN
    INSERT INTO Direccion (ProvinciaiD, Detalle)
    VALUES (@ProvinciaiD, @Detalle);

    PRINT 'Registro insertado exitosamente.';
END;
GO

CREATE or alter PROCEDURE InsertarEstado
    @Estado VARCHAR(20)
AS
BEGIN

    INSERT INTO EstadoEnvio(Estado)
    VALUES (@Estado);

    PRINT 'Registro insertado exitosamente.';
END;


GO
CREATE or alter PROCEDURE InsertarEnvio
    @FechaPedido date,
    @FechaEntrega date,
    @EstadoID INT,
    @CarritoID INT,
    @DireccionID INT
AS
BEGIN

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
CREATE or alter PROCEDURE agregarCarrito
    @UsuarioID INT
AS
BEGIN

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
CREATE or alter PROCEDURE InsertarProductosXCatalogo
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
CREATE or alter PROCEDURE InsertarProductosXPaquete
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
CREATE or alter PROCEDURE InsertarPaqueteXCatalogo
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
CREATE or alter PROCEDURE InsertarPaqueteXCarrito
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
CREATE or alter PROCEDURE InsertarSubcategoriaXProducto
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
CREATE or alter PROCEDURE InsertarProductosXCarrito
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
CREATE or alter PROCEDURE InsertarMaquillaje
    @Nombre VARCHAR(30),
    @Descripcion VARCHAR(100),
    @Estado bit
AS
BEGIN
    INSERT INTO Maquillaje (Nombre, Descripcion, Estado)
    VALUES (@Nombre, @Descripcion, @Estado);

    PRINT 'Registro insertado exitosamente.';
END;

go
create or alter procedure registrarUsuario
@cedula varchar(9),
@nombre varchar (20),
@apellido1 varchar (50),
@correo varchar (50),
@usuario varchar (20),
@clave varchar (500),
@TipoUsarioID int,
@patron varchar(100)
as
IF EXISTS (select Correo from Usuario where Correo = @correo)
    BEGIN
        return -1;
    END
ELSE
begin
BEGIN TRY
    insert into usuario(nombre, apellido, correo, usuario, clave, TipoID)
    values(@nombre, @apellido1, @correo, @usuario, ENCRYPTBYPASSPHRASE (@patron, @clave), @TipoUsarioID)
    RETURN 0
END TRY
BEGIN CATCH
    RETURN -2
END CATCH
end


go
create OR alter procedure validarUsuario
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


GO
CREATE OR ALTER PROCEDURE EliminarProducto
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


GO
CREATE OR ALTER PROCEDURE EliminarPaquete
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


GO
CREATE OR ALTER PROCEDURE EliminarCatalogo
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


GO
CREATE OR ALTER PROCEDURE EliminarMaquillaje
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


--sp para crear carrito si no existe 

go
create or alter proc AgregarProductoCarrito(
@idProducto int,
@idCarrito int,
@idCliente int
)  
as 
begin
	if exists(select * from Carrito where UsuarioID = @IdCliente)
		insert into ProductosXCarrito values(@idProducto, @idCarrito, 1)
	else
		print'el cliente no tiene un carrito'
end
             

go
CREATE or alter PROCEDURE ObtenerProductosEnCarrito
    @UsuarioID INT,
    @CarritoID INT
AS
BEGIN
    SELECT p.ProductoID, p.Nombre, p.Descripcion, p.Precio, ProductosXCarrito.Cantidad, p.CategoriaID, p.Estado, p.ImagenID, Imagen.Url
    FROM ProductosXCarrito
    INNER JOIN Carrito ON ProductosXCarrito.CarritoID = Carrito.CarritoID
    INNER JOIN Producto p ON ProductosXCarrito.ProductoID = p.ProductoID
    INNER JOIN Usuario ON Carrito.UsuarioID = Usuario.UsuarioID
	INNER JOIN Imagen on p.ImagenID = Imagen.ImagenID
    WHERE Usuario.UsuarioID = @UsuarioID AND Carrito.CarritoID = @CarritoID;
END;




go
CREATE OR ALTER PROC RestarCantidadProductos
(
    @IDProducto INT,
    @IDCarrito INT,
	@cantFinal INT
)
AS
BEGIN

    UPDATE ProductosXCarrito
    SET Cantidad = Cantidad - 1  , @cantFinal = Cantidad - 1
    WHERE ProductoID = @IDProducto AND CarritoID = @IDCarrito;
    IF @cantFinal = 0
    BEGIN

        DELETE FROM ProductosXCarrito
        WHERE ProductoID = @IDProducto AND CarritoID = @IDCarrito;
    END;
END;




go
CREATE OR ALTER PROC SumarCantidadProductos
(
    @IDProducto INT,
	@IDCarrito INT
)
AS
BEGIN
    UPDATE ProductosXCarrito
    SET Cantidad = Cantidad+1
    WHERE ProductoID = @IDProducto and CarritoID = @IDCarrito;
END;


GO
create or alter procedure eliminarDelCarrito
(
	@IDProducto int,
	@IDCarrito int
)
as 
begin
	delete ProductosXCarrito
	where ProductoID = @IDProducto and CarritoID = @IDCarrito
END;
go
CREATE or alter FUNCTION ObtenerCarritoPorUsuarioID (@UsuarioID INT)
RETURNS INT
AS
BEGIN
    DECLARE @CarritoID INT

    SELECT @CarritoID = CarritoID
    FROM Carrito
    WHERE UsuarioID = @UsuarioID
	AND estado = 1

    RETURN @CarritoID
END;
go

----------------------PREPARACION DE SP PARA CHECKOUT------------------------
--ALTER TABLE Provincia
--ADD CONSTRAINT SinRepeticiones UNIQUE (Nombre);
--insert into provincia (Nombre) values 
--('San Jose'), ('Alajuela'), ('Cartago'), ('Heredia'), ('Guanacaste'), ('Puntarenas'), ('Limon') --ids del 1 al 7
--insert into EstadoEnvio (Estado) values ('En ruta'), ('Entregado') -- ids 1 y 2

go
CREATE OR ALTER PROCEDURE concretarVenta (
@usuario int,
@carrito int,
@codPostal int,
@direccion VARCHAR(100),
@provincia int,
@imagenID int
)
AS
BEGIN
	-- Validar que productosxcarrito no est� vac�o
	DECLARE @carritoCount INT;
	SELECT @carritoCount = COUNT(*) FROM ProductosXCarrito WHERE CarritoID = @carrito;
	IF @carritoCount > 0
	BEGIN
		-- Continuar con el resto del procedimiento
		UPDATE carrito SET estado = 0 WHERE CarritoID = @carrito; -- Deshabilitar el carrito del usuario
		INSERT INTO CARRITO (UsuarioID, estado) VALUES (@usuario, 1); -- Crear un nuevo carrito para el usuario

		-- Creaci�n de la direcci�n
		INSERT INTO Direccion (CodigoPostal, Detalle, ProvinciaID) VALUES (@codPostal, @direccion, @provincia);
		DECLARE @DireccionID int;
		SELECT @DireccionID = SCOPE_IDENTITY(); -- Reservar el id de la direcci�n para m�s adelante
		-- Creaci�n de la venta
		INSERT INTO Venta (monto, imgComprobante, CarritoID, codPostal, fechaEntrega, fechaPedido, direccion, estado, ProvinciaID) 
		VALUES (0, @imagenID, @carrito, @codPostal, null, CONVERT(date, GETDATE()), @DireccionID, 1, @provincia);

		-- Creaci�n del env�o
		
		INSERT INTO Envio(FechaPedido, FechaEntrega, EstadoID, CarritoID, DireccionID)
		VALUES (CONVERT(date, GETDATE()), null, 1, @carrito, @DireccionID); -- la fecha de entrega la determina el admin al aprobar
	END
	ELSE
	BEGIN
		-- Terminar el procedimiento si el carrito est� vac�o
		RETURN;
	END
END;

-- funcion para el envio del correo
go
CREATE OR ALTER FUNCTION ObtenerDetalle(@carritoID INT)
RETURNS VARCHAR(200)
AS
BEGIN
    DECLARE @detalle VARCHAR(200) = '';

    -- Declarar una variable para mantener el detalle
    DECLARE @detalleCompra VARCHAR(MAX) = '';

    -- Declarar una variable para el subtotal
    DECLARE @subtotal DECIMAL(10, 2) = 0;

    -- Utilizar un cursor para iterar a trav�s de los productos en el carrito
    DECLARE @productoID INT;
    DECLARE @cantidad INT;
    DECLARE @precio DECIMAL(10, 2);
    
    DECLARE carrito_cursor CURSOR FOR
    SELECT pc.productoID, pc.cantidad, p.precio
    FROM productosxcarrito pc
    JOIN Producto p ON pc.productoID = p.productoID
    WHERE pc.carritoID = @carritoID;

    OPEN carrito_cursor;

    FETCH NEXT FROM carrito_cursor INTO @productoID, @cantidad, @precio;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Agregar detalles al detalle de compra
        SET @detalleCompra = @detalleCompra + 
            'Producto: ' + CAST((select nombre from producto where productoID = @productoID) AS VARCHAR) + ', ' +
            'Precio: ' + CAST(@precio AS VARCHAR) + ', ' +
            'Cantidad: ' + CAST(@cantidad AS VARCHAR) + char(10);

        -- Calcular subtotal
        SET @subtotal = @subtotal + (@precio * @cantidad);

        FETCH NEXT FROM carrito_cursor INTO @productoID, @cantidad, @precio;
    END;

    CLOSE carrito_cursor;
    DEALLOCATE carrito_cursor;

    -- Agregar el subtotal al detalle de compra
    SET @detalle = @detalleCompra + 'Subtotal: ' + CAST(@subtotal AS VARCHAR) + char(10);

    -- Retornar el detalle
    RETURN @detalle;
END;
go

----------------------PREPARACION DE SP PARA CHECKOUT------------------------

go
CREATE or alter proc ObtenerVentas
AS
BEGIN
    SELECT  Usuario.Correo ,VentaID ,monto, fechaPedido, fechaEntrega, direccion, venta.estado, Url
	from venta 
    join Imagen on Imagen.ImagenID = Venta.imgComprobante
	join Carrito on Carrito.CarritoID = Venta.CarritoID
	join Usuario on Usuario.UsuarioID = Carrito.UsuarioID
END;
go

go 
create or alter proc obtenerPedidosClientes
@usuario int
as 
begin
	SELECT monto, fechaPedido, fechaEntrega, direccion, venta.estado, Url
	from venta 
    join Imagen on Imagen.ImagenID = Venta.imgComprobante
	join Carrito on Carrito.CarritoID = Venta.CarritoID
	where UsuarioID = @Usuario
end;



go
create or alter proc aprobarVenta
@idVenta int
as
begin
update venta 
set estado = 1
where @idVenta = venta.VentaID
end;


go
create or alter proc denegarVenta
@idVenta int
as
begin
update venta 
set estado = 2
where @idVenta = venta.VentaID
end;



select * from Usuario


