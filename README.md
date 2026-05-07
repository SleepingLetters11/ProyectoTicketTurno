# Proyecto Ticket de Turno - Fase 1: Fundación

## Descripción
Aplicación de escritorio desarrollada en C# con .NET Framework 4.8.1 para gestión de turnos escolares en el estado de Coahuila.

**Fase Actual:** Fundación (Estructura Base + Entity Framework 6 + Repositorio Genérico)

## Especificaciones Técnicas
- **Lenguaje:** C# con .NET Framework 4.8.1
- **ORM:** Entity Framework 6.4.4
- **Base de Datos:** MS SQL Server 2019
- **Arquitectura:** MVC + Repository Pattern
- **UI:** WinForms

## Estructura del Proyecto
- `ProyectoTicketTurno.Data/` - Capa de Acceso a Datos (EF6 + Repositories)
- `ProyectoTicketTurno.Business/` - Lógica de Negocio (Models + Services)
- `ProyectoTicketTurno.Infrastructure/` - Utilidades (Logging, Helpers)
- `ProyectoTicketTurno.Presentation/` - Interfaz de Usuario (WinForms)
- `ProyectoTicketTurno.Tests/` - Pruebas Unitarias e Integración

## Instalación y Configuración

### 1. Requisitos Previos
- Visual Studio 2019 o superior
- .NET Framework 4.8.1 SDK (Target Framework)
- SQL Server 2019 Developer Edition

### 2. Restaurar Dependencias
```bash
nuget restore ProyectoTicketTurno.sln
```

### 3. Configurar Connection String
Editar `ProyectoTicketTurno.Data\App.config`:
```xml
<add name="AplicacionDbContext" 
     connectionString="Server=YOUR_SERVER;Database=TicketTurnoDb;Integrated Security=true;" 
     providerName="System.Data.SqlClient" />
```

### 4. Ejecutar Migraciones
```powershell
Update-Database -Project ProyectoTicketTurno.Data -Verbose
```

### 5. Crear Base de Datos
Ejecutar script SQL ubicado en `Database/Ticket_Turno_tablas.sql`

## Plan de Desarrollo
- **Fase 1:** Fundación
- **Fase 2:** Lógica de Negocio
- **Fase 3:** Interfaz de Usuario
- **Fase 4:** Dashboard y Finalización

## Estado del Proyecto
🟢 Fase 4: Dashboard y Finalización
