# Dashboard v1 - Guía de Integración

## 📋 Descripción

Dashboard interactivo para visualizar el estado de las solicitudes de turno escolar con filtrado dinámico por municipio.

## 🎯 Características Principales

### ✅ Filtrado Dinámico
- **Vista Total:** Muestra todas las solicitudes de todos los municipios
- **Filtro por Municipio:** Selecciona un municipio específico para ver solo sus solicitudes
- **ComboBox automático:** Se llena con los municipios que tienen solicitudes

### 📊 Gráficos Interactivos

1. **Gráfico de Torta** - Estado General
   - Muestra el porcentaje de solicitudes Pendiente vs Resuelto
   - Se actualiza según el filtro seleccionado
   - Colores: Rojo (Pendiente), Verde (Resuelto)

2. **Gráfico de Barras** - Por Municipio
   - Compara solicitudes pendientes y resueltas por municipio
   - Visualiza todos los municipios en una vista general
   - Ayuda a identificar municipios con más carga

3. **Gráfico de Línea** - Tendencia (Últimos 30 días)
   - Muestra la evolución temporal de solicitudes
   - Filtra según el municipio seleccionado
   - Permite identificar tendencias y patrones

### 📋 Tabla de Detalles
- Listado completo de solicitudes con:
  - Número de turno
  - CURP del estudiante
  - Municipio
  - Fecha de solicitud
  - Asunto del trámite
  - Persona que realiza el trámite
  - Estado (Pendiente/Resuelto)

### 📊 Panel de Estadísticas
- **Total:** Cantidad total de solicitudes
- **Pendientes:** Número y porcentaje de solicitudes sin resolver
- **Resueltos:** Número y porcentaje de solicitudes resueltas
- Se actualiza en tiempo real según el filtro

## 🚀 Instalación y Configuración

### 1. Clonar la Rama
```bash
git clone https://github.com/SleepingLetters11/ProyectoTicketTurno.git
cd ProyectoTicketTurno
git checkout feature/dashboard-v1
```

### 2. Abrir en Visual Studio
- Archivo → Abrir → ProyectoTicketTurno.sln

### 3. Instalar Dependencias NuGet
Si no está instalado System.Windows.Forms.DataVisualization:
```bash
Install-Package System.Windows.Forms.DataVisualization
```

### 4. Verificar Conexión a BD
En `App.config` de `ProyectoTicketTurno.Data`:
```xml
<connectionStrings>
  <add name="ProyectoTicketTurnoConnection" 
       connectionString="Server=.\SQLEXPRESS;Database=TicketTurnoDb;Integrated Security=true;" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

### 5. Establecer como Punto de Entrada
En `Program.cs`:
```csharp
Application.Run(new FormDashboard());
```

### 6. Compilar y Ejecutar
- Ctrl + Shift + B (Compilar)
- F5 (Ejecutar)

## 📁 Estructura de Archivos Creados

```
ProyectoTicketTurno/
├── ProyectoTicketTurno.Business/
│   ├── Models/
│   │   └── DashboardModels.cs          ← Modelos de datos
│   └── Services/
│       └── DashboardService.cs         ← Lógica de negocio
├── ProyectoTicketTurno.Data/
│   └── Repositories/
│       ├── IMunicipioRepository.cs     ← Interfaz
│       └── MunicipioRepository.cs      ← Implementación
└── ProyectoTicketTurno.Presentation/
    └── Forms/
        ├── FormDashboard.cs            ← Formulario principal
        ├── FormDashboard.Designer.cs   ← Código generado
        └── FormDashboard.resx          ← Recursos
