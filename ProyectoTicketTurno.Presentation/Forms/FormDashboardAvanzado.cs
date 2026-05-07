using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Serilog;
using ProyectoTicketTurno.Business.Services;
using ProyectoTicketTurno.Business.Models;
using ProyectoTicketTurno.Data.Context;
using ProyectoTicketTurno.Data.Repositories;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.WindowsForms;

namespace ProyectoTicketTurno.Presentation
{
    public partial class FormDashboardAvanzado : Form
    {
        private readonly ILogger _logger;
        private readonly IDashboardService _dashboardService;
        private readonly ISolicitudTurnoRepository _solicitudTurnoRepository;

        public FormDashboardAvanzado(ILogger logger)
        {
            InitializeComponent();
            _logger = logger;

            var context = new AplicacionDbContext();
            _solicitudTurnoRepository = new SolicitudTurnoRepository(context);
            _dashboardService = new DashboardService(_solicitudTurnoRepository, logger);

            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "Dashboard - Estadísticas de Solicitudes";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new System.Drawing.Size(1000, 750);
            this.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
        }

        private void InitializeComponent()
        {
            var panelEncabezado = new Panel();
            panelEncabezado.Dock = DockStyle.Top;
            panelEncabezado.Height = 80;
            panelEncabezado.BackColor = System.Drawing.Color.FromArgb(33, 150, 243);

            var labelTitulo = new Label();
            labelTitulo.Text = "DASHBOARD - ESTADÍSTICAS DE SOLICITUDES";
            labelTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            labelTitulo.ForeColor = System.Drawing.Color.White;
            labelTitulo.Location = new System.Drawing.Point(20, 15);
            labelTitulo.Size = new System.Drawing.Size(600, 30);

            var labelFecha = new Label();
            labelFecha.Text = $"Actualizado: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
            labelFecha.Font = new System.Drawing.Font("Segoe UI", 9F);
            labelFecha.ForeColor = System.Drawing.Color.White;
            labelFecha.Location = new System.Drawing.Point(20, 45);
            labelFecha.Size = new System.Drawing.Size(400, 20);

            panelEncabezado.Controls.AddRange(new Control[] { labelTitulo, labelFecha });

            // Panel contenido
            var panelContenido = new Panel();
            panelContenido.Dock = DockStyle.Fill;
            panelContenido.AutoScroll = true;
            panelContenido.BackColor = System.Drawing.Color.White;

            int yPos = 15;

            // Panel de filtros
            var panelFiltros = new Panel();
            panelFiltros.Height = 60;
            panelFiltros.Location = new System.Drawing.Point(15, yPos);
            panelFiltros.Size = new System.Drawing.Size(950, 60);
            panelFiltros.BorderStyle = BorderStyle.FixedSingle;
            panelFiltros.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);

            var labelFiltro = new Label();
            labelFiltro.Text = "Filtrar por Municipio:";
            labelFiltro.Location = new System.Drawing.Point(15, 12);
            labelFiltro.Size = new System.Drawing.Size(150, 20);
            labelFiltro.Font = new System.Drawing.Font("Segoe UI", 10F);

            var comboBoxMunicipio = new ComboBox();
            comboBoxMunicipio.Name = "comboBoxMunicipio";
            comboBoxMunicipio.Location = new System.Drawing.Point(170, 12);
            comboBoxMunicipio.Size = new System.Drawing.Size(280, 25);
            comboBoxMunicipio.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxMunicipio.Items.AddRange(new object[] {
                "Todos", "Saltillo", "Torreón", "Monclova", "Parras", "Matamoros",
                "Acuña", "Frontera", "Castaños", "Nadadores", "Escobedo"
            });
            comboBoxMunicipio.SelectedIndex = 0;
            comboBoxMunicipio.Font = new System.Drawing.Font("Segoe UI", 10F);

            var btnActualizar = new Button();
            btnActualizar.Text = "Actualizar Gráfica";
            btnActualizar.Location = new System.Drawing.Point(470, 12);
            btnActualizar.Size = new System.Drawing.Size(150, 25);
            btnActualizar.BackColor = System.Drawing.Color.FromArgb(76, 175, 80);
            btnActualizar.ForeColor = System.Drawing.Color.White;
            btnActualizar.FlatStyle = FlatStyle.Flat;
            btnActualizar.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnActualizar.Click += (s, e) => ActualizarGrafica(comboBoxMunicipio);

