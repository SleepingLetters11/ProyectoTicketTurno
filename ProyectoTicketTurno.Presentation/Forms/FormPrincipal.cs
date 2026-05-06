using System;
using System.Windows.Forms;
using Serilog;

namespace ProyectoTicketTurno.Presentation
{
    public partial class FormPrincipal : Form
    {
        private readonly ILogger _logger;

        public FormPrincipal(ILogger logger)
        {
            InitializeComponent();
            _logger = logger;
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "Aplicación de Gestión de Turnos - Menú Principal";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new System.Drawing.Size(500, 400);
            this.FormClosing += FormPrincipal_FormClosing;
        }

        private void InitializeComponent()
        {
            // Panel principal
            var panelPrincipal = new Panel();
            panelPrincipal.Dock = DockStyle.Fill;
            panelPrincipal.BackColor = System.Drawing.Color.White;

            // Label título
            var labelTitulo = new Label();
            labelTitulo.Text = "MENÚ PRINCIPAL";
            labelTitulo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            labelTitulo.Location = new System.Drawing.Point(50, 30);
            labelTitulo.Size = new System.Drawing.Size(400, 50);
            labelTitulo.ForeColor = System.Drawing.Color.FromArgb(33, 150, 243);

            // Botón solicitar turno
            var btnSolicitudTurno = new Button();
            btnSolicitudTurno.Text = "Solicitar Turno";
            btnSolicitudTurno.Location = new System.Drawing.Point(100, 100);
            btnSolicitudTurno.Size = new System.Drawing.Size(300, 50);
            btnSolicitudTurno.Font = new System.Drawing.Font("Segoe UI", 11F);
            btnSolicitudTurno.BackColor = System.Drawing.Color.FromArgb(76, 175, 80);
            btnSolicitudTurno.ForeColor = System.Drawing.Color.White;
            btnSolicitudTurno.FlatStyle = FlatStyle.Flat;
            btnSolicitudTurno.Click += (s, e) => AbrirFormularioSolicitud();

            // Botón administrador
            var btnAdministrador = new Button();
            btnAdministrador.Text = "Panel Administrador";
            btnAdministrador.Location = new System.Drawing.Point(100, 170);
            btnAdministrador.Size = new System.Drawing.Size(300, 50);
            btnAdministrador.Font = new System.Drawing.Font("Segoe UI", 11F);
            btnAdministrador.BackColor = System.Drawing.Color.FromArgb(255, 152, 0);
            btnAdministrador.ForeColor = System.Drawing.Color.White;
            btnAdministrador.FlatStyle = FlatStyle.Flat;
            btnAdministrador.Click += (s, e) => AbrirFormularioAdministrador();

            // Botón dashboard
            var btnDashboard = new Button();
            btnDashboard.Text = "Dashboard";
            btnDashboard.Location = new System.Drawing.Point(100, 240);
            btnDashboard.Size = new System.Drawing.Size(300, 50);
            btnDashboard.Font = new System.Drawing.Font("Segoe UI", 11F);
            btnDashboard.BackColor = System.Drawing.Color.FromArgb(156, 39, 176);
            btnDashboard.ForeColor = System.Drawing.Color.White;
            btnDashboard.FlatStyle = FlatStyle.Flat;
            btnDashboard.Click += (s, e) => AbrirDashboard();

            // Botón salir
            var btnSalir = new Button();
            btnSalir.Text = "Cerrar Sesión";
            btnSalir.Location = new System.Drawing.Point(100, 310);
            btnSalir.Size = new System.Drawing.Size(300, 50);
            btnSalir.Font = new System.Drawing.Font("Segoe UI", 11F);
            btnSalir.BackColor = System.Drawing.Color.FromArgb(244, 67, 54);
            btnSalir.ForeColor = System.Drawing.Color.White;
            btnSalir.FlatStyle = FlatStyle.Flat;
            btnSalir.Click += (s, e) => CerrarSesion();

            panelPrincipal.Controls.Add(labelTitulo);
            panelPrincipal.Controls.Add(btnSolicitudTurno);
            panelPrincipal.Controls.Add(btnAdministrador);
            panelPrincipal.Controls.Add(btnDashboard);
            panelPrincipal.Controls.Add(btnSalir);

            this.Controls.Add(panelPrincipal);
        }

        private void AbrirFormularioSolicitud()
        {
            try
            {
                FormSolicitudTurno formSolicitud = new FormSolicitudTurno(_logger);
                formSolicitud.ShowDialog();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al abrir formulario de solicitud de turno");
                MessageBox.Show($"Error: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AbrirFormularioAdministrador()
        {
            try
            {
                FormAdministrador formAdmin = new FormAdministrador(_logger);
                formAdmin.ShowDialog();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al abrir formulario de administrador");
                MessageBox.Show($"Error: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AbrirDashboard()
        {
            try
            {
                FormDashboard formDashboard = new FormDashboard(_logger);
                formDashboard.ShowDialog();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al abrir dashboard");
                MessageBox.Show($"Error: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CerrarSesion()
        {
            try
            {
                DialogResult resultado = MessageBox.Show(
                    "¿Desea cerrar la sesión?",
                    "Confirmación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    _logger.Information("Sesión cerrada");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al cerrar sesión");
            }
        }

        private void FormPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _logger.Information("Cierre de sesión - Liberando recursos");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al cerrar formulario principal");
            }
        }
    }
}