using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Serilog;
using ProyectoTicketTurno.Business.Models;
using ProyectoTicketTurno.Business.Services;
using ProyectoTicketTurno.Data.Context;
using ProyectoTicketTurno.Data.Repositories;

namespace ProyectoTicketTurno.Presentation
{
    /// <summary>
    /// Formulario Dashboard para visualización de estadísticas de solicitudes de turno
    /// </summary>
    public partial class FormDashboard : Form
    {
        private readonly IDashboardService _dashboardService;
        private readonly ILogger _logger;
        private Chart _chartTorta;
        private Chart _chartBarras;
        private Chart _chartLinea;
        private DataGridView _dataGridViewDetalles;
        private Label _labelTotal;
        private Label _labelPendientes;
        private Label _labelResueltos;
        private ComboBox _combomunicipio;

        public FormDashboard()
        {
            InitializeComponent();

            // Inicializar servicios
            var context = new AplicacionDbContext();
            var solicitudTurnoRepository = new SolicitudTurnoRepository(context);
            var municipioRepository = new MunicipioRepository(context);
            _logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            _dashboardService = new DashboardService(solicitudTurnoRepository, municipioRepository, _logger);

            ConfigurarFormulario();
            CargarDatos();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "📊 Dashboard - Gestión de Solicitudes de Turno";
            this.Size = new Size(1400, 900);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.Font = new Font("Segoe UI", 9F);

            // Panel principal
            var panelPrincipal = new Panel();
            panelPrincipal.Dock = DockStyle.Fill;
            panelPrincipal.AutoScroll = true;
            this.Controls.Add(panelPrincipal);

            // Panel encabezado
            CrearPanelEncabezado(panelPrincipal);

            // Panel gráficos
            CrearPanelGraficos(panelPrincipal);

            // Panel grid de detalles
            CrearPanelDetalles(panelPrincipal);

            // Panel pie de página
            CrearPanelPie(panelPrincipal);
        }

