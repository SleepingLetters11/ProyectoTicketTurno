using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ProyectoTicketTurno.Business.Models;

namespace ProyectoTicketTurno.Business.Services
{
    public interface IValidacionCURPService
    {
        bool ValidarFormatoCURP(string curp);
        string GenerarCURP(Estudiante estudiante);
        bool ValidarCURP(string curp, Estudiante estudiante);
    }

    public class ValidacionCURPService : IValidacionCURPService
    {
        private readonly Dictionary<string, string> _abreviaturasPorEstado = new Dictionary<string, string>
        {
            { "Aguascalientes", "AGS" },
            { "Baja California", "BC" },
            { "Baja California Sur", "BCS" },
            { "Campeche", "CAMP" },
            { "Coahuila", "COAH" },
            { "Colima", "COL" },
            { "Chiapas", "CHIS" },
            { "Chihuahua", "CHIH" },
            { "Ciudad de México", "CDMX" },
            { "Durango", "DGO" },
            { "Guanajuato", "GTO" },
            { "Guerrero", "GRO" },
            { "Hidalgo", "HGO" },
            { "Jalisco", "JAL" },
            { "México", "MEX" },
            { "Michoacán", "MICH" },
            { "Morelos", "MOR" },
            { "Nayarit", "NAY" },
            { "Nuevo León", "NL" },
            { "Oaxaca", "OAX" },
            { "Puebla", "PUE" },
            { "Querétaro", "QRO" },
            { "Quintana Roo", "QROO" },
            { "San Luis Potosí", "SLP" },
            { "Sinaloa", "SIN" },
            { "Sonora", "SON" },
            { "Tabasco", "TAB" },
            { "Tamaulipas", "TAMS" },
            { "Tlaxcala", "TLAX" },
            { "Veracruz", "VER" },
            { "Yucatán", "YUC" },
            { "Zacatecas", "ZAC" }
        };

        /// <summary>
        /// Valida que el formato de la CURP sea correcto (18 caracteres alfanuméricos)
        /// </summary>
        public bool ValidarFormatoCURP(string curp)
        {
            if (string.IsNullOrWhiteSpace(curp))
                return false;

            // CURP debe tener exactamente 18 caracteres
            if (curp.Length != 18)
                return false;

            // Expresión regular: 4 letras, 6 dígitos, 1 letra, 2 letras, 3 letras, 2 alfanuméricos
            string patron = @"^[A-Z]{4}\d{6}[HM][A-Z]{2}[A-Z]{3}[A-Z0-9]{2}$";
            return Regex.IsMatch(curp, patron);
        }

        /// <summary>
        /// Genera un CURP basado en los datos del estudiante
        /// </summary>
        public string GenerarCURP(Estudiante estudiante)
        {
            try
            {
                string curp = string.Empty;

                // 1° y 2°: Inicial y primer vocal interna del primer apellido
                string primeraPartePrimeraApe = ObtenerPrimeraLetra(estudiante.ApellidoPaterno);
                string primerVocalPrimeraApe = ObtenerPrimerVocal(estudiante.ApellidoPaterno);
                curp += primeraPartePrimeraApe + primerVocalPrimeraApe;

                // 3° y 4°: Inicial del segundo apellido e inicial del nombre
                string inicialSegundoApe = ObtenerPrimeraLetra(estudiante.ApellidoMaterno);
                string inicialNombre = ObtenerPrimeraLetra(estudiante.Nombre);
                curp += inicialSegundoApe + inicialNombre;

                // 5° a 10°: Fecha de nacimiento (AAMMDD)
                string fechaNacimiento = estudiante.FechaNacimiento.ToString("yyMMdd");
                curp += fechaNacimiento;

                // 11°: Sexo (H/M)
                curp += estudiante.Sexo;

                // 12° y 13°: Abreviatura del estado de nacimiento
                if (!_abreviaturasPorEstado.ContainsKey(estudiante.EstadoNacimiento))
                    throw new ArgumentException($"Estado no válido: {estudiante.EstadoNacimiento}");

                string abrevEstado = _abreviaturasPorEstado[estudiante.EstadoNacimiento];
                curp += abrevEstado.Substring(0, 2).PadRight(2, 'X');

                // 14°, 15° y 16°: Primera consonante interna de apellidos y nombre
                string primerConsApe1 = ObtenerPrimeraConsonante(estudiante.ApellidoPaterno);
                string primerConsApe2 = ObtenerPrimeraConsonante(estudiante.ApellidoMaterno);
                string primerConsNombre = ObtenerPrimeraConsonante(estudiante.Nombre);
                curp += primerConsApe1 + primerConsApe2 + primerConsNombre;

                // 17° y 18°: Números aleatorios
                Random random = new Random();
                string aleatorio = random.Next(0, 100).ToString("D2");
                curp += aleatorio;

                return curp.ToUpper();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al generar CURP: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Valida que el CURP corresponda con los datos del estudiante
        /// </summary>
        public bool ValidarCURP(string curp, Estudiante estudiante)
        {
            if (!ValidarFormatoCURP(curp))
                return false;

            // Validar que el CURP generado coincida con el proporcionado
            string curpGenerado = GenerarCURP(estudiante);
            
            // Comparar los primeros 16 caracteres (sin los aleatorios)
            return curp.Substring(0, 16) == curpGenerado.Substring(0, 16);
        }

        private string ObtenerPrimeraLetra(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return "X";
            return texto[0].ToString().ToUpper();
        }

        private string ObtenerPrimerVocal(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return "X";

            string vocales = "AEIOU";
            for (int i = 1; i < texto.Length; i++)
            {
                if (vocales.Contains(char.ToUpper(texto[i]).ToString()))
                    return char.ToUpper(texto[i]).ToString();
            }
            return "X";
        }

        private string ObtenerPrimeraConsonante(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return "X";

            string consonantes = "BCDFGHJKLMNÑPQRSTVWXYZ";
            for (int i = 1; i < texto.Length; i++)
            {
                if (consonantes.Contains(char.ToUpper(texto[i]).ToString()))
                    return char.ToUpper(texto[i]).ToString();
            }
            return "X";
        }
    }
}