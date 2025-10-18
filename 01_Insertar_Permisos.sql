-- Primero, eliminamos todos los registros existentes para evitar duplicados.
DELETE FROM Permiso;
GO

-- Opcional: Reiniciamos el contador de identidad (ID) a 1.
-- Esto es para SQL Server. Si usas otra base de datos, el comando puede variar.
-- (por ejemplo, para MySQL sería: ALTER TABLE Permiso AUTO_INCREMENT = 1;)
DBCC CHECKIDENT ('Permiso', RESEED, 0);
GO

-- Insertamos todos los permisos en la tabla.
INSERT INTO Permiso (Nombre) VALUES
-- Gestión de Usuarios y Roles
('Listar_Usuarios'),
('Crear_Usuario'),
('Editar_Usuario'),
('Borrar_Usuario'),
('Bloquear_Usuario'),
('Asignar_Roles_Usuario'),
('Resetear_Password_Usuario'),
('Listar_Roles'),
('Crear_Rol'),
('Editar_Rol'),
('Borrar_Rol'),
('Asignar_Permisos_Rol'),

-- Gestión de Contenido
('Listar_Novedades'),
('Crear_Novedad'),
('Editar_Novedad'),
('Borrar_Novedad'),
('Publicar_Novedad'),
('Listar_FAQs'),
('Crear_FAQ'),
('Editar_FAQ'),
('Borrar_FAQ'),

-- Gestión de Empresas
('Listar_Empresas'),
('Crear_Empresa'),
('Editar_Empresa'),
('Borrar_Empresa'),

-- Gestión de Encuestas
('Listar_Encuestas'),
('Crear_Encuesta'),
('Editar_Encuesta'),
('Borrar_Encuesta'),
('Enviar_Encuesta'),
('Ver_Resultados_Encuesta'),

-- Marketing y Feedback
('Listar_Publicidades'),
('Crear_Publicidad'),
('Editar_Publicidad'),
('Borrar_Publicidad'),
('Listar_Resenas'),
('Aprobar_Resena'),
('Borrar_Resena'),
('Ver_Estadisticas_Resenas'),

-- Administración del Sistema
('Acceder_Consola_Admin'),
('Ver_Bitacora'),
('Listar_Backups'),
('Crear_Backup'),
('Restaurar_Backup'),
('Descargar_Backup'),
('Listar_Idiomas'),
('Importar_Idioma'),
('Exportar_Idioma'),
('Borrar_Idioma'),
('Gestionar_Traducciones'),

-- Gestión Financiera
('Listar_Notas_Credito'),
('Crear_Nota_Credito'),
('Contratar_Plan'),

-- Permisos de Usuario (No Administrador)
('Editar_Mi_Perfil'),
('Cambiar_Mi_Password'),
('Ver_Mi_Cuenta');
GO

PRINT 'Se han insertado los permisos correctamente en la tabla Permiso.';
GO