            var btnExportar = new Button();
            btnExportar.Text = "Exportar Datos";
            btnExportar.Location = new System.Drawing.Point(640, 12);
            btnExportar.Size = new System.Drawing.Size(150, 25);
            btnExportar.BackColor = System.Drawing.Color.FromArgb(255, 152, 0);
            btnExportar.ForeColor = System.Drawing.Color.White;
            btnExportar.FlatStyle = FlatStyle.Flat;
            btnExportar.Font = new System.Drawing.Font("Segoe UI", 10F);
            btnExportar.Click += (s, e) => ExportarDatos();

            panelFiltros.Controls.AddRange(new Control[] { labelFiltro, comboBoxMunicipio, btnActualizar, btnExportar });
            panelContenido.Controls.Add(panelFiltros);

            yPos = 85;

            // PlotView para gráfica principal
            var plotView = new PlotView();
            plotView.Name = "plotView";
            plotView.Location = new System.Drawing.Point(15, yPos);
            plotView.Size = new System.Drawing.Size(950, 350);
            plotView.BackColor = System.Drawing.Color.White;

            yPos += 370;

            // Panel de estadísticas
            var panelEstadisticas = new Panel();
            panelEstadisticas.Location = new System.Drawing.Point(15, yPos);
            panelEstadisticas.Size = new System.Drawing.Size(950, 100);
            panelEstadisticas.BorderStyle = BorderStyle.FixedSingle;
            panelEstadisticas.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);

            var labelEstadisticas = new Label();
            labelEstadisticas.Text = "ESTADÍSTICAS GENERALES";
            labelEstadisticas.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            labelEstadisticas.Location = new System.Drawing.Point(15, 10);
            labelEstadisticas.Size = new System.Drawing.Size(300, 20);

            var labelPendientes = new Label();
            labelPendientes.Name = "labelPendientes";
            labelPendientes.Text = "Solicitudes Pendientes: 0";
            labelPendientes.Font = new System.Drawing.Font("Segoe UI", 10F);
            labelPendientes.Location = new System.Drawing.Point(15, 35);
            labelPendientes.Size = new System.Drawing.Size(300, 20);
            labelPendientes.ForeColor = System.Drawing.Color.FromArgb(255, 152, 0);

            var labelResueltas = new Label();
            labelResueltas.Name = "labelResueltas";
            labelResueltas.Text = "Solicitudes Resueltas: 0";
            labelResueltas.Font = new System.Drawing.Font("Segoe UI", 10F);
            labelResueltas.Location = new System.Drawing.Point(15, 60);
            labelResueltas.Size = new System.Drawing.Size(300, 20);
            labelResueltas.ForeColor = System.Drawing.Color.FromArgb(76, 175, 80);

            var labelPorcentajePendientes = new Label();
            labelPorcentajePendientes.Name = "labelPorcentajePendientes";
            labelPorcentajePendientes.Text = "% Pendientes: 0%";
            labelPorcentajePendientes.Font = new System.Drawing.Font("Segoe UI", 10F);
            labelPorcentajePendientes.Location = new System.Drawing.Point(400, 35);
            labelPorcentajePendientes.Size = new System.Drawing.Size(300, 20);

            var labelPorcentajeResueltas = new Label();
            labelPorcentajeResueltas.Name = "labelPorcentajeResueltas";
            labelPorcentajeResueltas.Text = "% Resueltas: 0%";
            labelPorcentajeResueltas.Font = new System.Drawing.Font("Segoe UI", 10F);
            labelPorcentajeResueltas.Location = new System.Drawing.Point(400, 60);
            labelPorcentajeResueltas.Size = new System.Drawing.Size(300, 20);

            panelEstadisticas.Controls.AddRange(new Control[] {
                labelEstadisticas, labelPendientes, labelResueltas,
                labelPorcentajePendientes, labelPorcentajeResueltas
            });

            panelContenido.Controls.AddRange(new Control[] { plotView, panelEstadisticas });

