using System;
using System.Text.RegularExpressions;

namespace ProyectoTicketTurno.Infrastructure.Utilities
{
    public static class FormatoCURPHelper
    {
        private static readonly string[] EstadoAbreviaturas = 
        {
            "AG", "BC", "BS", "CC", "CS", "CH", "CL", "CM", "DF", "DG",
            "GT", "GR", "HG", "JL", "MC", "MN", "MS", "NL", "OC", "PL",
            "QR", "SL", "SN", "SP", "TC", "TL", "TM", "TX", "VZ", "YN",
            "ZS", "CO", "NE", "NT"
        };

        public static bool ValidarFormatoCURP(string curp, string nombre, string apellidoPaterno, 
            string apellidoMaterno, DateTime fechaNacimiento, char sexo, string abreviaturaEstado)
        {
            if (string.IsNullOrWhiteSpace(curp) || curp.Length != 18)
                return false;

            curp = curp.ToUpper();

            // Validar caracteres alfanuméricos
            if (!Regex.IsMatch(curp, @"^[A-Z0-9]{18}$"))
                return false;

            // Posiciones 1-2: Inicial y primera vocal del apellido paterno
            string inicial1 = apellidoPaterno.Length > 0 ? apellidoPaterno[0].ToString() : "";
            string vocal1 = ObtenerPrimeraVocal(apellidoPaterno);
            if (inicial1.Length == 0 || !curp.StartsWith(inicial1))
                return false;

            // Posiciones 3-4: Inicial apellido materno + inicial nombre
            string inicial2 = apellidoMaterno.Length > 0 ? apellidoMaterno[0].ToString() : "";
            string inicial3 = nombre.Length > 0 ? nombre[0].ToString() : "";

            // Posiciones 5-10: Fecha nacimiento (AAMMDD)
            string fechaFormato = fechaNacimiento.ToString("yyMMdd");
            if (!curp.Substring(4, 6).Equals(fechaFormato))
                return false;

            // Posición 11: Sexo (H/M)
            if (curp[10] != sexo)
                return false;

            // Posiciones 12-13: Abreviatura estado nacimiento
            if (!curp.Substring(11, 2).Equals(abreviaturaEstado, StringComparison.OrdinalIgnoreCase))
                return false;

            return true;
        }

        public static string GenerarCURP(string nombre, string apellidoPaterno, 
            string apellidoMaterno, DateTime fechaNacimiento, char sexo, string abreviaturaEstado)
        {
            try
            {
                string curp = "";

                // Posiciones 1-2: Primera letra apellido paterno + primera vocal
                curp += apellidoPaterno.Length > 0 ? apellidoPaterno[0].ToString().ToUpper() : "";
                curp += ObtenerPrimeraVocal(apellidoPaterno);

                // Posiciones 3-4: Primera letra apellido materno + primera letra nombre
                curp += apellidoMaterno.Length > 0 ? apellidoMaterno[0].ToString().ToUpper() : "";
                curp += nombre.Length > 0 ? nombre[0].ToString().ToUpper() : "";

                // Posiciones 5-10: Fecha nacimiento (AAMMDD)
                curp += fechaNacimiento.ToString("yyMMdd");

                // Posición 11: Sexo
                curp += sexo;

                // Posiciones 12-13: Abreviatura estado
                curp += abreviaturaEstado.ToUpper().Substring(0, 2);

                // Posiciones 14-16: Primera consonante interna de apellidos y nombre
                curp += ObtenerPrimeraConsonante(apellidoPaterno);
                curp += ObtenerPrimeraConsonante(apellidoMaterno);
                curp += ObtenerPrimeraConsonante(nombre);

                // Posiciones 17-18: Aleatorios
                Random random = new Random();
                curp += random.Next(0, 10).ToString();
                curp += random.Next(0, 10).ToString();

                return curp;
            }
            catch
            {
                return null;
            }
        }

        private static string ObtenerPrimeraVocal(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return "";

            foreach (char c in texto.ToUpper())
            {
                if ("AEIOU".Contains(c.ToString()))
                    return c.ToString();
            }
            return "";
        }

        private static string ObtenerPrimeraConsonante(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return "";

            foreach (char c in texto.ToUpper())
            {
                if (!char.IsWhiteSpace(c) && !"AEIOU".Contains(c.ToString()))
                    return c.ToString();
            }
            return "";
        }
    }
}
