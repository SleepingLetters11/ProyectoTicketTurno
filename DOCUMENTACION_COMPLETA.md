# 📋 Documentación Completa - Proyecto Ticket de Turno

**Última Actualización:** Mayo 2026  
**Fase Actual:** Fase 4 - Dashboard y Finalización  
**Estado:** 🟢 En Desarrollo

---

## 📑 Tabla de Contenidos

1. [Visión General](#visión-general)
2. [Especificaciones Técnicas](#especificaciones-técnicas)
3. [Arquitectura del Sistema](#arquitectura-del-sistema)
4. [Estructura de Carpetas](#estructura-de-carpetas)
5. [Modelos de Datos](#modelos-de-datos)
6. [Flujos de Negocio](#flujos-de-negocio)
7. [Componentes Principales](#componentes-principales)
8. [Dashboard y Reportes](#dashboard-y-reportes)
9. [API de Servicios](#api-de-servicios)
10. [Guía de Instalación](#guía-de-instalación)
11. [Pruebas](#pruebas)

---

## 🎯 Visión General

**ProyectoTicketTurno** es una aplicación de escritorio desarrollada en C# con .NET Framework para la **gestión de solicitudes y asignación de turnos escolares en el estado de Coahuila**.

### Objetivos Principales:
- ✅ Gestión centralizada de estudiantes y sus datos
- ✅ Asignación automática de turnos por municipio
- ✅ Seguimiento de solicitudes (Pendiente/Resuelto)
- ✅ Dashboard con análisis y reportes en tiempo real
- ✅ Validación de datos (CURP, información personal)
- ✅ Exportación de datos a reportes

### Beneficiarios:
- **Administradores:** Gestión completa del sistema
- **Operadores:** Solicitud y consulta de turnos
- **Estudiantes:** Consulta de estado de solicitudes

---

## 🛠️ Especificaciones Técnicas

| Aspecto | Detalle |
|--------|--------|
| **Lenguaje** | C# (.NET Framework 4.8.1) |
| **ORM** | Entity Framework 6.4.4 |
| **Base de Datos** | MS SQL Server 2019 |
| **Arquitectura** | MVC + Repository Pattern |
| **UI** | WinForms |
| **Logging** | Serilog |
| **Reportes** | OxyPlot (Gráficas) |
| **Testing** | NUnit / xUnit |

### Stack Tecnológico Completo:
Frontend (Presentación) ↓ Business Logic (Servicios) ↓ Data Access (Repositories) ↓ Database (SQL Server)

---

## 🏗️ Arquitectura del Sistema

### Patrón de Arquitectura: **3-Layered Architecture + Repository Pattern**

```
┌─────────────────────────────────────────────────────┐
│ CAPA DE PRESENTACIÓN (WinForms)                     │
│ Forms: Login, Principal, Solicitud, Admin, etc      │
└──────────────────┬──────────────────────────────────┘
                   │
┌──────────────────▼──────────────────────────────────┐
│ CAPA DE LÓGICA DE NEGOCIO (Services)               │
│ DashboardService, SolicitudTurnoService, etc        │
└──────────────────┬──────────────────────────────────┘
                   │
┌──────────────────▼──────────────────────────────────┐
│ CAPA DE ACCESO A DATOS (Repository Pattern)        │
│ Repositories + Entity Framework 6                   │
└──────────────────┬──────────────────────────────────┘
                   │
┌──────────────────▼──────────────────────────────────┐
│ BASE DE DATOS (SQL Server 2019)                    │
│ Tablas: Estudiantes, SolicitudesTurno, etc         │
└─────────────────────────────────────────────────────┘
```

### Principios Arquitectónicos:
- **Separación de Responsabilidades:** Cada capa tiene una función específica
- **DIP (Dependency Inversion Principle):** Uso de interfaces
- **Single Responsibility:** Cada servicio/repositorio hace una cosa bien
- **Open/Closed Principle:** Extensible sin modificar código existente

---

## 📁 Estructura de Carpetas

```
ProyectoTicketTurno/
├── ProyectoTicketTurno.Data/
│   ├── Context/
│   │   └── AplicacionDbContext.cs ← DbContext principal
│   ├── Repositories/
│   │   ├── Repository.cs ← Repositorio genérico
│   │   ├── IRepository.cs ← Interfaz base
│   │   ├── EstudianteRepository.cs ← Repos. específico
│   │   ├── SolicitudTurnoRepository.cs ← Repos. específico
│   │   └── MunicipioRepository.cs ← Repos. específico
│   ├── Migrations/
│   │   ├── Configuration.cs ← Seed data
│   │   └── [Migrations auto-generadas]
│   └── App.config ← Connection String
│
├── ProyectoTicketTurno.Business/
│   ├── Models/
│   │   ├── Estudiante.cs ← Entidad
│   │   ├── SolicitudTurno.cs ← Entidad
│   │   ├── Municipio.cs ← Entidad
│   │   └── Estado.cs ← Entidad
│   ├── Services/
│   │   ├── EstudianteService.cs
│   │   ├── SolicitudTurnoService.cs
│   │   ├── TurnoService.cs ← Lógica de auto-incremento
│   │   ├── DashboardService.cs ← Cálculos de métricas
│   │   ├── ReporteService.cs ← Generación de reportes
│   │   └── ValidacionCURPService.cs ← Validaciones
│   └── DTOs/
│       └── [Objetos de transferencia de datos]
│
├── ProyectoTicketTurno.Infrastructure/
│   ├── Logging/
│   │   └── LoggerSetup.cs ← Configuración Serilog
│   ├── Helpers/
│   │   ├── ValidationHelper.cs
│   │   └── CURPValidator.cs ← Validación CURP
│   └── Reports/
│       └── ReporteFactory.cs ← Factory para reportes
│
├── ProyectoTicketTurno.Presentation/
│   ├── Forms/
│   │   ├── FormLogin.cs ← Login
│   │   ├── FormPrincipal.cs ← Menú principal
│   │   ├── FormSolicitudTurno.cs ← Crear solicitud
│   │   ├── FormConsultaEstudiante.cs ← Consultar estudiante
│   │   ├── FormAdministrador.cs ← Panel admin
│   │   ├── FormDashboard.cs ← Dashboard simple
│   │   └── FormDashboardAvanzado.cs ← Dashboard con gráficos
│   ├── Program.cs ← Entry point
│   └── App.config ← Configuración UI
│
├── ProyectoTicketTurno.Tests/
│   ├── UnitTests/
│   │   ├── Services/
│   │   └── Repositories/
│   ├── IntegrationTests/
│   └── TestData/
│
├── Database/
│   └── Ticket_Turno_tablas.sql ← Script SQL inicial
│
├── ProyectoTicketTurno.sln ← Solución
├── README.md ← Resumen rápido
└── DOCUMENTACION_COMPLETA.md ← Este archivo
```

---

## 🗄️ Modelos de Datos

### 1. **Estudiante** (Primaria: CURP)

```csharp
public class Estudiante
{
    public string CURP { get; set; }              // Clave Única de Registro de Población
    public string Nombre { get; set; }
    public string ApellidoPaterno { get; set; }
    public string ApellidoMaterno { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public char Sexo { get; set; }                // H/M
    public int Edad { get; set; }
    public string EstadoNacimiento { get; set; }
    public string MunicipioEstudio { get; set; }
    public string TelefonoContacto { get; set; }
    public string NivelEducativo { get; set; }    // Primaria/Secundaria/Preparatoria
    public int Grado { get; set; }                // 1-6
}
```

**Restricciones:**
- CURP: 18 caracteres máximo, requerido (PK)
- Nombre, Apellidos: 100 caracteres, requeridos
- Sexo: Requerido (H o M)

### 2. SolicitudTurno (Primaria: NumeroTurno)

```csharp
public class SolicitudTurno
{
    public int NumeroTurno { get; set; }         // Auto-incremento por municipio
    public string CURP { get; set; }             // FK → Estudiante
    public string Municipio { get; set; }
    public DateTime FechaSolicitud { get; set; }
    public string Asunto { get; set; }           // Razón del trámite
    public string PersonaTramitera { get; set; } // Quién realiza el trámite
    public string Parentesco { get; set; }       // Relación con estudiante
    public EstatusEnum Estatus { get; set; }     // Pendiente/Resuelto
}

public enum EstatusEnum
{
    Pendiente = 0,
    Resuelto = 1
}
```

**Características:**
- NumeroTurno se genera automáticamente con patrón: MUN-YYYYMMDD-0001
- Auto-incremento diferenciado por municipio
- Fechas registradas automáticamente

### 3. Municipio (Primaria: IdMunicipio)

```csharp
public class Municipio
{
    public int IdMunicipio { get; set; }
    public string Nombre { get; set; }           // Nombre del municipio
    public int ContadorTurnos { get; set; }      // Para auto-incremento
}
```

**Municipios de Coahuila:**
Saltillo, Torreón, Monclova, Parras, Matamoros, etc.

### 4. Estado (Primaria: Clave)

```csharp
public class Estado
{
    public string Clave { get; set; }            // Código INEGI
    public string Nombre { get; set; }           // Nombre estado
}
```

### Diagrama Entidad-Relación (ER)

```
┌─────────────────────┐         ┌──────────────────────┐
│   ESTUDIANTE        │         │  SOLICITUD_TURNO     │
├─────────────────────┤         ├──────────────────────┤
│ PK: CURP (18)       │◄────────│ PK: NumeroTurno (auto)
│ Nombre (100)        │    FK   │ FK: CURP (18)        │
│ ApellidoPaterno     │         │ Municipio (100)      │
│ ApellidoMaterno     │         │ FechaSolicitud       │
│ FechaNacimiento     │         │ Asunto (500)         │
│ Sexo (1)            │         │ PersonaTramitera     │
│ EstadoNacimiento    │         │ Parentesco           │
│ MunicipioEstudio    │         │ Estatus (enum)       │
│ TelefonoContacto    │         │                      │
│ NivelEducativo      │         │                      │
│ Grado               │         │                      │
└─────────────────────┘         └──────────────────────┘

┌──────────────────────┐         ┌──────────────────┐
│   MUNICIPIO          │         │   ESTADO         │
├──────────────────────┤         ├──────────────────┤
│ PK: IdMunicipio (auto)         │ PK: Clave (10)   │
│ Nombre (100)         │         │ Nombre (100)     │
│ ContadorTurnos       │         │                  │
└──────────────────────┘         └──────────────────┘
```

---

## 🔄 Flujos de Negocio

### Flujo 1: Solicitar un Turno

```
┌──────────────┐
│   Inicio     │
└──────┬───────┘
       │
       ▼
┌──────────────────────────────┐
│ Usuario abre FormSolicitudTurno
└──────┬───────────────────────┘
       │
       ▼
┌──────────────────────────────┐
│ Ingresa CURP del estudiante  │
└──────┬───────────────────────┘
       │
       ▼
┌──────────────────────────────┐      ┌──────────────┐
│ ValidacionCURPService        │─────▶│ ¿CURP válida?│
│ valida el formato            │      └──────┬───────┘
└──────────────────────────────┘             │
                                    ┌────────┴────────┐
                                    │ NO              │ SÍ
                                    ▼                 ▼
                            ┌──────────────┐  ┌──────────────────┐
                            │ Mostrar error│  │ Buscar estudiante│
                            └──────────────┘  │ en BD            │
                                              └────────┬─────────┘
                                                       │
                                    ┌──────────────────┼──────────────┐
                                    │ NO encontrado    │ Encontrado   │
                                    ▼                  ▼
                                  ┌───────┐    ┌─────────────────┐
                                  │Error  │    │ Mostrar datos   │
                                  └───────┘    │ del estudiante  │
                                               └────────┬────────┘
                                                        │
                                                        ▼
                                               ┌──────────────────┐
                                               │ Usuario completa │
                                               │ datos de trámite │
                                               └────────┬─────────┘
                                                        │
                                                        ▼
                                               ┌──────────────────┐
                                               │ TurnoService:    │
                                               │ AsignarTurno()   │
                                               │ (Auto-increment) │
                                               └────────┬─────────┘
                                                        │
                                                        ▼
                                               ┌──────────────────┐
                                               │ Guardar en BD    │
                                               │ SolicitudTurno   │
                                               └────────┬─────────┘
                                                        │
                                                        ▼
                                               ┌──────────────────┐
                                               │ Mostrar número   │
                                               │ de turno asignado│
                                               └────────┬─────────┘
                                                        │
                                                        ▼
                                               ┌──────────────────┐
                                               │     Fin          │
                                               └──────────────────┘
```

### Flujo 2: Consultar Estado de Turno

```
┌──────────────┐
│   Inicio     │
└──────┬───────┘
       │
       ▼
┌────────────────────────────┐
│ Usuario abre FormConsultaEstudiante
└──────┬────────────────────┘
       │
       ▼
┌────────────────────────────┐
│ Ingresa CURP               │
└──────┬────────────────────┘
       │
       ▼
┌────────────────────────────┐
│ EstudianteService:         │
│ ObtenerPorCURP()           │
└──────┬────────────────────┘
       │
       ▼
┌────────────────────────────┐      ┌──────────────┐
│ Buscar en BD               │─────▶│ ¿Encontrado? │
└────────────────────────────┘      └──────┬───────┘
                                  ┌────────┴────────┐
                                  │ NO              │ SÍ
                                  ▼                 ▼
                          ┌──────────────┐  ┌────────────────┐
                          │ Mostrar error│  │ SolicitudTurno:│
                          └──────────────┘  │ ObtenerPorCURP()
                                            └────────┬───────┘
                                                     │
                                                     ▼
                                            ┌────────────────┐
                                            │ Mostrar        │
                                            │ - Datos alumno │
                                            │ - Turno(s)     │
                                            │ - Estado       │
                                            │ - Fecha        │
                                            └────────┬───────┘
                                                     │
                                                     ▼
                                            ┌────────────────┐
                                            │     Fin        │
                                            └────────────────┘
```

### Flujo 3: Administración de Solicitudes

```
┌──────────────────────┐
│ FormAdministrador    │
└──────┬───────────────┘
       │
       ├─────────────────────────────────┐
       │                                 │
       ▼                                 ▼
  ┌─────────────┐              ┌──────────────────┐
  │ Buscar por  │              │ Ver todas las    │
  │ CURP        │              │ solicitudes      │
  └─────┬───────┘              └────────┬─────────┘
        │                               │
        ▼                               ▼
  ┌──────────────────┐         ┌──────────────────┐
  │ Mostrar datos    │         │ Listar en Grid   │
  │ y solicitudes    │         │ con filtros      │
  └─────┬────────────┘         └────────┬─────────┘
        │                               │
        ▼                               ▼
  ┌──────────────────┐         ┌──────────────────┐
  │ Permitir:        │         │ Permitir:        │
  │ • Editar datos   │         │ • Cambiar estado │
  │ • Cambiar estado │         │ • Exportar datos │
  │ • Eliminar       │         │ • Ver detalle    │
  └──────────────────┘         └──────────────────┘
```

---

## 🎯 Componentes Principales

### 1. Services (Lógica de Negocio)

#### EstudianteService

```csharp
public interface IEstudianteService
{
    Estudiante ObtenerPorCURP(string curp);
    IEnumerable<Estudiante> ObtenerTodos();
    void GuardarEstudiante(Estudiante estudiante);
    void ActualizarEstudiante(Estudiante estudiante);
}
```

#### SolicitudTurnoService

```csharp
public interface ISolicitudTurnoService
{
    SolicitudTurno ObtenerSolicitudPorNumeroTurno(int numeroTurno);
    IEnumerable<SolicitudTurno> ObtenerSolicitudesPorCURP(string curp);
    void GuardarSolicitud(SolicitudTurno solicitud);
    void ActualizarEstatus(int numeroTurno, EstatusEnum nuevoEstatus);
}
```

#### TurnoService

```csharp
public interface ITurnoService
{
    int AsignarTurno(string municipio);
    int ObtenerProximoTurno(string municipio);
}
```

Lógica: Genera turnos auto-incrementados por municipio sin repetir números.

#### DashboardService

```csharp
public interface IDashboardService
{
    DashboardData ObtenerDatosGenerales();
    DashboardData ObtenerDatosPorMunicipio(string municipio);
}
```

### 2. Repositories (Acceso a Datos)

#### Repository Genérico

```csharp
public class Repository<T> : IRepository<T> where T : class
{
    public virtual IEnumerable<T> ObtenerTodos();
    public virtual T ObtenerPorId(object id);
    public virtual IEnumerable<T> ObtenerPor(Expression<Func<T, bool>> predicado);
    public virtual void Agregar(T entidad);
    public virtual void Actualizar(T entidad);
    public virtual void Eliminar(T entidad);
    public virtual void Guardar();
}
```

#### Repositorios Específicos

- **EstudianteRepository:** CRUD de estudiantes
- **SolicitudTurnoRepository:** CRUD de solicitudes + métodos especializados
- **MunicipioRepository:** Gestión de municipios

### 3. Forms (Presentación)

| Form | Propósito | Acceso |
|------|-----------|--------|
| FormLogin | Autenticación de usuario | Pública |
| FormPrincipal | Menú principal con opciones | Post-Login |
| FormSolicitudTurno | Crear nueva solicitud | Post-Login |
| FormConsultaEstudiante | Consultar datos y turno | Post-Login |
| FormAdministrador | Gestión completa (admin only) | Admin |
| FormDashboard | Métricas básicas | Admin |
| FormDashboardAvanzado | Gráficos y análisis | Admin |

---

## 📊 Dashboard y Reportes

### Dashboard Básico - Métricas Principales

```
╔════════════════════════════════════════════════════╗
║          DASHBOARD - ESTADÍSTICAS DE TURNOS        ║
╠════════════════════════════════════════════════════╣
║                                                    ║
║  Solicitudes Pendientes: 145          [15.6%]      ║
║  Solicitudes Resueltas:  770          [84.4%]      ║
║                                                    ║
║  Total de Solicitudes: 915                         ║
║  Tasa de Resolución: 84.4%                         ║
║  Promedio de Espera: 12.3 días                     ║
║                                                    ║
╚════════════════════════════════════════════════════╝
```

### Dashboard Avanzado - Gráficos (OxyPlot)

**Visualizaciones:**
- Gráfico de Pastel: Pendiente vs Resuelto
- Gráfico de Barras: Solicitudes por municipio
- Gráfico de Líneas: Tendencia temporal
- Tabla Filtrable: Detalle por municipio

**Funcionalidades:**
- Filtrar por municipio
- Seleccionar rango de fechas
- Exportar a CSV
- Actualización en tiempo real

---

## 🔌 API de Servicios

### EstudianteService

```csharp
// Obtener estudiante por CURP
Estudiante estudiante = _estudianteService.ObtenerPorCURP("VELD...XXXX");

// Guardar nuevo estudiante
_estudianteService.GuardarEstudiante(nuevoEstudiante);

// Obtener todos
var todos = _estudianteService.ObtenerTodos();
```

### SolicitudTurnoService

```csharp
// Obtener solicitud por número de turno
var solicitud = _solicitudTurnoService.ObtenerSolicitudPorNumeroTurno(1001);

// Obtener todas las solicitudes de un estudiante
var solicitudes = _solicitudTurnoService.ObtenerSolicitudesPorCURP("CURP");

// Cambiar estado
_solicitudTurnoService.ActualizarEstatus(1001, EstatusEnum.Resuelto);
```

### DashboardService

```csharp
// Datos generales
DashboardData general = _dashboardService.ObtenerDatosGenerales();
Console.WriteLine($"Pendientes: {general.SolicitudesPendientes}");
Console.WriteLine($"% Pendientes: {general.PorcentajePendientes}%");

// Datos por municipio
DashboardData saltillo = _dashboardService.ObtenerDatosPorMunicipio("Saltillo");
```

---

## 📦 Guía de Instalación

### Requisitos Previos

- Visual Studio 2019 o superior
- .NET Framework 4.8.1 SDK
- SQL Server 2019 (con Management Studio)
- Git

### Paso 1: Clonar el Repositorio

```bash
git clone https://github.com/SleepingLetters11/ProyectoTicketTurno.git
cd ProyectoTicketTurno
```

### Paso 2: Restaurar Dependencias

```bash
nuget restore ProyectoTicketTurno.sln
```

### Paso 3: Configurar Connection String

**Archivo:** `ProyectoTicketTurno.Data\App.config`

```xml
<configuration>
  <connectionStrings>
    <add name="ProyectoTicketTurnoConnection" 
         connectionString="Server=TU_SERVIDOR;Database=TicketTurnoDb;Integrated Security=true;" 
         providerName="System.Data.SqlClient" />
  </connectionStrings>
</configuration>
```

### Paso 4: Ejecutar Migraciones

Abrir Package Manager Console en Visual Studio:

```powershell
# Establecer proyecto por defecto
Set-StartupProject ProyectoTicketTurno.Data

# Crear migración inicial (si no existe)
Add-Migration InitialCreate

# Aplicar migración
Update-Database -Verbose
```

### Paso 5: Compilar y Ejecutar

```bash
# Compilar
Build-Solution

# Ejecutar (F5 en Visual Studio)
```

---

## 🧪 Pruebas

### Estructura de Pruebas

```
ProyectoTicketTurno.Tests/
├── UnitTests/
│   ├── Services/
│   │   ├── ValidacionCURPServiceTests.cs
│   │   ├── TurnoServiceTests.cs
│   │   ├── DashboardServiceTests.cs
│   │   └── EstudianteServiceTests.cs
│   └── Repositories/
│       ├── RepositoryTests.cs
│       └── SolicitudTurnoRepositoryTests.cs
└── IntegrationTests/
    ├── StudentWorkflowTests.cs
    └── TurnoAssignmentTests.cs
```

### Ejecutar Pruebas

```bash
# Abrir Test Explorer (Ctrl+E, T)
# O ejecutar desde línea de comandos:

dotnet test ProyectoTicketTurno.Tests.csproj
```

### Ejemplo de Prueba Unitaria

```csharp
[TestClass]
public class TurnoServiceTests
{
    private ITurnoService _turnoService;
    private Mock<ISolicitudTurnoRepository> _mockRepository;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<ISolicitudTurnoRepository>();
        _turnoService = new TurnoService(_mockRepository.Object, new Mock<ILogger>().Object);
    }

    [TestMethod]
    public void AsignarTurno_DebeIncremntarPorMunicipio()
    {
        // Arrange
        var municipio = "Saltillo";

        // Act
        var turno1 = _turnoService.AsignarTurno(municipio);
        var turno2 = _turnoService.AsignarTurno(municipio);

        // Assert
        Assert.AreEqual(turno2, turno1 + 1);
    }
}
```

---

## 📈 Fases del Proyecto

| Fase | Estado | Descripción |
|------|--------|-------------|
| 1. Fundación | ✅ Completada | Estructura base, EF6, Repositories |
| 2. Lógica de Negocio | ✅ Completada | Services, validaciones, workflows |
| 3. Interfaz de Usuario | ✅ Completada | Forms WinForms, navegación |
| 4. Dashboard y Finalización | 🟢 En Progreso | Dashboards, reportes, testing final |

---

## 📝 Notas Importantes

### Configuración de Logging (Serilog)

```csharp
// En Program.cs
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/app-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
```

### Validación de CURP

```
Formato: 6 letras + 8 dígitos + 4 caracteres
Ejemplo: VELD890123HDFXXX00
```

### Auto-incremento de Turnos por Municipio

```
Patrón: MUN-YYYYMMDD-0001
Ejemplo: SAL-20260526-0001 (Saltillo, 26 mayo 2026, turno 1)
```

---

## 🔗 Enlaces Útiles

- [Microsoft Docs: Entity Framework 6](https://docs.microsoft.com/en-us/ef/ef6/)
- [Serilog Documentation](https://serilog.net/)
- [OxyPlot Charts](https://oxyplot.github.io/)
- [WinForms Best Practices](https://docs.microsoft.com/en-us/dotnet/desktop/winforms/)
