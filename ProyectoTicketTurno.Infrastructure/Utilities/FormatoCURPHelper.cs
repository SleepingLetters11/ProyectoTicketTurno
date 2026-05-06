using System;
using System.Collections.Generic;
using ProyectoTicketTurno.Business.Models;

namespace ProyectoTicketTurno.Infrastructure.Utilities
{
    public static class FormatoCURPHelper
    {
        private static readonly Dictionary<string, string> AbreviaturasPorEstado = new Dictionary<string, string>
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

        public static bool ValidarFormatoCURP(string curp)
        {
            if (string.IsNullOrWhiteSpace(curp) || curp.Length != 18)
                return false;

            return System.Text.RegularExpressions.Regex.IsMatch(curp, @"^[A-Z]{4}\d{6}[HM][A-Z]{2}[A-Z]{3}[A-Z0-9]{2}$");
        }

        public static string ObtenerAbreviaturaPorEstado(string estado)
        {
            return AbreviaturasPorEstado.TryGetValue(estado, out var abreviatura) ? abreviatura : "XX";
        }

        public static string FormattearCURP(string curp)
        {
            if (string.IsNullOrWhiteSpace(curp) || curp.Length != 18)
                return curp;

            return $"{curp.Substring(0, 4)}-{curp.Substring(4, 6)}-{curp.Substring(10, 3)}-{curp.Substring(13)}";
        }
    }
}