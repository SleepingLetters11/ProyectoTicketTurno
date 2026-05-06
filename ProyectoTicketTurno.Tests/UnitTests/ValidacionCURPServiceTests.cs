using System;
using Xunit;
using ProyectoTicketTurno.Business.Models;
using ProyectoTicketTurno.Business.Services;

namespace ProyectoTicketTurno.Tests
{
    public class ValidacionCURPServiceTests
    {
        private readonly ValidacionCURPService _service;

        public ValidacionCURPServiceTests()
        {
            _service = new ValidacionCURPService();
        }

        [Fact]
        public void ValidarFormatoCURP_ConFormatoValido_DebeRetornarTrue()
        {
            // Arrange
            string curpValido = "GOAA811230HDFRNN09";

            // Act
            bool resultado = _service.ValidarFormatoCURP(curpValido);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        public void ValidarFormatoCURP_ConMenosDe18Caracteres_DebeRetornarFalse()
        {
            // Arrange
            string curpInvalido = "GOAA811230HDFRNN";

            // Act
            bool resultado = _service.ValidarFormatoCURP(curpInvalido);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public void ValidarFormatoCURP_ConCaracteresInvalidos_DebeRetornarFalse()
        {
            // Arrange
            string curpInvalido = "GOAA811230HDFRNN0@";

            // Act
            bool resultado = _service.ValidarFormatoCURP(curpInvalido);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public void GenerarCURP_ConDatosValidos_DebeGenerarCURPValido()
        {
            // Arrange
            var estudiante = new Estudiante
            {
                ApellidoPaterno = "González",
                ApellidoMaterno = "Alvarez",
                Nombre = "Ana",
                FechaNacimiento = new DateTime(1981, 12, 30),
                Sexo = 'M',
                EstadoNacimiento = "Coahuila"
            };

            // Act
            string curpGenerado = _service.GenerarCURP(estudiante);

            // Assert
            Assert.NotNull(curpGenerado);
            Assert.Equal(18, curpGenerado.Length);
            Assert.True(_service.ValidarFormatoCURP(curpGenerado));
            Assert.StartsWith("GOAA", curpGenerado);
        }

        [Fact]
        public void GenerarCURP_ConEstadoInvalido_DebeLanzarExcepcion()
        {
            // Arrange
            var estudiante = new Estudiante
            {
                ApellidoPaterno = "González",
                ApellidoMaterno = "Alvarez",
                Nombre = "Ana",
                FechaNacimiento = new DateTime(1981, 12, 30),
                Sexo = 'M',
                EstadoNacimiento = "EstadoInvalido"
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _service.GenerarCURP(estudiante));
        }
    }
}