using System;
using System.Windows.Forms;
using Serilog;
using Serilog.Core;
using ProyectoTicketTurno.Presentation;
using ProyectoTicketTurno.Data.Context;

namespace ProyectoTicketTurno
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // Configurar Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                // Inicializar base de datos
                InitializarBaseDatos();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                FormLogin loginForm = new FormLogin(Log.Logger);
                Application.Run(loginForm);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Error fatal en la aplicación");
                MessageBox.Show($"Error fatal: {ex.Message}", "Error Crítico",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void InitializarBaseDatos()
        {
            try
            {
                using (var context = new AplicacionDbContext())
                {
                    // Crear base de datos si no existe
                    context.Database.CreateIfNotExists();
                    Log.Information("Base de datos inicializada correctamente");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al inicializar la base de datos");
                throw;
            }
        }
    }
}