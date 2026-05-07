using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Serilog;
using ProyectoTicketTurno.Business.Services;
using ProyectoTicketTurno.Data.Context;
using ProyectoTicketTurno.Data.Repositories;

namespace ProyectoTicketTurno.Presentation
{
    public partial class FormConsultaEstudiante : Form
    {
        private readonly ILogger _logger;
        private readonly IEstudianteService _estudianteService;
        private readonly ISolicitudTurnoService _solicitudTurnoService;

        public FormConsultaEstudiante(ILogger logger)
        {
            InitializeComponent();
            _logger = logger;

            var context = new AplicacionDbContext();
            var estudianteRepository = new EstudianteRepository(context);
            var solicitudTurnoRepository = new SolicitudTurnoRepository(context);
            var turnoService = new TurnoService(solicitudTurnoRepository, logger);

            _estudianteService = new EstudianteService(estudianteRepository, logger);
            _solicitudTurnoService = new SolicitudTurnoService(solicitudTurnoRepository, turnoService, logger);

            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "Consulta de Estudiante";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new System.Drawing.Size(700, 600);
        }

        private void InitializeComponent()
        {
            var panelPrincipal = new Panel();
            panelPrincipal.Dock = DockStyle.Fill;
            panelPrincipal.BackColor = System.Drawing.Color.White;

            int yPos = 15;

            // Título
            var labelTitulo = new Label();
            labelTitulo.Text = "CONSULTA DE ESTUDIANTE";
            labelTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            labelTitulo.Location = new System.Drawing.Point(20, yPos);
            labelTitulo.Size = new System.Drawing.Size(400, 30);
            labelTitulo.ForeColor = System.Drawing.Color.FromArgb(33, 150, 243);
            yPos += 50;

            // Búsqueda
            var labelBusqueda = new Label();
            labelBusqueda.Text = "CURP:";
            labelBusqueda.Location = new System.Drawing.Point(20, yPos);
            labelBusqueda.Size = new System.Drawing.Size(100, 20);
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
            btnBuscar.Click += (s, e) => BuscarEstudiante(textBoxBusqueda.Text);

            // Panel de resultados
            var panelResultados = new Panel();
            panelResultados.Location = new System.Drawing.Point(20, yPos);
            panelResultados.Size = new System.Drawing.Size(620, 400);
            panelResultados.BorderStyle = BorderStyle.FixedSingle;
            panelResultados.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            panelResultados.AutoScroll = true;

            // Añadir panel al formulario
            panelPrincipal.Controls.AddRange(new Control[] {
                labelTitulo, labelBusqueda, textBoxBusqueda, btnBuscar, panelResultados
            });

            this.Controls.Add(panelPrincipal);
        }

