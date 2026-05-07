using System;
using System.Collections.Generic;
using System.Linq;
using ProyectoTicketTurno.Business.Models;
using ProyectoTicketTurno.Data.Repositories;

namespace ProyectoTicketTurno.Business.Services
{
    public interface IDashboardService
    {
        DashboardData ObtenerDatosGenerales();
        DashboardData ObtenerDatosPorMunicipio(string municipio);
    }

    public class DashboardData
    {
        public int SolicitudesPendientes { get; set; }
        public int SolicitudesResueltas { get; set; }
        public decimal PorcentajePendientes { get; set; }
        public decimal PorcentajeResueltas { get; set; }
    }

    public class DashboardService : IDashboardService
    {
        private readonly ISolicitudTurnoRepository _solicitudTurnoRepository;
        private readonly Serilog.ILogger _logger;

        public DashboardService(ISolicitudTurnoRepository solicitudTurnoRepository, Serilog.ILogger logger)
        {
            _solicitudTurnoRepository = solicitudTurnoRepository;
            _logger = logger;
        }

        public DashboardData ObtenerDatosGenerales()
        {
            try
            {
                var todasLasSolicitudes = _solicitudTurnoRepository.ObtenerTodos().ToList();
                return CalcularDatos(todasLasSolicitudes);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al obtener datos generales del dashboard");
                throw;
            }
        }

        public DashboardData ObtenerDatosPorMunicipio(string municipio)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(municipio))
                    throw new ArgumentException("El municipio no puede estar vacío");

                var solicitudesPorMunicipio = _solicitudTurnoRepository.ObtenerPorMunicipio(municipio).ToList();
                return CalcularDatos(solicitudesPorMunicipio);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al obtener datos del dashboard para municipio: {Municipio}", municipio);
                throw;
            }
        }

        private DashboardData CalcularDatos(List<SolicitudTurno> solicitudes)
        {
            if (solicitudes.Count == 0)
            {
                return new DashboardData
                {
                    SolicitudesPendientes = 0,
                    SolicitudesResueltas = 0,
                    PorcentajePendientes = 0,
                    PorcentajeResueltas = 0
                };
            }

            int pendientes = solicitudes.Count(s => s.Estatus == EstatusEnum.Pendiente);
            int resueltas = solicitudes.Count(s => s.Estatus == EstatusEnum.Resuelto);
            int total = solicitudes.Count;

            return new DashboardData
            {
                SolicitudesPendientes = pendientes,
                SolicitudesResueltas = resueltas,
                PorcentajePendientes = Math.Round((decimal)pendientes / total * 100, 2),
                PorcentajeResueltas = Math.Round((decimal)resueltas / total * 100, 2)
            };
        }
    }
}