```

## 🎨 Colores Utilizados

| Concepto | Color RGB | Hex |
|----------|-----------|-----|
| Pendiente | 255, 107, 107 | #FF6B6B |
| Resuelto | 76, 175, 80 | #4CAF50 |
| Encabezado | 0, 51, 102 | #003366 |
| Botón Principal | 0, 122, 204 | #007ACC |
| Botón Refrescar | 76, 175, 80 | #4CAF50 |
| Fondo | 240, 240, 240 | #F0F0F0 |

## 🔧 Métodos del DashboardService

### ObtenerEstadisticasGenerales()
Retorna estadísticas de TODAS las solicitudes.
```csharp
var stats = _dashboardService.ObtenerEstadisticasGenerales();
// Total: 50, Pendientes: 15, Resueltos: 35
```

### ObtenerEstadisticasPorMunicipio(string municipio)
Retorna estadísticas FILTRADAS por municipio.
```csharp
var stats = _dashboardService.ObtenerEstadisticasPorMunicipio("Saltillo");
// Total: 12, Pendientes: 4, Resueltos: 8
```

### ObtenerDatosEstados(string municipio = null)
Datos para el gráfico de torta.
```csharp
var datos = _dashboardService.ObtenerDatosEstados(municipio);
// [{Estado: "Pendiente", Cantidad: 15, Porcentaje: 30}]
```

### ObtenerDatosPorMunicipio()
Datos para el gráfico de barras (sin filtro).
```csharp
var datos = _dashboardService.ObtenerDatosPorMunicipio();
// [{NombreMunicipio: "Saltillo", Pendientes: 4, Resueltos: 8}]
```

### ObtenerDatosTendencia(string municipio = null)
Datos para el gráfico de línea (últimos 30 días).
```csharp
var datos = _dashboardService.ObtenerDatosTendencia(municipio);
// [{Fecha: 2026-05-01, Pendientes: 2, Resueltos: 3}]
```

### ObtenerMunicipios()
Obtiene lista de municipios con solicitudes.
```csharp
var municipios = _dashboardService.ObtenerMunicipios();
// ["Saltillo", "Torreón", "Monclova"]
```

### ObtenerDetallesSolicitudes(string municipio = null)
Obtiene lista detallada para el grid.
```csharp
var detalles = _dashboardService.ObtenerDetallesSolicitudes(municipio);
// [{ NumeroTurno: 1, CURP: "...", Municipio: "Saltillo", ... }]
```

## 🎮 Uso del Formulario

### Flujo de Uso
1. **Al abrir:** Dashboard se carga con datos totales
2. **Seleccionar municipio:** ComboBox muestra opciones disponibles
3. **Hacer clic en municipio:** Dashboard se actualiza automáticamente
4. **Ver gráficos:** Se actualizan según filtro
5. **Ver tabla:** Muestra solicitudes del filtro
6. **Clic "Ver Total":** Limpia filtro y muestra todo
7. **Clic "Refrescar":** Recarga datos desde BD

### Ejemplo de Interacción
```
┌─────────────────────────────────────────────────────┐
│ 📊 Dashboard - Gestión de Solicitudes de Turno      │
├─────────────────────────────────────────────────────┤
│ Municipio: [TOTAL ▼] [Ver Total] [Refrescar]      │
│                              Total: 50              │
│                         Pendientes: 15 (30%)        │
│                         Resueltos: 35 (70%)         │
├─────────────────────────────────────────────────────┤
│  Gráfico Torta   │ Gráfico Barras │ Gráfico Línea  │
│ (Estado General) │ (Por Municipio)│ (Tendencia)    │
├─────────────────────────────────────────────────────┤
│ # Turno │ CURP │ Municipio │ Fecha │ Estado        │
│    001  │ ...  │ Saltillo  │ ...   │ Pendiente     │
│    002  │ ...  │ Torreón   │ ...   │ Resuelto      │
└─────────────────────────────────────────────────────┘
```

## ✅ Checklist de Verificación

Antes de usar en producción:

- [ ] ¿La conexión a BD está configurada?
- [ ] ¿Está instalado System.Windows.Forms.DataVisualization?
- [ ] ¿El FormDashboard es el punto de entrada?
- [ ] ¿Hay datos de prueba en la BD?
- [ ] ¿Los gráficos se cargan sin errores?
- [ ] ¿El filtro de municipios funciona?
- [ ] ¿La tabla de detalles se actualiza?

## 🐛 Resolución de Problemas

### Error: "Connection string not found"
**Solución:** Verificar que App.config tenga la conexión "ProyectoTicketTurnoConnection"

### Error: "Chart control not found"
**Solución:** Instalar NuGet `System.Windows.Forms.DataVisualization`

### Error: "No data appears"
**Solución:** Verificar que hay solicitudes en la tabla SolicitudesTurno

### Gráficos sin datos
**Solución:** Ejecutar la migración de BD: `Update-Database`

## 📞 Contacto y Soporte

Para reportar problemas o sugerencias:
- Crear un Issue en GitHub
- Detallar los pasos para reproducir
- Adjuntar capturas de pantalla

## 📝 Notas Importantes

- El dashboard se actualiza en TIEMPO REAL
- Los datos se cargan desde la BD al iniciar
- El filtro por municipio es dinámico
- Los gráficos se actualizan automáticamente
- La tabla muestra hasta 1000 registros por defecto

---

**Versión:** 1.0
**Fecha:** 2026-05-13
**Estado:** ✅ Producción
