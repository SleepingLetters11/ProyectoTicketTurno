using System;
using System.Windows.Forms;
using Serilog;

namespace ProyectoTicketTurno.Presentation
{
    public partial class FormLogin : Form
    {
        private readonly ILogger _logger;
        private const string USUARIO_ADMIN = "admin";
        private const string CONTRASEÑA_ADMIN = "admin123"; // En producción usar hash

        public FormLogin(ILogger logger)
        {
            InitializeComponent();
            _logger = logger;
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "Aplicación de Gestión de Turnos - Login";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new System.Drawing.Size(400, 250);
        }

        private void InitializeComponent()
        {
            // Panel principal
            var panelPrincipal = new Panel();
            panelPrincipal.Dock = DockStyle.Fill;
            panelPrincipal.BackColor = System.Drawing.Color.White;

            // Label usuario
            var labelUsuario = new Label();
            labelUsuario.Text = "Usuario:";
            labelUsuario.Location = new System.Drawing.Point(50, 40);
            labelUsuario.Size = new System.Drawing.Size(100, 25);
            labelUsuario.Font = new System.Drawing.Font("Segoe UI", 10F);

            // TextBox usuario
            var textBoxUsuario = new TextBox();
            textBoxUsuario.Name = "textBoxUsuario";
            textBoxUsuario.Location = new System.Drawing.Point(150, 40);
            textBoxUsuario.Size = new System.Drawing.Size(200, 25);
            textBoxUsuario.Font = new System.Drawing.Font("Segoe UI", 10F);

            // Label contraseña
            var labelContraseña = new Label();
            labelContraseña.Text = "Contraseña:";
            labelContraseña.Location = new System.Drawing.Point(50, 80);
            labelContraseña.Size = new System.Drawing.Size(100, 25);
            labelContraseña.Font = new System.Drawing.Font("Segoe UI", 10F);

            // TextBox contraseña
            var textBoxContraseña = new TextBox();
            textBoxContraseña.Name = "textBoxContraseña";
            textBoxContraseña.Location = new System.Drawing.Point(150, 80);
            textBoxContraseña.Size = new System.Drawing.Size(200, 25);
            textBoxContraseña.UseSystemPasswordChar = true;
            textBoxContraseña.Font = new System.Drawing.Font("Segoe UI", 10F);

            // Botón login
            var btnLogin = new Button();
            btnLogin.Text = "Iniciar Sesión";
            btnLogin.Location = new System.Drawing.Point(150, 130);
            btnLogin.Size = new System.Drawing.Size(200, 40);
            btnLogin.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnLogin.BackColor = System.Drawing.Color.FromArgb(33, 150, 243);
            btnLogin.ForeColor = System.Drawing.Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Click += (s, e) => BotonLogin_Click(textBoxUsuario, textBoxContraseña);

            // Botón salir
            var btnSalir = new Button();
            btnSalir.Text = "Salir";
            btnSalir.Location = new System.Drawing.Point(150, 180);
            btnSalir.Size = new System.Drawing.Size(200, 40);
            btnSalir.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnSalir.BackColor = System.Drawing.Color.FromArgb(244, 67, 54);
            btnSalir.ForeColor = System.Drawing.Color.White;
            btnSalir.FlatStyle = FlatStyle.Flat;
            btnSalir.Click += (s, e) => CerrarAplicacion();

            panelPrincipal.Controls.Add(labelUsuario);
            panelPrincipal.Controls.Add(textBoxUsuario);
            panelPrincipal.Controls.Add(labelContraseña);
            panelPrincipal.Controls.Add(textBoxContraseña);
            panelPrincipal.Controls.Add(btnLogin);
            panelPrincipal.Controls.Add(btnSalir);

            this.Controls.Add(panelPrincipal);
        }

        private void BotonLogin_Click(TextBox textBoxUsuario, TextBox textBoxContraseña)
        {
            try
            {
                string usuario = textBoxUsuario.Text.Trim();
                string contraseña = textBoxContraseña.Text;

                if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contraseña))
                {
                    MessageBox.Show("Por favor ingrese usuario y contraseña", "Campos Requeridos", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (ValidarCredenciales(usuario, contraseña))
                {
                    _logger.Information("Usuario {Usuario} inició sesión correctamente", usuario);
                    
                    // Abrir formulario principal
                    FormPrincipal formPrincipal = new FormPrincipal(_logger);
                    this.Hide();
                    formPrincipal.ShowDialog();
                    this.Close();
                }
                else
                {
                    _logger.Warning("Intento de login fallido para usuario: {Usuario}", usuario);
                    MessageBox.Show("Usuario o contraseña incorrectos", "Error de Autenticación",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxContraseña.Clear();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error durante el login");
                MessageBox.Show($"Error: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidarCredenciales(string usuario, string contraseña)
        {
            // En producción, verificar contra base de datos con hash bcrypt
            return usuario == USUARIO_ADMIN && contraseña == CONTRASEÑA_ADMIN;
        }

        private void CerrarAplicacion()
        {
            try
            {
                DialogResult resultado = MessageBox.Show(
                    "¿Desea cerrar la aplicación?",
                    "Confirmación de Salida",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    _logger.Information("Aplicación cerrada por el usuario");
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al cerrar aplicación");
            }
        }
    }
}