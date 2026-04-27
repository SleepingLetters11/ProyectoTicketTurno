using System;
using ProyectoTicketTurno.Business.Models;

namespace ProyectoTicketTurno.Infrastructure.Utilities
{
    public static class ReportGenerator
    {
        public static string GenerarComprobanteTexto(SolicitudTurno solicitud, Estudiante estudiante, Municipio municipio)
        {
            if (solicitud == null || estudiante == null || municipio == null)
                return "Error: Datos insuficientes para generar comprobante.";

            string comprobante = $@"
================================================================================
                            COMPROBANTE DE TURNO
================================================================================
Fecha de Emisión: {DateTime.Now:dd/MM/yyyy HH:mm:ss}

Datos del Estudiante:
    Nombre Completo: {estudiante.Nombre} {estudiante.ApellidoPaterno} {estudiante.ApellidoMaterno}
    CURP: {estudiante.CURP}
    Fecha de Nacimiento: {estudiante.FechaNacimiento:dd/MM/yyyy}
    Edad: {estudiante.Edad} años
    Sexo: {(estudiante.Sexo == 'H' ? "Hombre" : "Mujer")}
    Teléfono de Contacto: {estudiante.TelefonoContacto}
    Nivel Educativo: {estudiante.NivelEducativo?.Nombre ?? "N/A"}
    Grado: {estudiante.Grado}

Datos de la Solicitud:
    Número de Turno: {solicitud.NumeroTurno}
    Municipio: {municipio.Nombre}
    Asunto a Tratar: {solicitud.Asunto}
    Persona que Realiza el Trámite: {solicitud.PersonaTramite}
    Parentesco: {solicitud.Parentesco}
    Fecha de Solicitud: {solicitud.FechaSolicitud:dd/MM/yyyy HH:mm:ss}
    Estatus: {solicitud.Estatus}

================================================================================
IMPORTANTE: Conserve este comprobante para futuras consultas.
Verificación de turno disponible en la ventanilla del municipio indicado.
================================================================================
";
            return comprobante;
        }

        public static string GenerarEstadisticasTexto(int pendientes, int resueltos)
        {
            int total = pendientes + resueltos;
            double porcentajePendientes = total > 0 ? (pendientes * 100.0 / total) : 0;
            double porcentajeResueltos = total > 0 ? (resueltos * 100.0 / total) : 0;

            string estadisticas = $@"
================================================================================
                        ESTADÍSTICAS DE SOLICITUDES
================================================================================
Total de Solicitudes: {total}

Pendientes:     {pendientes} ({porcentajePendientes:F2}%)
Resueltos:      {resueltos} ({porcentajeResueltos:F2}%)

Generado: {DateTime.Now:dd/MM/yyyy HH:mm:ss}
================================================================================
";
            return estadisticas;
        }
    }
}
