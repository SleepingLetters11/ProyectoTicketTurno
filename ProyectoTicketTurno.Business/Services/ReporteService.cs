using System;
using Serilog;
using ProyectoTicketTurno.Business.Models;
using ProyectoTicketTurno.Data.Repositories;
using ProyectoTicketTurno.Infrastructure.Reports;

namespace ProyectoTicketTurno.Business.Services
{
    public interface IReporteService
    {
        void GenerarComprobanteTurno(int numeroTurno);
    }

    public class ReporteService : IReporteService
    {
        private readonly ISolicitudTurnoRepository _solicitudTurnoRepository;
        private readonly IEstudianteRepository _estudianteRepository;
        private readonly IReporteFactory _reporteFactory;
        private readonly ILogger _logger;

        public ReporteService(
            ISolicitudTurnoRepository solicitudTurnoRepository,
            IEstudianteRepository estudianteRepository,
            IReporteFactory reporteFactory,
            ILogger logger)
        {
            _solicitudTurnoRepository = solicitudTurnoRepository;
            _estudianteRepository = estudianteRepository;
            _reporteFactory = reporteFactory;
            _logger = logger;
        }

        public void GenerarComprobanteTurno(int numeroTurno)
        {
            try
            {
                var solicitud = _solicitudTurnoRepository.ObtenerPorNumeroTurno(numeroTurno);
                if (solicitud == null)
                    throw new InvalidOperationException($"No existe solicitud con número: {numeroTurno}");

                var estudiante = _estudianteRepository.ObtenerPorCURP(solicitud.CURP);
                if (estudiante == null)
                    throw new InvalidOperationException($"No existe estudiante con CURP: {solicitud.CURP}");

                var reporte = _reporteFactory.CrearReporte(TipoReporte.Comprobante);
                reporte.Generar(solicitud, estudiante);

                _logger.Information("Comprobante generado para turno: {NumeroTurno}", numeroTurno);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al generar comprobante para turno: {NumeroTurno}", numeroTurno);
                throw;
            }
        }
    }
}