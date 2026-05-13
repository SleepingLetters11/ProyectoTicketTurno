using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;
using ProyectoTicketTurno.Business.Models;
using ProyectoTicketTurno.Data.Repositories;

namespace ProyectoTicketTurno.Business.Services
{
    /// <summary>
    /// Interfaz para servicios del dashboard
    /// </summary>
    public interface IDashboardService
    {
        /// <summary>
        /// Obtiene estadísticas generales de todas las solicitudes
        /// </summary>
        DashboardEstadisticasGenerales ObtenerEstadisticasGenerales();

        /// <summary>
        /// Obtiene estadísticas filtradas por municipio
        /// </summary>
        DashboardEstadisticasGenerales ObtenerEstadisticasPorMunicipio(string municipio);

        /// <summary>
        /// Obtiene los datos para el gráfico de torta (Pendiente/Resuelto)
        /// </summary>
        List<DashboardEstadoData> ObtenerDatosEstados(string municipio = null);

        /// <summary>
        /// Obtiene los datos para el gráfico de barras (por municipio)
        /// </summary>
        List<DashboardMunicipioData> ObtenerDatosPorMunicipio();

        /// <summary>
        /// Obtiene los datos para el gráfico de línea (tendencia últimos 30 días)
        /// </summary>
        List<DashboardTendenciaData> ObtenerDatosTendencia(string municipio = null);

        /// <summary>
        /// Obtiene la lista de municipios con solicitudes
        /// </summary>
        List<string> ObtenerMunicipios();

        /// <summary>
        /// Obtiene el listado detallado de solicitudes para el grid
        /// </summary>
        List<DashboardSolicitudDetalle> ObtenerDetallesSolicitudes(string municipio = null);
    }

    /// <summary>
    /// Implementación del servicio de dashboard
    /// </summary>
    public class DashboardService : IDashboardService
    {
        private readonly ISolicitudTurnoRepository _solicitudTurnoRepository;
        private readonly IMunicipioRepository _municipioRepository;
        private readonly ILogger _logger;

        public DashboardService(
            ISolicitudTurnoRepository solicitudTurnoRepository,
            IMunicipioRepository municipioRepository,
            ILogger logger)
        {
            _solicitudTurnoRepository = solicitudTurnoRepository;
            _municipioRepository = municipioRepository;
            _logger = logger;
        }

        public DashboardEstadisticasGenerales ObtenerEstadisticasGenerales()
        {
            try
            {
                var todasLasSolicitudes = _solicitudTurnoRepository.ObtenerTodos().ToList();
                return CalcularEstadisticas(todasLasSolicitudes);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al obtener estadísticas generales del dashboard");
                throw;
            }
        }

        public DashboardEstadisticasGenerales ObtenerEstadisticasPorMunicipio(string municipio)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(municipio))
                    throw new ArgumentException("El municipio no puede estar vacío");

