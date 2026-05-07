using System;
using System.Windows.Forms;
using Serilog;
using ProyectoTicketTurno.Business.Services;
using ProyectoTicketTurno.Business.Models;
using ProyectoTicketTurno.Data.Context;
using ProyectoTicketTurno.Data.Repositories;

namespace ProyectoTicketTurno.Presentation
{
    public partial class FormSolicitudTurno : Form
    {
        private readonly ILogger _logger;
        private readonly IValidacionCURPService _validacionCURPService;
        private readonly IEstudianteService _estudianteService;
        private readonly ISolicitudTurnoService _solicitudTurnoService;
        private Estudiante _estudianteActual;

        public FormSolicitudTurno(ILogger logger)
        {
            InitializeComponent();
            _logger = logger;

            // Inicializar servicios (en producción usar inyección de dependencias)
            var context = new AplicacionDbContext();
            var estudianteRepository = new EstudianteRepository(context);
            var solicitudTurnoRepository = new SolicitudTurnoRepository(context);
            var turnoService = new TurnoService(solicitudTurnoRepository, logger);

            _validacionCURPService = new ValidacionCURPService();
            _estudianteService = new EstudianteService(estudianteRepository, logger);
            _solicitudTurnoService = new SolicitudTurnoService(solicitudTurnoRepository, turnoService, logger);

            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "Solicitud de Turno";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new System.Drawing.Size(600, 700);
            this.AutoScroll = true;
        }

