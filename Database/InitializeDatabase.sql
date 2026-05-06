-- Crear tabla de Estados
CREATE TABLE Estados (
    Clave NVARCHAR(10) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL
);

-- Crear tabla de Municipios
CREATE TABLE Municipios (
    IdMunicipio INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    ContadorTurnos INT DEFAULT 0
);

-- Crear tabla de Estudiantes
CREATE TABLE Estudiantes (
    CURP NVARCHAR(18) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    ApellidoPaterno NVARCHAR(100) NOT NULL,
    ApellidoMaterno NVARCHAR(100) NOT NULL,
    FechaNacimiento DATETIME NOT NULL,
    Sexo CHAR(1) NOT NULL,
    Edad INT NOT NULL,
    EstadoNacimiento NVARCHAR(50),
    MunicipioEstudio NVARCHAR(100),
    TelefonoContacto NVARCHAR(20),
    NivelEducativo NVARCHAR(50),
    Grado INT
);

-- Crear tabla de SolicitudesTurno
CREATE TABLE SolicitudesTurno (
    NumeroTurno INT PRIMARY KEY IDENTITY(1,1),
    CURP NVARCHAR(18) NOT NULL FOREIGN KEY REFERENCES Estudiantes(CURP),
    Municipio NVARCHAR(100) NOT NULL,
    FechaSolicitud DATETIME NOT NULL,
    Asunto NVARCHAR(500),
    PersonaTramitera NVARCHAR(100),
    Parentesco NVARCHAR(50),
    Estatus INT NOT NULL DEFAULT 0
);

-- Crear índices para optimización
CREATE INDEX IX_SolicitudesTurno_CURP ON SolicitudesTurno(CURP);
CREATE INDEX IX_SolicitudesTurno_Municipio ON SolicitudesTurno(Municipio);
CREATE INDEX IX_SolicitudesTurno_Estatus ON SolicitudesTurno(Estatus);
CREATE INDEX IX_SolicitudesTurno_FechaSolicitud ON SolicitudesTurno(FechaSolicitud);

-- Insertar estados
INSERT INTO Estados VALUES ('AGS', 'Aguascalientes');
INSERT INTO Estados VALUES ('BC', 'Baja California');
INSERT INTO Estados VALUES ('BCS', 'Baja California Sur');
INSERT INTO Estados VALUES ('CAMP', 'Campeche');
INSERT INTO Estados VALUES ('COAH', 'Coahuila');
INSERT INTO Estados VALUES ('COL', 'Colima');
INSERT INTO Estados VALUES ('CHIS', 'Chiapas');
INSERT INTO Estados VALUES ('CHIH', 'Chihuahua');
INSERT INTO Estados VALUES ('CDMX', 'Ciudad de México');
INSERT INTO Estados VALUES ('DGO', 'Durango');
INSERT INTO Estados VALUES ('GTO', 'Guanajuato');
INSERT INTO Estados VALUES ('GRO', 'Guerrero');
INSERT INTO Estados VALUES ('HGO', 'Hidalgo');
INSERT INTO Estados VALUES ('JAL', 'Jalisco');
INSERT INTO Estados VALUES ('MEX', 'México');
INSERT INTO Estados VALUES ('MICH', 'Michoacán');
INSERT INTO Estados VALUES ('MOR', 'Morelos');
INSERT INTO Estados VALUES ('NAY', 'Nayarit');
INSERT INTO Estados VALUES ('NL', 'Nuevo León');
INSERT INTO Estados VALUES ('OAX', 'Oaxaca');
INSERT INTO Estados VALUES ('PUE', 'Puebla');
INSERT INTO Estados VALUES ('QRO', 'Querétaro');
INSERT INTO Estados VALUES ('QROO', 'Quintana Roo');
INSERT INTO Estados VALUES ('SLP', 'San Luis Potosí');
INSERT INTO Estados VALUES ('SIN', 'Sinaloa');
INSERT INTO Estados VALUES ('SON', 'Sonora');
INSERT INTO Estados VALUES ('TAB', 'Tabasco');
INSERT INTO Estados VALUES ('TAMS', 'Tamaulipas');
INSERT INTO Estados VALUES ('TLAX', 'Tlaxcala');
INSERT INTO Estados VALUES ('VER', 'Veracruz');
INSERT INTO Estados VALUES ('YUC', 'Yucatán');
INSERT INTO Estados VALUES ('ZAC', 'Zacatecas');

-- Insertar municipios de Coahuila
INSERT INTO Municipios (Nombre, ContadorTurnos) VALUES ('Saltillo', 0);
INSERT INTO Municipios (Nombre, ContadorTurnos) VALUES ('Torreón', 0);
INSERT INTO Municipios (Nombre, ContadorTurnos) VALUES ('Monclova', 0);
INSERT INTO Municipios (Nombre, ContadorTurnos) VALUES ('Parras', 0);
INSERT INTO Municipios (Nombre, ContadorTurnos) VALUES ('Matamoros', 0);
INSERT INTO Municipios (Nombre, ContadorTurnos) VALUES ('Acuña', 0);
INSERT INTO Municipios (Nombre, ContadorTurnos) VALUES ('Frontera', 0);
INSERT INTO Municipios (Nombre, ContadorTurnos) VALUES ('Castaños', 0);
INSERT INTO Municipios (Nombre, ContadorTurnos) VALUES ('Nadadores', 0);
INSERT INTO Municipios (Nombre, ContadorTurnos) VALUES ('Escobedo', 0);