                var solicitudesPorMunicipio = _solicitudTurnoRepository.ObtenerPorMunicipio(municipio).ToList();
                return CalcularEstadisticas(solicitudesPorMunicipio);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al obtener estadísticas por municipio: {Municipio}", municipio);
                throw;
            }
        }

        public List<DashboardEstadoData> ObtenerDatosEstados(string municipio = null)
        {
            try
            {
                List<SolicitudTurno> solicitudes;

                if (string.IsNullOrWhiteSpace(municipio))
                    solicitudes = _solicitudTurnoRepository.ObtenerTodos().ToList();
                else
                    solicitudes = _solicitudTurnoRepository.ObtenerPorMunicipio(municipio).ToList();

                if (solicitudes.Count == 0)
                    return new List<DashboardEstadoData>();

                int pendientes = solicitudes.Count(s => s.Estatus == EstatusEnum.Pendiente);
                int resueltas = solicitudes.Count(s => s.Estatus == EstatusEnum.Resuelto);
                int total = solicitudes.Count;

                var datos = new List<DashboardEstadoData>
                {
                    new DashboardEstadoData
                    {
                        Estado = "Pendiente",
                        Cantidad = pendientes,
                        Porcentaje = total > 0 ? Math.Round((decimal)pendientes / total * 100, 2) : 0
                    },
                    new DashboardEstadoData
                    {
                        Estado = "Resuelto",
                        Cantidad = resueltas,
                        Porcentaje = total > 0 ? Math.Round((decimal)resueltas / total * 100, 2) : 0
                    }
                };

                return datos;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al obtener datos de estados");
                throw;
            }
        }

        public List<DashboardMunicipioData> ObtenerDatosPorMunicipio()
        {
            try
            {
                var todasLasSolicitudes = _solicitudTurnoRepository.ObtenerTodos().ToList();

                var datosPorMunicipio = todasLasSolicitudes
                    .GroupBy(s => s.Municipio)
                    .Select(g => new DashboardMunicipioData
                    {
                        NombreMunicipio = g.Key,
                        Pendientes = g.Count(s => s.Estatus == EstatusEnum.Pendiente),
                        Resueltos = g.Count(s => s.Estatus == EstatusEnum.Resuelto),
                        Total = g.Count()
                    })
                    .OrderBy(x => x.NombreMunicipio)
                    .ToList();

                return datosPorMunicipio;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al obtener datos por municipio");
                throw;
            }
        }

        public List<DashboardTendenciaData> ObtenerDatosTendencia(string municipio = null)
        {
            try
            {
                List<SolicitudTurno> solicitudes;

                if (string.IsNullOrWhiteSpace(municipio))
                    solicitudes = _solicitudTurnoRepository.ObtenerTodos().ToList();
                else
                    solicitudes = _solicitudTurnoRepository.ObtenerPorMunicipio(municipio).ToList();

                // Últimos 30 días
                var hace30Dias = DateTime.Now.AddDays(-30);
                var solicitudesUltimos30Dias = solicitudes
                    .Where(s => s.FechaSolicitud >= hace30Dias)
                    .ToList();

                // Agrupar por fecha
                var datosTendencia = solicitudesUltimos30Dias
                    .GroupBy(s => s.FechaSolicitud.Date)
                    .Select(g => new DashboardTendenciaData
                    {
                        Fecha = g.Key,
                        Pendientes = g.Count(s => s.Estatus == EstatusEnum.Pendiente),
                        Resueltos = g.Count(s => s.Estatus == EstatusEnum.Resuelto),
                        Total = g.Count()
                    })
                    .OrderBy(x => x.Fecha)
                    .ToList();

                return datosTendencia;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al obtener datos de tendencia");
                throw;
            }
        }

        public List<string> ObtenerMunicipios()
        {
            try
            {
                var municipios = _solicitudTurnoRepository.ObtenerTodos()
                    .Select(s => s.Municipio)
                    .Distinct()
                    .OrderBy(m => m)
                    .ToList();

                return municipios;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al obtener lista de municipios");
                throw;
            }
        }

        public List<DashboardSolicitudDetalle> ObtenerDetallesSolicitudes(string municipio = null)
        {
            try
            {
                List<SolicitudTurno> solicitudes;

                if (string.IsNullOrWhiteSpace(municipio))
                    solicitudes = _solicitudTurnoRepository.ObtenerTodos().ToList();
                else
                    solicitudes = _solicitudTurnoRepository.ObtenerPorMunicipio(municipio).ToList();

                var detalles = solicitudes
                    .Select(s => new DashboardSolicitudDetalle
                    {
                        NumeroTurno = s.NumeroTurno,
                        CURP = s.CURP,
                        Municipio = s.Municipio,
                        FechaSolicitud = s.FechaSolicitud,
                        Asunto = s.Asunto,
                        PersonaTramitera = s.PersonaTramitera,
                        Estatus = s.Estatus == EstatusEnum.Pendiente ? "Pendiente" : "Resuelto"
                    })
                    .OrderByDescending(x => x.FechaSolicitud)
                    .ToList();

                return detalles;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al obtener detalles de solicitudes");
                throw;
            }
        }

        /// <summary>
        /// Método privado para calcular estadísticas
        /// </summary>
        private DashboardEstadisticasGenerales CalcularEstadisticas(List<SolicitudTurno> solicitudes)
        {
            if (solicitudes.Count == 0)
            {
                return new DashboardEstadisticasGenerales
                {
                    Total = 0,
                    SolicitudesPendientes = 0,
                    SolicitudesResueltas = 0,
                    PorcentajePendientes = 0,
                    PorcentajeResueltas = 0,
                    FechaActualizacion = DateTime.Now
                };
            }

            int pendientes = solicitudes.Count(s => s.Estatus == EstatusEnum.Pendiente);
            int resueltas = solicitudes.Count(s => s.Estatus == EstatusEnum.Resuelto);
            int total = solicitudes.Count;

            return new DashboardEstadisticasGenerales
            {
                Total = total,
                SolicitudesPendientes = pendientes,
                SolicitudesResueltas = resueltas,
                PorcentajePendientes = Math.Round((decimal)pendientes / total * 100, 2),
                PorcentajeResueltas = Math.Round((decimal)resueltas / total * 100, 2),
                FechaActualizacion = DateTime.Now
            };
        }
    }
}
