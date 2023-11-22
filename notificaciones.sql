-- Create the Notificaciones (Notifications) table with auto-incremental primary key
CREATE TABLE Notificaciones (
    notificacion_id INT IDENTITY(1,1) PRIMARY KEY,
    titulo NVARCHAR(100),
    mensaje NVARCHAR(255),
    usuario_id INT,
    fecha_envio DATETIME,
    visto BIT,  -- Se agrega la columna para rastrear el estado de visualizaci�n

    CONSTRAINT fk_notificacion_usuario FOREIGN KEY (usuario_id) REFERENCES Usuario(UsuarioId)
);
select * from venta
exec obtenerVentas

-- Insertar notificaciones de prueba asociadas a los usuarios
INSERT INTO Notificaciones (titulo, mensaje, usuario_id, fecha_envio, visto)
VALUES
    ('Notificaci�n 1', 'Mensaje de notificaci�n 1', 3, '2023-11-18 10:00:00', 0),
    ('Notificaci�n 2', 'Mensaje de notificaci�n 2', 3, '2023-11-18 11:30:00', 0),
    ('Notificaci�n 3', 'Mensaje de notificaci�n 3', 3, '2023-11-18 12:45:00', 0),
    ('Notificaci�n 4', 'Mensaje de notificaci�n 4', 3, '2023-11-18 14:15:00', 0);

SELECT
    Notificaciones.notificacion_id,
    Notificaciones.titulo,
    Notificaciones.mensaje,
    Notificaciones.usuario_id,
    Usuario.nombre AS nombre_usuario,
    Notificaciones.fecha_envio,
    Notificaciones.visto
FROM Notificaciones
INNER JOIN Usuario ON Notificaciones.usuario_id = Usuario.UsuarioID;
