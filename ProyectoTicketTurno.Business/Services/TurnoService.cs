using System;
using System.Collections.Generic;
using System.Linq;
using ProyectoTicketTurno.Business.Models;
using ProyectoTicketTurno.Data.Repositories;

namespace ProyectoTicketTurno.Business.Services
{
    public interface ITurnoService
    {
        int AsignarTurno(string municipio);
        int ObtenerProximoTurno(string municipio);
    }

    public class TurnoService : ITurnoService
    {
        private readonly ISolicitudTurnoRepository _solicitudTurnoRepository;
        private readonly Serilog.ILogger _logger;

        public TurnoService(ISolicitudTurnoRepository solicitudTurnoRepository, Serilog.ILogger logger)
        {
            _solicitudTurnoRepository = solicitudTurnoRepository;
            _logger = logger;
        }

        /// <summary>
        /// Asigna automáticamente el próximo número de turno para un municipio
        /// </summary>
        public int AsignarTurno(string municipio)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(municipio))
                    throw new ArgumentException("El municipio no puede estar vacío");

                int proximoTurno = ObtenerProximoTurno(municipio);
                _logger.Information("Turno asignado para municipio {Municipio}: {ProximoTurno}", municipio, proximoTurno);
                return proximoTurno;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al asignar turno para municipio: {Municipio}", municipio);
                throw;
            }
        }

        /// <summary>
        /// Obtiene el próximo número de turno a asignar (auto-incremento por municipio)
        /// </summary>
        public int ObtenerProximoTurno(string municipio)
        {
            try
            {
                var solicitudesPorMunicipio = _solicitudTurnoRepository.ObtenerPorMunicipio(municipio).ToList();
                
                if (solicitudesPorMunicipio.Count == 0)
                    return 1;

                return solicitudesPorMunicipio.Max(s => s.NumeroTurno) + 1;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al obtener próximo turno para municipio: {Municipio}", municipio);
                throw;
            }
        }
    }
}