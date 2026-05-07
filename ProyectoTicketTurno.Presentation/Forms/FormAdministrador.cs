using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Serilog;
using ProyectoTicketTurno.Business.Services;
using ProyectoTicketTurno.Business.Models;
using ProyectoTicketTurno.Data.Context;
using ProyectoTicketTurno.Data.Repositories;

namespace ProyectoTicketTurno.Presentation
{
    public partial class FormAdministrador : Form
    {
        private readonly ILogger _logger;
        private readonly ISolicitudTurnoService _solicitudTurnoService;
        private readonly IEstudianteService _estudianteService;

        public FormAdministrador(ILogger logger)
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
<<<<<<< HEAD
            this.Text = "Panel Administradors";
=======
            this.Text = "Panel Administrador";
>>>>>>> 5f19ed9ab7e7f99b306b1760875ec83b456feab6
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new System.Drawing.Size(800, 600);
        }

        private void InitializeComponent()
        {
            var panelPrincipal = new Panel();
            panelPrincipal.Dock = DockStyle.Fill;
            panelPrincipal.BackColor = System.Drawing.Color.White;

            int yPos = 10;

            // Label título
            var labelTitulo = new Label();
            labelTitulo.Text = "PANEL ADMINISTRADOR";
            labelTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            labelTitulo.Location = new System.Drawing.Point(20, yPos);
            labelTitulo.Size = new System.Drawing.Size(400, 30);
            yPos += 50;

            // Búsqueda
            var labelBusqueda = new Label();
            labelBusqueda.Text = "Buscar por CURP:";
            labelBusqueda.Location = new System.Drawing.Point(20, yPos);
            labelBusqueda.Size = new System.Drawing.Size(150, 20);
            yPos += 25;

            var textBoxBusqueda = new TextBox();
            textBoxBusqueda.Name = "textBoxBusqueda";
            textBoxBusqueda.Location = new System.Drawing.Point(20, yPos);
            textBoxBusqueda.Size = new System.Drawing.Size(250, 25);
            yPos += 35;

            var btnBuscar = new Button();
            btnBuscar.Text = "Buscar";
            btnBuscar.Location = new System.Drawing.Point(280, yPos - 35);
            btnBuscar.Size = new System.Drawing.Size(100, 25);
            btnBuscar.BackColor = System.Drawing.Color.FromArgb(33, 150, 243);
            btnBuscar.ForeColor = System.Drawing.Color.White;
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.Click += (s, e) => BuscarSolicitud(textBoxBusqueda.Text);

            // DataGridView para resultados
            var dataGridViewSolicitudes = new DataGridView();
            dataGridViewSolicitudes.Name = "dataGridViewSolicitudes";
            dataGridViewSolicitudes.Location = new System.Drawing.Point(20, yPos);
            dataGridViewSolicitudes.Size = new System.Drawing.Size(740, 300);
            dataGridViewSolicitudes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewSolicitudes.ReadOnly = true;
            dataGridViewSolicitudes.AllowUserToAddRows = false;

            // Columnas
            dataGridViewSolicitudes.Columns.Add("NumeroTurno", "Número Turno");
            dataGridViewSolicitudes.Columns.Add("CURP", "CURP");
            dataGridViewSolicitudes.Columns.Add("Municipio", "Municipio");
            dataGridViewSolicitudes.Columns.Add("FechaSolicitud", "Fecha Solicitud");
            dataGridViewSolicitudes.Columns.Add("Estatus", "Estatus");

            yPos += 310;

            // ComboBox para cambiar estatus
            var labelEstatus = new Label();
            labelEstatus.Text = "Cambiar Estatus:";
            labelEstatus.Location = new System.Drawing.Point(20, yPos);
            labelEstatus.Size = new System.Drawing.Size(150, 20);
            yPos += 25;

            var comboBoxEstatus = new ComboBox();
            comboBoxEstatus.Name = "comboBoxEstatus";
            comboBoxEstatus.Location = new System.Drawing.Point(20, yPos);
            comboBoxEstatus.Size = new System.Drawing.Size(200, 25);
            comboBoxEstatus.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxEstatus.Items.AddRange(new object[] { "Pendiente", "Resuelto" });

            var btnCambiarEstatus = new Button();
            btnCambiarEstatus.Text = "Actualizar Estatus";
            btnCambiarEstatus.Location = new System.Drawing.Point(230, yPos);
            btnCambiarEstatus.Size = new System.Drawing.Size(150, 25);
            btnCambiarEstatus.BackColor = System.Drawing.Color.FromArgb(255, 152, 0);
            btnCambiarEstatus.ForeColor = System.Drawing.Color.White;
            btnCambiarEstatus.FlatStyle = FlatStyle.Flat;
            btnCambiarEstatus.Click += (s, e) => CambiarEstatus(dataGridViewSolicitudes, comboBoxEstatus);

            var btnSalir = new Button();
            btnSalir.Text = "Cerrar";
            btnSalir.Location = new System.Drawing.Point(390, yPos);
            btnSalir.Size = new System.Drawing.Size(100, 25);
            btnSalir.BackColor = System.Drawing.Color.FromArgb(244, 67, 54);
            btnSalir.ForeColor = System.Drawing.Color.White;
            btnSalir.FlatStyle = FlatStyle.Flat;
            btnSalir.Click += (s, e) => this.Close();

            panelPrincipal.Controls.AddRange(new Control[] {
                labelTitulo, labelBusqueda, textBoxBusqueda, btnBuscar,
                dataGridViewSolicitudes, labelEstatus, comboBoxEstatus,
                btnCambiarEstatus, btnSalir
            });

            this.Controls.Add(panelPrincipal);
        }

        private void BuscarSolicitud(string curp)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(curp))
                {
                    MessageBox.Show("Ingrese un CURP", "Búsqueda",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var solicitudes = _solicitudTurnoService.ObtenerSolicitudesPorMunicipio("") ?? new List<SolicitudTurno>();
                var filtradas = solicitudes.Where(s => s.CURP.Contains(curp)).ToList();

                MostrarSolicitudes(filtradas);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al buscar solicitud");
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarSolicitudes(List<SolicitudTurno> solicitudes)
        {
            try
            {
                var dataGridView = (DataGridView)this.Controls.Find("dataGridViewSolicitudes", true).FirstOrDefault();
                if (dataGridView != null)
                {
                    dataGridView.Rows.Clear();
                    foreach (var solicitud in solicitudes)
                    {
                        dataGridView.Rows.Add(
                            solicitud.NumeroTurno,
                            solicitud.CURP,
                            solicitud.Municipio,
                            solicitud.FechaSolicitud.ToString("yyyy-MM-dd"),
                            solicitud.Estatus.ToString()
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al mostrar solicitudes");
            }
        }

        private void CambiarEstatus(DataGridView dataGridView, ComboBox comboBoxEstatus)
        {
            try
            {
                if (dataGridView.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Seleccione una solicitud", "Selección",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (comboBoxEstatus.SelectedItem == null)
                {
                    MessageBox.Show("Seleccione un estatus", "Selección",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int numeroTurno = (int)dataGridView.SelectedRows[0].Cells[0].Value;
                EstatusEnum nuevoEstatus = comboBoxEstatus.SelectedItem.ToString() == "Pendiente" 
                    ? EstatusEnum.Pendiente 
                    : EstatusEnum.Resuelto;

                _solicitudTurnoService.CambiarEstatusSolicitud(numeroTurno, nuevoEstatus);

                MessageBox.Show("Estatus actualizado correctamente", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                BuscarSolicitud("");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al cambiar estatus");
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}