using System;
using System.Collections.Generic;
using ProyectoTicketTurno.Business.Models;
using ProyectoTicketTurno.Data.Repositories;

namespace ProyectoTicketTurno.Business.Services
{
    public interface ISolicitudTurnoService
    {
        SolicitudTurno ObtenerSolicitudPorNumeroTurno(int numeroTurno);
        SolicitudTurno ObtenerSolicitudPorCURP(string curp);
        IEnumerable<SolicitudTurno> ObtenerSolicitudesPorMunicipio(string municipio);
        IEnumerable<SolicitudTurno> ObtenerSolicitudesPorEstatus(EstatusEnum estatus);
        void CrearSolicitudTurno(SolicitudTurno solicitud);
        void ActualizarSolicitudTurno(SolicitudTurno solicitud);
        void CambiarEstatusSolicitud(int numeroTurno, EstatusEnum nuevoEstatus);
    }

    public class SolicitudTurnoService : ISolicitudTurnoService
    {
        private readonly ISolicitudTurnoRepository _solicitudTurnoRepository;
        private readonly ITurnoService _turnoService;
        private readonly Serilog.ILogger _logger;

        public SolicitudTurnoService(
            ISolicitudTurnoRepository solicitudTurnoRepository,
            ITurnoService turnoService,
            Serilog.ILogger logger)
        {
            _solicitudTurnoRepository = solicitudTurnoRepository;
            _turnoService = turnoService;
            _logger = logger;
        }

        public SolicitudTurno ObtenerSolicitudPorNumeroTurno(int numeroTurno)
        {
            try
            {
                return _solicitudTurnoRepository.ObtenerPorNumeroTurno(numeroTurno);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al obtener solicitud por número de turno: {NumeroTurno}", numeroTurno);
                throw;
            }
        }

        public SolicitudTurno ObtenerSolicitudPorCURP(string curp)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(curp))
                    throw new ArgumentException("El CURP no puede estar vacío");

                var solicitudes = _solicitudTurnoRepository.ObtenerPorCURP(curp);
                
                // Retornar la solicitud más reciente
                using (var enumerator = solicitudes.GetEnumerator())
                {
                    SolicitudTurno ultima = null;
                    while (enumerator.MoveNext())
                    {
                        ultima = enumerator.Current;
                    }
                    return ultima;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al obtener solicitud por CURP: {CURP}", curp);
                throw;
            }
        }

        public IEnumerable<SolicitudTurno> ObtenerSolicitudesPorMunicipio(string municipio)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(municipio))
                    throw new ArgumentException("El municipio no puede estar vacío");

                return _solicitudTurnoRepository.ObtenerPorMunicipio(municipio);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al obtener solicitudes por municipio: {Municipio}", municipio);
                throw;
            }
        }

        public IEnumerable<SolicitudTurno> ObtenerSolicitudesPorEstatus(EstatusEnum estatus)
        {
            try
            {
                return _solicitudTurnoRepository.ObtenerPorEstatus(estatus);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al obtener solicitudes por estatus: {Estatus}", estatus);
                throw;
            }
        }

        public void CrearSolicitudTurno(SolicitudTurno solicitud)
        {
            try
            {
                if (solicitud == null)
                    throw new ArgumentNullException(nameof(solicitud));

                // Asignar número de turno automáticamente
                solicitud.NumeroTurno = _turnoService.AsignarTurno(solicitud.Municipio);
                solicitud.FechaSolicitud = DateTime.Now;
                solicitud.Estatus = EstatusEnum.Pendiente;

                _solicitudTurnoRepository.Agregar(solicitud);
                _solicitudTurnoRepository.Guardar();
                _logger.Information("Solicitud de turno creada: {NumeroTurno} - CURP: {CURP}", solicitud.NumeroTurno, solicitud.CURP);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al crear solicitud de turno");
                throw;
            }
        }

        public void ActualizarSolicitudTurno(SolicitudTurno solicitud)
        {
            try
            {
                if (solicitud == null)
                    throw new ArgumentNullException(nameof(solicitud));

                var solicitudExistente = _solicitudTurnoRepository.ObtenerPorNumeroTurno(solicitud.NumeroTurno);
                if (solicitudExistente == null)
                    throw new InvalidOperationException($"No existe solicitud con número de turno: {solicitud.NumeroTurno}");

                _solicitudTurnoRepository.Actualizar(solicitud);
                _solicitudTurnoRepository.Guardar();
                _logger.Information("Solicitud de turno actualizada: {NumeroTurno}", solicitud.NumeroTurno);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al actualizar solicitud de turno");
                throw;
            }
        }

        public void CambiarEstatusSolicitud(int numeroTurno, EstatusEnum nuevoEstatus)
        {
            try
            {
                var solicitud = _solicitudTurnoRepository.ObtenerPorNumeroTurno(numeroTurno);
                if (solicitud == null)
                    throw new InvalidOperationException($"No existe solicitud con número de turno: {numeroTurno}");

                solicitud.Estatus = nuevoEstatus;
                _solicitudTurnoRepository.Actualizar(solicitud);
                _solicitudTurnoRepository.Guardar();
                _logger.Information("Estatus de solicitud actualizado: {NumeroTurno} -> {NuevoEstatus}", numeroTurno, nuevoEstatus);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al cambiar estatus de solicitud");
                throw;
            }
        }
    }
}