using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using Serilog;
using ProyectoTicketTurno.Business.Models;
using ProyectoTicketTurno.Business.Services;
using ProyectoTicketTurno.Data.Repositories;

namespace ProyectoTicketTurno.Tests
{
    public class TurnoServiceTests
    {
        private readonly Mock<ISolicitudTurnoRepository> _mockRepository;
        private readonly Mock<ILogger> _mockLogger;
        private readonly TurnoService _service;

        public TurnoServiceTests()
        {
            _mockRepository = new Mock<ISolicitudTurnoRepository>();
            _mockLogger = new Mock<ILogger>();
            _service = new TurnoService(_mockRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public void ObtenerProximoTurno_ConMunicipioSinSolicitudes_DebeRetornar1()
        {
            // Arrange
            string municipio = "Saltillo";
            _mockRepository.Setup(r => r.ObtenerPorMunicipio(municipio))
                .Returns(new List<SolicitudTurno>());

            // Act
            int resultado = _service.ObtenerProximoTurno(municipio);

            // Assert
            Assert.Equal(1, resultado);
        }

        [Fact]
        public void ObtenerProximoTurno_ConSolicitudesExistentes_DebeRetornarSiguienteNumero()
        {
            // Arrange
            string municipio = "Saltillo";
            var solicitudes = new List<SolicitudTurno>
            {
                new SolicitudTurno { NumeroTurno = 1, Municipio = municipio },
                new SolicitudTurno { NumeroTurno = 2, Municipio = municipio },
                new SolicitudTurno { NumeroTurno = 3, Municipio = municipio }
            };
            _mockRepository.Setup(r => r.ObtenerPorMunicipio(municipio))
                .Returns(solicitudes);

            // Act
            int resultado = _service.ObtenerProximoTurno(municipio);

            // Assert
            Assert.Equal(4, resultado);
        }

        [Fact]
        public void AsignarTurno_ConMunicipioValido_DebeRetornarTurnoAsignado()
        {
            // Arrange
            string municipio = "Saltillo";
            _mockRepository.Setup(r => r.ObtenerPorMunicipio(municipio))
                .Returns(new List<SolicitudTurno>());

            // Act
            int resultado = _service.AsignarTurno(municipio);

            // Assert
            Assert.Equal(1, resultado);
        }
    }
}