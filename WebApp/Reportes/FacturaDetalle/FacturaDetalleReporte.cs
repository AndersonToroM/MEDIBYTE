using System;
using Blazor.WebApp.Models;
using DevExpress.XtraReports.UI;

namespace WebApp.Reportes.FacturaDetalle
{
    public partial class FacturaDetalleReporte
    {
        private InformacionReporte InformacionReporte { get; set; }
        public FacturaDetalleReporte(InformacionReporte _informacionReporte)
        {
            this.InformacionReporte = _informacionReporte;
            InitializeComponent();
        }

        protected override void OnReportInitialize()
        {
            this.P_Ids.Value = InformacionReporte.Ids;
            this.logoEmpresa.ImageSource = InformacionReporte.LogoEmpresa;
            this.P_Ids.Visible = false;
            base.OnReportInitialize();
        }

        protected override void OnDataSourceDemanded(EventArgs e)
        {
            this.FuenteDatos.ConnectionParameters = InformacionReporte.DataConnectionParametersBase;

            var subReporteFacturaDetalleTotalCategorias = new FacturaDetalleTotalCategoriasSubReporte();
            subReporteFacturaDetalleTotalCategorias.SetConnectionParameters(this.FuenteDatos.ConnectionParameters);
            this.FacturaDetalleTotalCategoriasSubReporte.ReportSource = subReporteFacturaDetalleTotalCategorias;

        }
    }
}