        private void BuscarEstudiante(string curp)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(curp))
                {
                    MessageBox.Show("Ingrese un CURP", "Búsqueda",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var estudiante = _estudianteService.ObtenerEstudiantePorCURP(curp);
                if (estudiante == null)
                {
                    MessageBox.Show("No se encontró el estudiante", "Búsqueda",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                MostrarResultados(estudiante);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al buscar estudiante");
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarResultados(Business.Models.Estudiante estudiante)
        {
            try
            {
                var panelResultados = this.Controls.OfType<Panel>().Where(p => p.Location.Y > 100).FirstOrDefault();
                if (panelResultados != null)
                {
                    panelResultados.Controls.Clear();
                    int yPos = 10;

                    var fuenteLabel = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
                    var fuenteNormal = new System.Drawing.Font("Segoe UI", 10F);

                    // Mostrar datos
                    MostrarCampo(panelResultados, "Nombre Completo:", $"{estudiante.Nombre} {estudiante.ApellidoPaterno} {estudiante.ApellidoMaterno}", ref yPos, fuenteLabel, fuenteNormal);
                    MostrarCampo(panelResultados, "CURP:", estudiante.CURP, ref yPos, fuenteLabel, fuenteNormal);
                    MostrarCampo(panelResultados, "Fecha de Nacimiento:", estudiante.FechaNacimiento.ToString("dd/MM/yyyy"), ref yPos, fuenteLabel, fuenteNormal);
                    MostrarCampo(panelResultados, "Edad:", estudiante.Edad.ToString(), ref yPos, fuenteLabel, fuenteNormal);
                    MostrarCampo(panelResultados, "Sexo:", estudiante.Sexo == 'H' ? "Hombre" : "Mujer", ref yPos, fuenteLabel, fuenteNormal);
                    MostrarCampo(panelResultados, "Estado Nacimiento:", estudiante.EstadoNacimiento, ref yPos, fuenteLabel, fuenteNormal);
                    MostrarCampo(panelResultados, "Municipio Estudio:", estudiante.MunicipioEstudio, ref yPos, fuenteLabel, fuenteNormal);
                    MostrarCampo(panelResultados, "Teléfono:", estudiante.TelefonoContacto, ref yPos, fuenteLabel, fuenteNormal);
                    MostrarCampo(panelResultados, "Nivel Educativo:", estudiante.NivelEducativo, ref yPos, fuenteLabel, fuenteNormal);
                    MostrarCampo(panelResultados, "Grado:", estudiante.Grado.ToString(), ref yPos, fuenteLabel, fuenteNormal);

                    // Mostrar solicitudes asociadas
                    yPos += 20;
                    var labelSolicitudes = new Label();
                    labelSolicitudes.Text = "SOLICITUDES DE TURNO ASOCIADAS:";
                    labelSolicitudes.Font = fuenteLabel;
                    labelSolicitudes.Location = new System.Drawing.Point(10, yPos);
                    labelSolicitudes.Size = new System.Drawing.Size(400, 20);
                    panelResultados.Controls.Add(labelSolicitudes);
                    yPos += 25;

                    var solicitudes = _solicitudTurnoService.ObtenerSolicitudesPorMunicipio("")
                        .Where(s => s.CURP == estudiante.CURP)
                        .OrderByDescending(s => s.FechaSolicitud)
                        .ToList();

                    if (solicitudes.Count > 0)
                    {
                        foreach (var solicitud in solicitudes)
                        {
                            MostrarCampo(panelResultados, $"Turno #{solicitud.NumeroTurno}:", $"Estado: {solicitud.Estatus} - {solicitud.FechaSolicitud:dd/MM/yyyy}", ref yPos, fuenteLabel, fuenteNormal);
                        }
                    }
                    else
                    {
                        var labelSinSolicitudes = new Label();
                        labelSinSolicitudes.Text = "No hay solicitudes registradas";
                        labelSinSolicitudes.Location = new System.Drawing.Point(10, yPos);
                        labelSinSolicitudes.Size = new System.Drawing.Size(400, 20);
                        labelSinSolicitudes.ForeColor = System.Drawing.Color.Gray;
                        panelResultados.Controls.Add(labelSinSolicitudes);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al mostrar resultados");
            }
        }

        private void MostrarCampo(Panel panel, string etiqueta, string valor, ref int yPos, System.Drawing.Font fuenteLabel, System.Drawing.Font fuenteNormal)
        {
            var labelEtiqueta = new Label();
            labelEtiqueta.Text = etiqueta;
            labelEtiqueta.Font = fuenteLabel;
            labelEtiqueta.Location = new System.Drawing.Point(10, yPos);
            labelEtiqueta.Size = new System.Drawing.Size(200, 20);
            panel.Controls.Add(labelEtiqueta);

            var labelValor = new Label();
            labelValor.Text = valor;
            labelValor.Font = fuenteNormal;
            labelValor.Location = new System.Drawing.Point(220, yPos);
            labelValor.Size = new System.Drawing.Size(380, 20);
            labelValor.AutoSize = false;
            panel.Controls.Add(labelValor);

            yPos += 25;
        }
    }
}