            this.Controls.AddRange(new Control[] { panelContenido, panelEncabezado });

            // Cargar gráfica inicial
            ActualizarGrafica(comboBoxMunicipio);
        }

        private void ActualizarGrafica(ComboBox comboBoxMunicipio)
        {
            try
            {
                var municipio = comboBoxMunicipio.SelectedItem?.ToString();
                DashboardData datos;

                if (municipio == "Todos" || string.IsNullOrWhiteSpace(municipio))
                {
                    datos = _dashboardService.ObtenerDatosGenerales();
                }
                else
                {
                    datos = _dashboardService.ObtenerDatosPorMunicipio(municipio);
                }

                // Actualizar labels
                var labelPendientes = this.Controls.Find("labelPendientes", true).FirstOrDefault() as Label;
                var labelResueltas = this.Controls.Find("labelResueltas", true).FirstOrDefault() as Label;
                var labelPorcentajePendientes = this.Controls.Find("labelPorcentajePendientes", true).FirstOrDefault() as Label;
                var labelPorcentajeResueltas = this.Controls.Find("labelPorcentajeResueltas", true).FirstOrDefault() as Label;

                if (labelPendientes != null)
                    labelPendientes.Text = $"Solicitudes Pendientes: {datos.SolicitudesPendientes}";

                if (labelResueltas != null)
                    labelResueltas.Text = $"Solicitudes Resueltas: {datos.SolicitudesResueltas}";

                if (labelPorcentajePendientes != null)
                    labelPorcentajePendientes.Text = $"% Pendientes: {datos.PorcentajePendientes}%";

                if (labelPorcentajeResueltas != null)
                    labelPorcentajeResueltas.Text = $"% Resueltas: {datos.PorcentajeResueltas}%";

                // Crear gráfica
                var plotModel = new PlotModel { Title = $"Estado de Solicitudes - {municipio}" };
                plotModel.DefaultFont = "Arial";
                plotModel.DefaultFontSize = 12;

                var pieSeries = new PieSeries();
                pieSeries.Slices.Add(new PieSlice
                {
                    Label = $"Pendientes: {datos.SolicitudesPendientes}",
                    Value = datos.SolicitudesPendientes > 0 ? datos.SolicitudesPendientes : 1,
                    Fill = OxyColor.FromRgb(255, 152, 0)
                });
                pieSeries.Slices.Add(new PieSlice
                {
                    Label = $"Resueltas: {datos.SolicitudesResueltas}",
                    Value = datos.SolicitudesResueltas > 0 ? datos.SolicitudesResueltas : 1,
                    Fill = OxyColor.FromRgb(76, 175, 80)
                });

                plotModel.Series.Add(pieSeries);

                var plotView = this.Controls.Find("plotView", true).FirstOrDefault() as PlotView;
                if (plotView != null)
                    plotView.Model = plotModel;

                _logger.Information("Dashboard actualizado para municipio: {Municipio}", municipio ?? "Todos");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al actualizar gráfica del dashboard");
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportarDatos()
        {
            try
            {
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Archivos CSV (*.csv)|*.csv";
                saveFileDialog.FileName = $"DashboardExport_{DateTime.Now:yyyyMMdd_HHmmss}";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportarACSV(saveFileDialog.FileName);
                    MessageBox.Show("Datos exportados correctamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _logger.Information("Datos exportados a: {Ruta}", saveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al exportar datos");
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportarACSV(string rutaArchivo)
        {
            try
            {
                var solicitudes = _solicitudTurnoRepository.ObtenerTodos().ToList();

                using (var writer = new System.IO.StreamWriter(rutaArchivo))
                {
                    writer.WriteLine("NumeroTurno,CURP,Municipio,FechaSolicitud,Estatus");

                    foreach (var solicitud in solicitudes)
                    {
                        writer.WriteLine($"\"{solicitud.NumeroTurno}\",\"{solicitud.CURP}\",\"{solicitud.Municipio}\",\"{solicitud.FechaSolicitud:yyyy-MM-dd HH:mm:ss}\",\"{solicitud.Estatus}\"");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error durante la exportación a CSV");
                throw;
            }
        }
    }
}