using System;
using Blazor.WebApp.Models;
using DevExpress.XtraReports.UI;
using WebApp.Reportes.Incapacidades;
using WebApp.Reportes.IndicacionesMedicas;
using WebApp.Reportes.OrdenesMedicamentos;
using WebApp.Reportes.OrdenesServicios;

namespace WebApp.Reportes.HistoriasClinicas
{
    public partial class DocumentosAPacientesReporte
    {
        private InformacionReporte InformacionReporte { get; set; }
        public DocumentosAPacientesReporte(InformacionReporte _informacionReporte)
        {
            this.InformacionReporte = _informacionReporte;
            InitializeComponent();
        }

        protected override void OnReportInitialize()
        {
            this.P_Ids.Value = InformacionReporte.Ids[0];
            this.P_Ids.Visible = false;
            base.OnReportInitialize();
        }

        protected override void OnDataSourceDemanded(EventArgs e)
        {
            this.FuenteDatos.ConnectionParameters = InformacionReporte.DataConnectionParametersBase;

            var subReporteIncapacidades = new IncapacidadesReporte(InformacionReporte, true);
            subReporteIncapacidades.SetSubReporte(InformacionReporte,true);
            this.IncapacidadesSubReporte.ReportSource = subReporteIncapacidades;

            var subReporteIndicacionesMedicas = new IndicacionesMedicasReporte(InformacionReporte, true);
            subReporteIndicacionesMedicas.SetSubReporte(InformacionReporte,true);
            this.IndicacionesMedicasSubReporte.ReportSource = subReporteIndicacionesMedicas;

            var subReporteOrdenesMedicamentos = new OrdenesMedicamentosReporte(InformacionReporte, true);
            subReporteOrdenesMedicamentos.SetSubReporte(InformacionReporte, true);
            this.OrdenesMedicamentosSubReporte.ReportSource = subReporteOrdenesMedicamentos;

            var subReporteOrdenesServicios = new OrdenesServiciosReporte(InformacionReporte,true);
            subReporteOrdenesServicios.SetSubReporte(InformacionReporte, true);
            this.OrdenesServiciosSubReporte.ReportSource = subReporteOrdenesServicios;
        }
    }
}
