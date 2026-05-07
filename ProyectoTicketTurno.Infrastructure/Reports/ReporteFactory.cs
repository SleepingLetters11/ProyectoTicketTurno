using Serilog;
using ProyectoTicketTurno.Business.Models;

namespace ProyectoTicketTurno.Infrastructure.Reports
{
    public enum TipoReporte
    {
        Comprobante = 0
    }

    public interface IReporteFactory
    {
        IReporte CrearReporte(TipoReporte tipo);
    }

    public class ReporteFactory : IReporteFactory
    {
        private readonly ILogger _logger;

        public ReporteFactory(ILogger logger)
        {
            _logger = logger;
        }

        public IReporte CrearReporte(TipoReporte tipo)
        {
            return tipo switch
            {
                TipoReporte.Comprobante => new ReporteComprobante(_logger),
                _ => throw new System.ArgumentException($"Tipo de reporte no reconocido: {tipo}")
            };
        }
    }
}