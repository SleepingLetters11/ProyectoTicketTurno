using System;
using System.Windows.Forms;

namespace ProyectoTicketTurno.Presentation
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // TODO: Agregar FormLogin como punto de entrada
            // Application.Run(new FormLogin());
            
            MessageBox.Show("Aplicación Ticket de Turno - Fase 1 en Construcción", "Información");
        }
    }
}