        private void CrearPanelEncabezado(Panel panelPrincipal)
        {
            var panelEncabezado = new Panel()
            {
                Height = 100,
                Dock = DockStyle.Top,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Título
            var labelTitulo = new Label()
            {
                Text = "📊 DASHBOARD - ESTADO DE SOLICITUDES",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 51, 102),
                Location = new Point(20, 15),
                AutoSize = true
            };
            panelEncabezado.Controls.Add(labelTitulo);

            // ComboBox Municipio
            var labelMunicipio = new Label()
            {
                Text = "Municipio:",
                Location = new Point(20, 50),
                AutoSize = true
            };
            panelEncabezado.Controls.Add(labelMunicipio);

            _combomunicipio = new ComboBox()
            {
                Location = new Point(100, 48),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _combomunicipio.SelectedIndexChanged += (s, e) => RefrescarDatos();
            panelEncabezado.Controls.Add(_combomunicipio);

            // Botón Ver Total
            var btnTotal = new Button()
            {
                Text = "📋 Ver Total",
                Location = new Point(320, 48),
                Width = 100,
                Height = 25,
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnTotal.Click += (s, e) =>
            {
                _combomunicipio.SelectedIndex = -1;
                RefrescarDatos();
            };
            panelEncabezado.Controls.Add(btnTotal);

            // Botón Refrescar
            var btnRefrescar = new Button()
            {
                Text = "🔄 Refrescar",
                Location = new Point(430, 48),
                Width = 100,
                Height = 25,
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRefrescar.Click += (s, e) => RefrescarDatos();
            panelEncabezado.Controls.Add(btnRefrescar);

            // Estadísticas
            _labelTotal = new Label()
            {
                Text = "Total: 0",
                Location = new Point(800, 50),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                AutoSize = true
            };
            panelEncabezado.Controls.Add(_labelTotal);

            _labelPendientes = new Label()
            {
                Text = "Pendientes: 0",
                Location = new Point(900, 50),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.Red,
                AutoSize = true
            };
            panelEncabezado.Controls.Add(_labelPendientes);

            _labelResueltos = new Label()
            {
                Text = "Resueltos: 0",
                Location = new Point(1050, 50),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.Green,
                AutoSize = true
            };
            panelEncabezado.Controls.Add(_labelResueltos);

            panelPrincipal.Controls.Add(panelEncabezado);
        }

        private void CrearPanelGraficos(Panel panelPrincipal)
        {
            var panelGraficos = new Panel()
            {
                Height = 350,
                Dock = DockStyle.Top,
                Padding = new Padding(10),
                BackColor = Color.FromArgb(240, 240, 240)
            };

            // Gráfico Torta
            _chartTorta = new Chart()
            {
                Location = new Point(10, 10),
                Width = 420,
                Height = 330,
                BackColor = Color.White
            };
            _chartTorta.Series.Add("Estados");
            _chartTorta.Series["Estados"].ChartType = SeriesChartType.Pie;
            _chartTorta.Legends.Add("Leyenda");
            panelGraficos.Controls.Add(_chartTorta);

            // Gráfico Barras
            _chartBarras = new Chart()
            {
                Location = new Point(440, 10),
                Width = 420,
                Height = 330,
                BackColor = Color.White
            };
            _chartBarras.Series.Add("Pendientes");
            _chartBarras.Series.Add("Resueltos");
            _chartBarras.ChartAreas.Add(new ChartArea("ChartArea1"));
            panelGraficos.Controls.Add(_chartBarras);

            // Gráfico Línea
            _chartLinea = new Chart()
            {
                Location = new Point(870, 10),
                Width = 500,
                Height = 330,
                BackColor = Color.White
            };
            _chartLinea.Series.Add("Pendientes");
            _chartLinea.Series.Add("Resueltos");
            _chartLinea.Series["Pendientes"].ChartType = SeriesChartType.Line;
            _chartLinea.Series["Resueltos"].ChartType = SeriesChartType.Line;
            _chartLinea.ChartAreas.Add(new ChartArea("ChartArea1"));
            panelGraficos.Controls.Add(_chartLinea);

            panelPrincipal.Controls.Add(panelGraficos);
        }

        private void CrearPanelDetalles(Panel panelPrincipal)
        {
            var panelDetalles = new Panel()
            {
                Height = 300,
                Dock = DockStyle.Top,
                Padding = new Padding(10),
                BackColor = Color.FromArgb(240, 240, 240)
            };

            var labelDetalles = new Label()
            {
                Text = "📋 DETALLE DE SOLICITUDES",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 51, 102),
                Location = new Point(10, 5),
                AutoSize = true
            };
            panelDetalles.Controls.Add(labelDetalles);

            _dataGridViewDetalles = new DataGridView()
            {
                Location = new Point(10, 30),
                Width = 1360,
                Height = 260,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            };

            _dataGridViewDetalles.Columns.AddRange(
                new DataGridViewTextBoxColumn { Name = "NumeroTurno", HeaderText = "# Turno", Width = 80 },
                new DataGridViewTextBoxColumn { Name = "CURP", HeaderText = "CURP", Width = 150 },
                new DataGridViewTextBoxColumn { Name = "Municipio", HeaderText = "Municipio", Width = 120 },
                new DataGridViewTextBoxColumn { Name = "FechaSolicitud", HeaderText = "Fecha", Width = 130 },
                new DataGridViewTextBoxColumn { Name = "Asunto", HeaderText = "Asunto", Width = 250 },
                new DataGridViewTextBoxColumn { Name = "PersonaTramitera", HeaderText = "Persona", Width = 150 },
                new DataGridViewTextBoxColumn { Name = "Estatus", HeaderText = "Estado", Width = 100 }
            );

            panelDetalles.Controls.Add(_dataGridViewDetalles);
            panelPrincipal.Controls.Add(panelDetalles);
        }

        private void CrearPanelPie(Panel panelPrincipal)
        {
            var panelPie = new Panel()
            {
                Height = 40,
                Dock = DockStyle.Bottom,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var labelEstado = new Label()
            {
                Text = "✓ Dashboard cargado correctamente",
                Location = new Point(20, 12),
                ForeColor = Color.Green,
                Font = new Font("Segoe UI", 9F),
                AutoSize = true
            };
            panelPie.Controls.Add(labelEstado);

            panelPrincipal.Controls.Add(panelPie);
        }

        private void CargarDatos()
        {
            try
            {
                // Cargar municipios en ComboBox
                var municipios = _dashboardService.ObtenerMunicipios();
                _combomunicipio.Items.Clear();
                _combomunicipio.Items.Add("TOTAL");
                foreach (var municipio in municipios)
                {
                    _combomunicipio.Items.Add(municipio);
                }
                _combomunicipio.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al cargar datos iniciales");
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefrescarDatos()
        {
            try
            {
                string municipioSeleccionado = _combomunicipio.SelectedItem?.ToString();
                string municipioFiltro = (municipioSeleccionado == "TOTAL" || string.IsNullOrEmpty(municipioSeleccionado)) ? null : municipioSeleccionado;

                // Obtener estadísticas
                var estadisticas = string.IsNullOrEmpty(municipioFiltro)
                    ? _dashboardService.ObtenerEstadisticasGenerales()
                    : _dashboardService.ObtenerEstadisticasPorMunicipio(municipioFiltro);

                // Actualizar etiquetas
                _labelTotal.Text = $"Total: {estadisticas.Total}";
                _labelPendientes.Text = $"Pendientes: {estadisticas.SolicitudesPendientes} ({estadisticas.PorcentajePendientes}%)";
                _labelResueltos.Text = $"Resueltos: {estadisticas.SolicitudesResueltas} ({estadisticas.PorcentajeResueltas}%)";

                // Actualizar gráficos
                ActualizarGraficoTorta(municipioFiltro);
                ActualizarGraficoBarras();
                ActualizarGraficoLinea(municipioFiltro);
                ActualizarGridDetalles(municipioFiltro);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al refrescar datos del dashboard");
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarGraficoTorta(string municipio)
        {
            var datos = _dashboardService.ObtenerDatosEstados(municipio);
            _chartTorta.Series["Estados"].Points.Clear();

            foreach (var dato in datos)
            {
                var punto = _chartTorta.Series["Estados"].Points.AddXY(dato.Estado, dato.Cantidad);
                punto.Label = $"{dato.Estado}: {dato.Porcentaje}%";
            }

            _chartTorta.Series["Estados"].Points[0].Color = Color.FromArgb(255, 107, 107); // Rojo para Pendiente
            if (_chartTorta.Series["Estados"].Points.Count > 1)
                _chartTorta.Series["Estados"].Points[1].Color = Color.FromArgb(76, 175, 80); // Verde para Resuelto

            _chartTorta.Refresh();
        }

        private void ActualizarGraficoBarras()
        {
            var datos = _dashboardService.ObtenerDatosPorMunicipio();
            _chartBarras.Series["Pendientes"].Points.Clear();
            _chartBarras.Series["Resueltos"].Points.Clear();
            _chartBarras.Series["Pendientes"].ChartType = SeriesChartType.Column;
            _chartBarras.Series["Resueltos"].ChartType = SeriesChartType.Column;
            _chartBarras.Series["Pendientes"].Color = Color.FromArgb(255, 107, 107);
            _chartBarras.Series["Resueltos"].Color = Color.FromArgb(76, 175, 80);

            foreach (var dato in datos)
            {
                _chartBarras.Series["Pendientes"].Points.AddXY(dato.NombreMunicipio, dato.Pendientes);
                _chartBarras.Series["Resueltos"].Points.AddXY(dato.NombreMunicipio, dato.Resueltos);
            }

            _chartBarras.Refresh();
        }

        private void ActualizarGraficoLinea(string municipio)
        {
            var datos = _dashboardService.ObtenerDatosTendencia(municipio);
            _chartLinea.Series["Pendientes"].Points.Clear();
            _chartLinea.Series["Resueltos"].Points.Clear();
            _chartLinea.Series["Pendientes"].Color = Color.FromArgb(255, 107, 107);
            _chartLinea.Series["Resueltos"].Color = Color.FromArgb(76, 175, 80);
            _chartLinea.Series["Pendientes"].BorderWidth = 2;
            _chartLinea.Series["Resueltos"].BorderWidth = 2;

            foreach (var dato in datos)
            {
                _chartLinea.Series["Pendientes"].Points.AddXY(dato.Fecha, dato.Pendientes);
                _chartLinea.Series["Resueltos"].Points.AddXY(dato.Fecha, dato.Resueltos);
            }

            _chartLinea.Refresh();
        }

        private void ActualizarGridDetalles(string municipio)
        {
            var detalles = _dashboardService.ObtenerDetallesSolicitudes(municipio);
            _dataGridViewDetalles.Rows.Clear();

            foreach (var detalle in detalles)
            {
                _dataGridViewDetalles.Rows.Add(
                    detalle.NumeroTurno,
                    detalle.CURP,
                    detalle.Municipio,
                    detalle.FechaSolicitud.ToString("dd/MM/yyyy HH:mm"),
                    detalle.Asunto,
                    detalle.PersonaTramitera,
                    detalle.Estatus
                );
            }
        }
    }
}
