-- 1. Crear Base de Datos
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'GestionAuditDB')
BEGIN
    CREATE DATABASE GestionAuditDB;
END
GO

USE GestionAuditDB;
GO

-- 2. Limpieza de tablas si existen (para re-ejecución limpia)
DROP VIEW IF EXISTS dbo.vw_ResumenAuditoriasFinalizadas;
DROP TABLE IF EXISTS dbo.Hallazgos;
DROP TABLE IF EXISTS dbo.Auditorias;
DROP TABLE IF EXISTS dbo.Responsables;
GO

-- ---------------------------------------------------------
-- 3. Creación de Tablas
-- ---------------------------------------------------------

-- Tabla: Responsables [cite: 24]
CREATE TABLE dbo.Responsables (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Correo NVARCHAR(150) NOT NULL UNIQUE, -- Unique para evitar duplicados
    Area NVARCHAR(100) NOT NULL,
    FechaRegistro DATETIME DEFAULT GETDATE()
);
GO

-- Tabla: Auditorias [cite: 12]
CREATE TABLE dbo.Auditorias (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Titulo NVARCHAR(200) NOT NULL,
    FechaInicio DATETIME NOT NULL,
    FechaFin DATETIME NULL,
    AreaAuditada NVARCHAR(100) NOT NULL,
    Estado NVARCHAR(20) NOT NULL DEFAULT 'Pendiente',
    ResponsableId INT NOT NULL,
    
    -- FK hacia Responsables [cite: 26]
    CONSTRAINT FK_Auditorias_Responsables FOREIGN KEY (ResponsableId) 
        REFERENCES dbo.Responsables(Id),

    -- Validación de Estados Permitidos [cite: 16, 17, 18]
    -- Esto asegura la integridad del dato a nivel de motor
    CONSTRAINT CK_Auditoria_Estado CHECK (Estado IN ('Pendiente', 'En Proceso', 'Finalizada')),

    -- Validación lógica: FechaFin no puede ser menor a FechaInicio
    CONSTRAINT CK_Auditoria_Fechas CHECK (FechaFin >= FechaInicio)
);
GO

-- Tabla: Hallazgos [cite: 19]
CREATE TABLE dbo.Hallazgos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Descripcion NVARCHAR(MAX) NOT NULL,
    Tipo NVARCHAR(50) NOT NULL, -- 'Observación', 'No Conformidad'
    Severidad NVARCHAR(20) NOT NULL, -- 'Baja', 'Media', 'Alta'
    FechaDeteccion DATETIME NOT NULL DEFAULT GETDATE(),
    AuditoriaId INT NOT NULL,

    -- FK hacia Auditorias [cite: 20]
    CONSTRAINT FK_Hallazgos_Auditorias FOREIGN KEY (AuditoriaId) 
        REFERENCES dbo.Auditorias(Id) ON DELETE CASCADE,

    -- Validaciones de Dominio [cite: 21]
    CONSTRAINT CK_Hallazgo_Tipo CHECK (Tipo IN ('Observación', 'No Conformidad')),
    CONSTRAINT CK_Hallazgo_Severidad CHECK (Severidad IN ('Baja', 'Media', 'Alta'))
);
GO

-- ---------------------------------------------------------
-- 4. Creación de Vista para Reportes 
-- ---------------------------------------------------------
/* Requerimiento: Vista SQL que muestre auditorías finalizadas 
   con número de hallazgos por severidad.
*/
CREATE VIEW dbo.vw_ResumenAuditoriasFinalizadas
AS
SELECT 
    a.Id AS AuditoriaId,
    a.Titulo,
    a.FechaInicio,
    a.FechaFin,
    r.Nombre AS Responsable,
    -- Conteo condicional para pivotear las severidades en columnas
    COUNT(CASE WHEN h.Severidad = 'Baja' THEN 1 END) AS HallazgosBajos,
    COUNT(CASE WHEN h.Severidad = 'Media' THEN 1 END) AS HallazgosMedios,
    COUNT(CASE WHEN h.Severidad = 'Alta' THEN 1 END) AS HallazgosAltos,
    COUNT(h.Id) AS TotalHallazgos
FROM 
    dbo.Auditorias a
INNER JOIN 
    dbo.Responsables r ON a.ResponsableId = r.Id
LEFT JOIN 
    dbo.Hallazgos h ON a.Id = h.AuditoriaId
WHERE 
    a.Estado = 'Finalizada' -- Solo auditorías finalizadas
GROUP BY 
    a.Id, a.Titulo, a.FechaInicio, a.FechaFin, r.Nombre;
GO

-- ---------------------------------------------------------
-- 5. Datos de Prueba (Seed Data) - Opcional para validar
-- ---------------------------------------------------------
INSERT INTO Responsables (Nombre, Correo, Area) VALUES 
('Juan Perez', 'juan.perez@empresa.com', 'Finanzas'),
('Maria Gomez', 'maria.gomez@empresa.com', 'IT');

INSERT INTO Auditorias (Titulo, FechaInicio, FechaFin, AreaAuditada, Estado, ResponsableId) VALUES 
('Auditoría Q1 Finanzas', '2023-01-01', '2023-01-15', 'Finanzas', 'Finalizada', 1),
('Auditoría Seguridad IT', '2023-02-01', NULL, 'IT', 'En Proceso', 2);

INSERT INTO Hallazgos (Descripcion, Tipo, Severidad, FechaDeteccion, AuditoriaId) VALUES 
('Falta firma en documento', 'Observación', 'Baja', '2023-01-05', 1),
('Diferencia en caja chica', 'No Conformidad', 'Alta', '2023-01-06', 1),
('Servidor sin parche', 'No Conformidad', 'Alta', '2023-02-02', 2);
GO

-- Prueba de la vista
SELECT * FROM dbo.vw_ResumenAuditoriasFinalizadas;