        private void InitializeComponent()
        {
            var panelPrincipal = new Panel();
            panelPrincipal.Dock = DockStyle.Fill;
            panelPrincipal.AutoScroll = true;
            panelPrincipal.BackColor = System.Drawing.Color.White;

            int yPos = 10;

            // Label título
            var labelTitulo = new Label();
            labelTitulo.Text = "SOLICITUD DE TURNO";
            labelTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            labelTitulo.Location = new System.Drawing.Point(20, yPos);
            labelTitulo.Size = new System.Drawing.Size(400, 30);
            yPos += 50;

            // Sección búsqueda
            var labelBusqueda = new Label();
            labelBusqueda.Text = "Ingrese CURP o Número de Turno:";
            labelBusqueda.Location = new System.Drawing.Point(20, yPos);
            labelBusqueda.Size = new System.Drawing.Size(300, 20);
            yPos += 25;

            var textBoxBusqueda = new TextBox();
            textBoxBusqueda.Name = "textBoxBusqueda";
            textBoxBusqueda.Location = new System.Drawing.Point(20, yPos);
            textBoxBusqueda.Size = new System.Drawing.Size(300, 25);
            yPos += 35;

            var btnBuscar = new Button();
            btnBuscar.Text = "Buscar";
            btnBuscar.Location = new System.Drawing.Point(330, yPos - 35);
            btnBuscar.Size = new System.Drawing.Size(100, 25);
            btnBuscar.BackColor = System.Drawing.Color.FromArgb(33, 150, 243);
            btnBuscar.ForeColor = System.Drawing.Color.White;
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.Click += (s, e) => BuscarSolicitud(textBoxBusqueda.Text);

            // Sección datos estudiante
            var labelDatos = new Label();
            labelDatos.Text = "Datos del Estudiante:";
            labelDatos.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            labelDatos.Location = new System.Drawing.Point(20, yPos);
            labelDatos.Size = new System.Drawing.Size(300, 25);
            yPos += 30;

            var labelCURP = new Label { Text = "CURP:", Location = new System.Drawing.Point(20, yPos), Size = new System.Drawing.Size(100, 20) };
            var textBoxCURP = new TextBox { Name = "textBoxCURP", Location = new System.Drawing.Point(130, yPos), Size = new System.Drawing.Size(200, 25) };
            yPos += 35;

            var labelNombre = new Label { Text = "Nombre:", Location = new System.Drawing.Point(20, yPos), Size = new System.Drawing.Size(100, 20) };
            var textBoxNombre = new TextBox { Name = "textBoxNombre", Location = new System.Drawing.Point(130, yPos), Size = new System.Drawing.Size(200, 25) };
            yPos += 35;

            var labelApellidoP = new Label { Text = "Apellido Paterno:", Location = new System.Drawing.Point(20, yPos), Size = new System.Drawing.Size(100, 20) };
            var textBoxApellidoP = new TextBox { Name = "textBoxApellidoP", Location = new System.Drawing.Point(130, yPos), Size = new System.Drawing.Size(200, 25) };
            yPos += 35;

            var labelApellidoM = new Label { Text = "Apellido Materno:", Location = new System.Drawing.Point(20, yPos), Size = new System.Drawing.Size(100, 20) };
            var textBoxApellidoM = new TextBox { Name = "textBoxApellidoM", Location = new System.Drawing.Point(130, yPos), Size = new System.Drawing.Size(200, 25) };
            yPos += 35;

            var labelFechaNac = new Label { Text = "Fecha Nacimiento:", Location = new System.Drawing.Point(20, yPos), Size = new System.Drawing.Size(100, 20) };
            var dateTimePickerFechaNac = new DateTimePicker { Name = "dateTimePickerFechaNac", Location = new System.Drawing.Point(130, yPos), Size = new System.Drawing.Size(200, 25) };
            yPos += 35;

            var labelSexo = new Label { Text = "Sexo:", Location = new System.Drawing.Point(20, yPos), Size = new System.Drawing.Size(100, 20) };
            var comboBoxSexo = new ComboBox { Name = "comboBoxSexo", Location = new System.Drawing.Point(130, yPos), Size = new System.Drawing.Size(200, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            comboBoxSexo.Items.AddRange(new object[] { "Hombre", "Mujer" });
            yPos += 35;

            var labelEstadoNac = new Label { Text = "Estado Nacimiento:", Location = new System.Drawing.Point(20, yPos), Size = new System.Drawing.Size(100, 20) };
            var textBoxEstadoNac = new TextBox { Name = "textBoxEstadoNac", Location = new System.Drawing.Point(130, yPos), Size = new System.Drawing.Size(200, 25) };
            yPos += 35;

            var labelMunicipio = new Label { Text = "Municipio Estudio:", Location = new System.Drawing.Point(20, yPos), Size = new System.Drawing.Size(100, 20) };
            var comboBoxMunicipio = new ComboBox { Name = "comboBoxMunicipio", Location = new System.Drawing.Point(130, yPos), Size = new System.Drawing.Size(200, 25) };
            CargarMunicipios(comboBoxMunicipio);
            yPos += 35;

            var labelTelefono = new Label { Text = "Teléfono:", Location = new System.Drawing.Point(20, yPos), Size = new System.Drawing.Size(100, 20) };
            var textBoxTelefono = new TextBox { Name = "textBoxTelefono", Location = new System.Drawing.Point(130, yPos), Size = new System.Drawing.Size(200, 25) };
            yPos += 35;

            var labelNivel = new Label { Text = "Nivel Educativo:", Location = new System.Drawing.Point(20, yPos), Size = new System.Drawing.Size(100, 20) };
            var comboBoxNivel = new ComboBox { Name = "comboBoxNivel", Location = new System.Drawing.Point(130, yPos), Size = new System.Drawing.Size(200, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            comboBoxNivel.Items.AddRange(new object[] { "Preescolar", "Primaria", "Secundaria", "Preparatoria" });
            yPos += 35;

            var labelGrado = new Label { Text = "Grado:", Location = new System.Drawing.Point(20, yPos), Size = new System.Drawing.Size(100, 20) };
            var numericUpDownGrado = new NumericUpDown { Name = "numericUpDownGrado", Location = new System.Drawing.Point(130, yPos), Size = new System.Drawing.Size(200, 25), Minimum = 1, Maximum = 6 };
            yPos += 40;

            // Sección trámite
            var labelSeccionTramite = new Label();
            labelSeccionTramite.Text = "Datos del Trámite:";
            labelSeccionTramite.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            labelSeccionTramite.Location = new System.Drawing.Point(20, yPos);
            labelSeccionTramite.Size = new System.Drawing.Size(300, 25);
            yPos += 30;

            var labelAsunto = new Label { Text = "Asunto:", Location = new System.Drawing.Point(20, yPos), Size = new System.Drawing.Size(100, 20) };
            var textBoxAsunto = new TextBox { Name = "textBoxAsunto", Location = new System.Drawing.Point(130, yPos), Size = new System.Drawing.Size(300, 25), Multiline = true, Height = 50 };
            yPos += 60;

            var labelPersona = new Label { Text = "Persona Trámite:", Location = new System.Drawing.Point(20, yPos), Size = new System.Drawing.Size(100, 20) };
            var textBoxPersona = new TextBox { Name = "textBoxPersona", Location = new System.Drawing.Point(130, yPos), Size = new System.Drawing.Size(200, 25) };
            yPos += 35;

            var labelParentesco = new Label { Text = "Parentesco:", Location = new System.Drawing.Point(20, yPos), Size = new System.Drawing.Size(100, 20) };
            var comboBoxParentesco = new ComboBox { Name = "comboBoxParentesco", Location = new System.Drawing.Point(130, yPos), Size = new System.Drawing.Size(200, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            comboBoxParentesco.Items.AddRange(new object[] { "Padre", "Madre", "Tutor", "Abuelo", "Otro" });
            yPos += 40;

            // Botones
            var btnGuardar = new Button();
            btnGuardar.Text = "Guardar Solicitud";
            btnGuardar.Location = new System.Drawing.Point(130, yPos);
            btnGuardar.Size = new System.Drawing.Size(150, 40);
            btnGuardar.BackColor = System.Drawing.Color.FromArgb(76, 175, 80);
            btnGuardar.ForeColor = System.Drawing.Color.White;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Click += (s, e) => GuardarSolicitud(textBoxCURP, textBoxNombre, textBoxApellidoP, 
                textBoxApellidoM, dateTimePickerFechaNac, comboBoxSexo, textBoxEstadoNac, comboBoxMunicipio, 
                textBoxTelefono, comboBoxNivel, numericUpDownGrado, textBoxAsunto, textBoxPersona, comboBoxParentesco);

            var btnLimpiar = new Button();
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.Location = new System.Drawing.Point(290, yPos);
            btnLimpiar.Size = new System.Drawing.Size(150, 40);
            btnLimpiar.BackColor = System.Drawing.Color.FromArgb(158, 158, 158);
            btnLimpiar.ForeColor = System.Drawing.Color.White;
            btnLimpiar.FlatStyle = FlatStyle.Flat;
            btnLimpiar.Click += (s, e) => LimpiarFormulario(textBoxCURP, textBoxNombre, textBoxApellidoP, 
                textBoxApellidoM, dateTimePickerFechaNac, comboBoxSexo, textBoxEstadoNac, comboBoxMunicipio, 
                textBoxTelefono, comboBoxNivel, numericUpDownGrado, textBoxAsunto, textBoxPersona, comboBoxParentesco);

            panelPrincipal.Controls.AddRange(new Control[] {
                labelTitulo, labelBusqueda, textBoxBusqueda, btnBuscar,
                labelDatos, labelCURP, textBoxCURP, labelNombre, textBoxNombre,
                labelApellidoP, textBoxApellidoP, labelApellidoM, textBoxApellidoM,
                labelFechaNac, dateTimePickerFechaNac, labelSexo, comboBoxSexo,
                labelEstadoNac, textBoxEstadoNac, labelMunicipio, comboBoxMunicipio,
                labelTelefono, textBoxTelefono, labelNivel, comboBoxNivel,
                labelGrado, numericUpDownGrado, labelSeccionTramite,
                labelAsunto, textBoxAsunto, labelPersona, textBoxPersona,
                labelParentesco, comboBoxParentesco, btnGuardar, btnLimpiar
            });

            this.Controls.Add(panelPrincipal);
        }

        private void CargarMunicipios(ComboBox comboBox)
        {
            try
            {
                comboBox.Items.AddRange(new object[] {
                    "Saltillo", "Torreón", "Monclova", "Parras", "Matamoros",
                    "Acuña", "Frontera", "Castaños", "Nadadores", "Escobedo"
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al cargar municipios");
            }
        }

        private void BuscarSolicitud(string busqueda)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(busqueda))
                {
                    MessageBox.Show("Ingrese un CURP o número de turno", "Búsqueda", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _estudianteActual = _estudianteService.ObtenerEstudiantePorCURP(busqueda);
                if (_estudianteActual != null)
                {
                    CargarDatosEstudiante(_estudianteActual);
                }
                else
                {
                    MessageBox.Show("No se encontró el estudiante", "Búsqueda", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al buscar solicitud");
                MessageBox.Show($"Error: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarDatosEstudiante(Estudiante estudiante)
        {
            try
            {
                this.Controls["textBoxCURP"].Text = estudiante.CURP;
                this.Controls["textBoxNombre"].Text = estudiante.Nombre;
                this.Controls["textBoxApellidoP"].Text = estudiante.ApellidoPaterno;
                this.Controls["textBoxApellidoM"].Text = estudiante.ApellidoMaterno;
                ((DateTimePicker)this.Controls["dateTimePickerFechaNac"]).Value = estudiante.FechaNacimiento;
                ((ComboBox)this.Controls["comboBoxSexo"]).SelectedItem = estudiante.Sexo == 'H' ? "Hombre" : "Mujer";
                this.Controls["textBoxEstadoNac"].Text = estudiante.EstadoNacimiento;
                ((ComboBox)this.Controls["comboBoxMunicipio"]).SelectedItem = estudiante.MunicipioEstudio;
                this.Controls["textBoxTelefono"].Text = estudiante.TelefonoContacto;
                ((ComboBox)this.Controls["comboBoxNivel"]).SelectedItem = estudiante.NivelEducativo;
                ((NumericUpDown)this.Controls["numericUpDownGrado"]).Value = estudiante.Grado;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al cargar datos de estudiante");
            }
        }

        private void GuardarSolicitud(TextBox textBoxCURP, TextBox textBoxNombre, TextBox textBoxApellidoP,
            TextBox textBoxApellidoM, DateTimePicker dateTimePickerFechaNac, ComboBox comboBoxSexo,
            TextBox textBoxEstadoNac, ComboBox comboBoxMunicipio, TextBox textBoxTelefono,
            ComboBox comboBoxNivel, NumericUpDown numericUpDownGrado, TextBox textBoxAsunto,
            TextBox textBoxPersona, ComboBox comboBoxParentesco)
        {
            try
            {
                // Validar campos obligatorios
                if (string.IsNullOrWhiteSpace(textBoxNombre.Text) ||
                    string.IsNullOrWhiteSpace(textBoxApellidoP.Text) ||
                    string.IsNullOrWhiteSpace(textBoxAsunto.Text) ||
                    comboBoxMunicipio.SelectedItem == null)
                {
                    MessageBox.Show("Por favor complete todos los campos obligatorios", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Crear o actualizar estudiante
                var estudiante = new Estudiante
                {
                    CURP = textBoxCURP.Text,
                    Nombre = textBoxNombre.Text,
                    ApellidoPaterno = textBoxApellidoP.Text,
                    ApellidoMaterno = textBoxApellidoM.Text,
                    FechaNacimiento = dateTimePickerFechaNac.Value,
                    Sexo = comboBoxSexo.SelectedItem.ToString() == "Hombre" ? 'H' : 'M',
                    EstadoNacimiento = textBoxEstadoNac.Text,
                    MunicipioEstudio = comboBoxMunicipio.SelectedItem.ToString(),
                    TelefonoContacto = textBoxTelefono.Text,
                    NivelEducativo = comboBoxNivel.SelectedItem?.ToString(),
                    Grado = (int)numericUpDownGrado.Value,
                    Edad = DateTime.Now.Year - dateTimePickerFechaNac.Value.Year
                };

                // Generar CURP
                if (string.IsNullOrWhiteSpace(textBoxCURP.Text))
                {
                    estudiante.CURP = _validacionCURPService.GenerarCURP(estudiante);
                    textBoxCURP.Text = estudiante.CURP;
                }

                // Guardar estudiante
                if (_estudianteActual == null)
                {
                    _estudianteService.CrearEstudiante(estudiante);
                }
                else
                {
                    _estudianteService.ActualizarEstudiante(estudiante);
                }

                // Crear solicitud de turno
                var solicitud = new SolicitudTurno
                {
                    CURP = estudiante.CURP,
                    Municipio = comboBoxMunicipio.SelectedItem.ToString(),
                    Asunto = textBoxAsunto.Text,
                    PersonaTramitera = textBoxPersona.Text,
                    Parentesco = comboBoxParentesco.SelectedItem?.ToString()
                };

                _solicitudTurnoService.CrearSolicitudTurno(solicitud);

                MessageBox.Show($"Solicitud guardada. Turno asignado: {solicitud.NumeroTurno}", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Generar comprobante (en la siguiente fase)
                _logger.Information("Solicitud de turno guardada: {NumeroTurno}", solicitud.NumeroTurno);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al guardar solicitud");
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario(TextBox textBoxCURP, TextBox textBoxNombre, TextBox textBoxApellidoP,
            TextBox textBoxApellidoM, DateTimePicker dateTimePickerFechaNac, ComboBox comboBoxSexo,
            TextBox textBoxEstadoNac, ComboBox comboBoxMunicipio, TextBox textBoxTelefono,
            ComboBox comboBoxNivel, NumericUpDown numericUpDownGrado, TextBox textBoxAsunto,
            TextBox textBoxPersona, ComboBox comboBoxParentesco)
        {
            textBoxCURP.Clear();
            textBoxNombre.Clear();
            textBoxApellidoP.Clear();
            textBoxApellidoM.Clear();
            dateTimePickerFechaNac.Value = DateTime.Now;
            comboBoxSexo.SelectedIndex = -1;
            textBoxEstadoNac.Clear();
            comboBoxMunicipio.SelectedIndex = -1;
            textBoxTelefono.Clear();
            comboBoxNivel.SelectedIndex = -1;
            numericUpDownGrado.Value = 1;
            textBoxAsunto.Clear();
            textBoxPersona.Clear();
            comboBoxParentesco.SelectedIndex = -1;
            _estudianteActual = null;
        }
    }
}