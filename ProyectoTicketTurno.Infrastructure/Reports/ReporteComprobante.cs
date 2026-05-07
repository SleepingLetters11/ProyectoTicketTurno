using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Serilog;
using ProyectoTicketTurno.Business.Models;

namespace ProyectoTicketTurno.Infrastructure.Reports
{
    public class ReporteComprobante : IReporte
    {
        private readonly ILogger _logger;

        public ReporteComprobante(ILogger logger = null)
        {
            _logger = logger;
        }

        public void Generar(SolicitudTurno solicitud, Estudiante estudiante)
        {
            try
            {
                string rutaComprobantes = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Comprobantes");

                if (!Directory.Exists(rutaComprobantes))
                    Directory.CreateDirectory(rutaComprobantes);

                string nombreArchivo = $"Comprobante_{solicitud.NumeroTurno}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                string rutaCompleta = Path.Combine(rutaComprobantes, nombreArchivo);

                // Crear documento PDF
                Document documento = new Document(PageSize.LETTER);
                PdfWriter.GetInstance(documento, new FileStream(rutaCompleta, FileMode.Create));
                documento.Open();

                // Encabezado
                var fuente = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                documento.Add(new Paragraph("COMPROBANTE DE TURNO", fuente) { Alignment = Element.ALIGN_CENTER });
                documento.Add(new Paragraph(" "));

                var fuenteNormal = FontFactory.GetFont(FontFactory.HELVETICA, 11);
                var fuenteLabel = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11);

                // Número de turno destacado
                documento.Add(new Paragraph($"NÚMERO DE TURNO: {solicitud.NumeroTurno}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14)) { Alignment = Element.ALIGN_CENTER });
                documento.Add(new Paragraph(" "));

                // Línea divisora
                documento.Add(new Paragraph("_________________________________"));
                documento.Add(new Paragraph(" "));

                // Datos del estudiante
                documento.Add(new Paragraph("DATOS DEL ESTUDIANTE", fuenteLabel));
                documento.Add(new Paragraph($"Nombre Completo: {estudiante.Nombre} {estudiante.ApellidoPaterno} {estudiante.ApellidoMaterno}", fuenteNormal));
                documento.Add(new Paragraph($"CURP: {estudiante.CURP}", fuenteNormal));
                documento.Add(new Paragraph($"Fecha de Nacimiento: {estudiante.FechaNacimiento:dd/MM/yyyy}", fuenteNormal));
                documento.Add(new Paragraph($"Edad: {estudiante.Edad} años", fuenteNormal));
                documento.Add(new Paragraph(" "));

                // Datos de la solicitud
                documento.Add(new Paragraph("DATOS DE LA SOLICITUD", fuenteLabel));
                documento.Add(new Paragraph($"Municipio: {solicitud.Municipio}", fuenteNormal));
                documento.Add(new Paragraph($"Asunto: {solicitud.Asunto}", fuenteNormal));
                documento.Add(new Paragraph($"Fecha de Solicitud: {solicitud.FechaSolicitud:dd/MM/yyyy HH:mm}", fuenteNormal));
                documento.Add(new Paragraph($"Estado: {solicitud.Estatus}", fuenteNormal));
                documento.Add(new Paragraph(" "));

                // Datos de la persona que realiza el trámite
                documento.Add(new Paragraph("DATOS DE LA PERSONA QUE REALIZA EL TRÁMITE", fuenteLabel));
                documento.Add(new Paragraph($"Nombre: {solicitud.PersonaTramitera}", fuenteNormal));
                documento.Add(new Paragraph($"Parentesco: {solicitud.Parentesco}", fuenteNormal));
                documento.Add(new Paragraph(" "));

                // Pie de página
                documento.Add(new Paragraph("_________________________________"));
                documento.Add(new Paragraph(" "));
                documento.Add(new Paragraph($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm:ss}", fuenteNormal) { Alignment = Element.ALIGN_CENTER });
                documento.Add(new Paragraph("Presentar este comprobante en la cita", fuenteNormal) { Alignment = Element.ALIGN_CENTER });

                documento.Close();

                _logger?.Information("Comprobante generado exitosamente: {Archivo}", rutaCompleta);
            }
            catch (Exception ex)
            {
                _logger?.Error(ex, "Error al generar comprobante de turno");
                throw;
            }
        }
    }
}