select *from Usuario
-- Insertar tipos de entrada en la tabla TipoEntradaAgenda

-- Maquillajes
INSERT INTO TipoEntradaAgenda (TipoEntrada, PermiteTraslape)
VALUES ('maquillajes', 0);

-- Entregar pedido
INSERT INTO TipoEntradaAgenda (TipoEntrada, PermiteTraslape)
VALUES ('entregar pedido', 1);

-- Revisar inventario
INSERT INTO TipoEntradaAgenda (TipoEntrada, PermiteTraslape)
VALUES ('revisar inventario', 1);


-- Insertar un evento en la agenda (cambiar los valores según sea necesario)
DECLARE @UsuarioID INT = 3;
DECLARE @Detalle NVARCHAR(100) = 'Entregar pedido';
DECLARE @FechaInicio DATETIME = '2023-11-22 10:00:00';
DECLARE @DuracionHoras INT = 5;
DECLARE @TipoEntrada NVARCHAR(50) = 'entregar pedido';  -- Asegurase de que este tipo exista en la tabla TipoEntradaAgenda

-- Verificar si hay traslape (ajustar según sea necesario)
IF NOT EXISTS (
    SELECT 1
    FROM Agenda
    WHERE UsuarioID = @UsuarioID
        AND TipoEntrada = @TipoEntrada
        AND (
            (@FechaInicio >= FechaInicio AND @FechaInicio < DATEADD(HOUR, @DuracionHoras, FechaInicio))
            OR (DATEADD(HOUR, @DuracionHoras, @FechaInicio) > FechaInicio AND DATEADD(HOUR, @DuracionHoras, @FechaInicio) <= DATEADD(HOUR, @DuracionHoras, FechaInicio))
        )
)
BEGIN
    -- Insertar el evento si no hay traslape
    INSERT INTO Agenda (UsuarioID, Detalle, FechaInicio, DuracionHoras, TipoEntrada)
    VALUES (@UsuarioID, @Detalle, @FechaInicio, @DuracionHoras, @TipoEntrada);
    PRINT 'Evento insertado correctamente.';
END
ELSE
BEGIN
    PRINT 'Error: Existe un traslape con otro evento.';
END
