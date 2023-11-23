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
DECLARE @Asunto NVARCHAR(100) = 'Entregar pedido';
DECLARE @FechaInicio DATETIME = '2023-11-22 10:00:00';
DECLARE @DuracionMinutos INT = 60;
DECLARE @TipoEntrada NVARCHAR(50) = 'entregar pedido';  -- Asegúrate de que este tipo exista en la tabla TipoEntradaAgenda

-- Verificar si hay traslape (ajustar según sea necesario)
IF NOT EXISTS (
    SELECT 1
    FROM Agenda
    WHERE UsuarioID = @UsuarioID
        AND TipoEntrada = @TipoEntrada
        AND (
            (@FechaInicio >= FechaInicio AND @FechaInicio < DATEADD(MINUTE, DuracionMinutos, FechaInicio))
            OR (DATEADD(MINUTE, @DuracionMinutos, @FechaInicio) > FechaInicio AND DATEADD(MINUTE, @DuracionMinutos, @FechaInicio) <= DATEADD(MINUTE, DuracionMinutos, FechaInicio))
        )
)
BEGIN
    -- Insertar el evento si no hay traslape
    INSERT INTO Agenda (UsuarioID, Asunto, FechaInicio, DuracionMinutos, TipoEntrada)
    VALUES (@UsuarioID, @Asunto, @FechaInicio, @DuracionMinutos, @TipoEntrada);
    PRINT 'Evento insertado correctamente.';
END
ELSE
BEGIN
    PRINT 'Error: Existe un traslape con otro evento.';
END
