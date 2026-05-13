using System;
using System.Collections.Generic;

namespace ProyectoTicketTurno.Business.Models
{
    /// <summary>
    /// Modelo para almacenar estadísticas generales del dashboard
    /// </summary>
    public class DashboardEstadisticasGenerales
    {
        public int Total { get; set; }
        public int SolicitudesPendientes { get; set; }
        public int SolicitudesResueltas { get; set; }
        public decimal PorcentajePendientes { get; set; }
        public decimal PorcentajeResueltas { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }

    /// <summary>
    /// Modelo para datos del gráfico de torta (estado de solicitudes)
    /// </summary>
    public class DashboardEstadoData
    {
        public string Estado { get; set; }
        public int Cantidad { get; set; }
        public decimal Porcentaje { get; set; }
    }

    /// <summary>
    /// Modelo para datos del gráfico de barras (por municipio)
    /// </summary>
    public class DashboardMunicipioData
    {
        public string NombreMunicipio { get; set; }
        public int Pendientes { get; set; }
        public int Resueltos { get; set; }
        public int Total { get; set; }
    }

    /// <summary>
    /// Modelo para datos del gráfico de línea (tendencia temporal)
    /// </summary>
    public class DashboardTendenciaData
    {
        public DateTime Fecha { get; set; }
        public int Pendientes { get; set; }
        public int Resueltos { get; set; }
        public int Total { get; set; }
    }

    /// <summary>
    /// Modelo para detalles de solicitudes en el grid
    /// </summary>
    public class DashboardSolicitudDetalle
    {
        public int NumeroTurno { get; set; }
        public string CURP { get; set; }
        public string Municipio { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string Asunto { get; set; }
        public string PersonaTramitera { get; set; }
        public string Estatus { get; set; }
    }
}
