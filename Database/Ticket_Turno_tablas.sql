-- TABLA CATÁLOGO DE ESTADOS
CREATE TABLE Estado (
    IdEstado INT PRIMARY KEY IDENTITY(1,1),
    NombreEstado NVARCHAR(100) NOT NULL,
    Abreviatura NVARCHAR(2) NOT NULL UNIQUE
);

-- TABLA CATÁLOGO DE MUNICIPIOS
CREATE TABLE Municipio (
    IdMunicipio INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL UNIQUE,
    ContadorTurno INT DEFAULT 0
);

-- TABLA CATÁLOGO DE NIVELES EDUCATIVOS
CREATE TABLE NivelEducativo (
    IdNivel INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL UNIQUE
);

-- TABLA ESTUDIANTES
CREATE TABLE Estudiante (
    CURP NVARCHAR(18) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    ApellidoPaterno NVARCHAR(100) NOT NULL,
    ApellidoMaterno NVARCHAR(100) NOT NULL,
    FechaNacimiento DATE NOT NULL,
    Sexo CHAR(1) NOT NULL CHECK (Sexo IN ('H', 'M')),
    IdEstadoNacimiento INT NOT NULL,
    IdMunicipioEstudio INT NOT NULL,
    TelefonoContacto NVARCHAR(20),
    IdNivelEducativo INT NOT NULL,
    Grado INT NOT NULL,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    Activo BIT DEFAULT 1,
    FOREIGN KEY (IdEstadoNacimiento) REFERENCES Estado(IdEstado),
    FOREIGN KEY (IdMunicipioEstudio) REFERENCES Municipio(IdMunicipio),
    FOREIGN KEY (IdNivelEducativo) REFERENCES NivelEducativo(IdNivel)
);

-- TABLA SOLICITUDES DE TURNO
CREATE TABLE SolicitudesTurno (
    NumeroTurno INT PRIMARY KEY,
    CURP NVARCHAR(18) NOT NULL,
    IdMunicipio INT NOT NULL,
    FechaSolicitud DATETIME DEFAULT GETDATE(),
    Asunto NVARCHAR(500) NOT NULL,
    PersonaTramite NVARCHAR(150) NOT NULL,
    Parentesco NVARCHAR(50) NOT NULL,
    Estatus NVARCHAR(20) DEFAULT 'Pendiente' CHECK (Estatus IN ('Pendiente', 'Resuelto')),
    FechaResolucion DATETIME NULL,
    FOREIGN KEY (CURP) REFERENCES Estudiante(CURP),
    FOREIGN KEY (IdMunicipio) REFERENCES Municipio(IdMunicipio)
);

-- ÍNDICES PARA CONSULTAS FRECUENTES
CREATE INDEX IDX_Estudiantes_Nombre ON Estudiante(Nombre);
CREATE INDEX IDX_SolicitudesTurno_CURP ON SolicitudesTurno(CURP);
CREATE INDEX IDX_SolicitudesTurno_Municipio ON SolicitudesTurno(IdMunicipio);
