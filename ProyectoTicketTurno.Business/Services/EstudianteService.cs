using System;
using System.Collections.Generic;
using ProyectoTicketTurno.Business.Models;
using ProyectoTicketTurno.Data.Repositories;

namespace ProyectoTicketTurno.Business.Services
{
    public interface IEstudianteService
    {
        Estudiante ObtenerEstudiantePorCURP(string curp);
        IEnumerable<Estudiante> ObtenerTodos();
        void CrearEstudiante(Estudiante estudiante);
        void ActualizarEstudiante(Estudiante estudiante);
        void EliminarEstudiante(string curp);
    }

    public class EstudianteService : IEstudianteService
    {
        private readonly IEstudianteRepository _estudianteRepository;
        private readonly Serilog.ILogger _logger;

        public EstudianteService(IEstudianteRepository estudianteRepository, Serilog.ILogger logger)
        {
            _estudianteRepository = estudianteRepository;
            _logger = logger;
        }

        public Estudiante ObtenerEstudiantePorCURP(string curp)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(curp))
                    throw new ArgumentException("El CURP no puede estar vacío");

                return _estudianteRepository.ObtenerPorCURP(curp);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al obtener estudiante por CURP: {CURP}", curp);
                throw;
            }
        }

        public IEnumerable<Estudiante> ObtenerTodos()
        {
            try
            {
                return _estudianteRepository.ObtenerTodos();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al obtener todos los estudiantes");
                throw;
            }
        }

        public void CrearEstudiante(Estudiante estudiante)
        {
            try
            {
                if (estudiante == null)
                    throw new ArgumentNullException(nameof(estudiante));

                // Validar que no exista un estudiante con el mismo CURP
                var estudianteExistente = _estudianteRepository.ObtenerPorCURP(estudiante.CURP);
                if (estudianteExistente != null)
                    throw new InvalidOperationException($"Ya existe un estudiante con el CURP: {estudiante.CURP}");

                _estudianteRepository.Agregar(estudiante);
                _estudianteRepository.Guardar();
                _logger.Information("Estudiante creado exitosamente: {CURP}", estudiante.CURP);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al crear estudiante");
                throw;
            }
        }

        public void ActualizarEstudiante(Estudiante estudiante)
        {
            try
            {
                if (estudiante == null)
                    throw new ArgumentNullException(nameof(estudiante));

                var estudianteExistente = _estudianteRepository.ObtenerPorCURP(estudiante.CURP);
                if (estudianteExistente == null)
                    throw new InvalidOperationException($"No existe estudiante con CURP: {estudiante.CURP}");

                _estudianteRepository.Actualizar(estudiante);
                _estudianteRepository.Guardar();
                _logger.Information("Estudiante actualizado exitosamente: {CURP}", estudiante.CURP);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al actualizar estudiante");
                throw;
            }
        }

        public void EliminarEstudiante(string curp)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(curp))
                    throw new ArgumentException("El CURP no puede estar vacío");

                var estudiante = _estudianteRepository.ObtenerPorCURP(curp);
                if (estudiante == null)
                    throw new InvalidOperationException($"No existe estudiante con CURP: {curp}");

                _estudianteRepository.Eliminar(estudiante);
                _estudianteRepository.Guardar();
                _logger.Information("Estudiante eliminado exitosamente: {CURP}", curp);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al eliminar estudiante");
                throw;
            }
        }
    }
}