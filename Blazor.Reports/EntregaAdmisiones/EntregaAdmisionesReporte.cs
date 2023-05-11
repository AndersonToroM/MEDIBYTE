using Blazor.BusinessLogic.Models;

namespace Blazor.Reports.EntregaAdmisiones
{
    public partial class EntregaAdmisionesReporte
    {
        private ReporteModel reportModel { get; set; }
        public EntregaAdmisionesReporte(ReporteModel reportModel)
        {
            this.reportModel = reportModel;
            InitializeComponent();
        }

        protected override void OnReportInitialize()
        {
            this.p_FechaDesde.Value = reportModel.ParametrosAdicionales["p_FechaDesde"];
            this.p_FechaHasta.Value = reportModel.ParametrosAdicionales["p_FechaHasta"];
            this.p_SedeId.Value = reportModel.ParametrosAdicionales["p_SedeId"];
            this.p_UsuarioGenero.Value = reportModel.ParametrosAdicionales["P_UsuarioGenero"];
            this.logoEmpresa.ImageSource = reportModel.LogoEmpresa;
            this.p_FechaDesde.Visible = false;
            this.p_FechaHasta.Visible = false;
            this.p_SedeId.Visible = false;
            this.p_UsuarioGenero.Visible = false;
            base.OnReportInitialize();
        }

        protected override void OnDataSourceDemanded(EventArgs e)
        {
            this.FuenteDatos.ConnectionParameters = reportModel.DataConnectionParametersBase;

            var entregaAdmisionesTotalesSubReporte = new EntregaAdmisionesTotalesSubReporte();
            entregaAdmisionesTotalesSubReporte.SetConnectionParameters(this.FuenteDatos.ConnectionParameters);
            this.EntregaAdmisionesTotalesSubReporte.ReportSource = entregaAdmisionesTotalesSubReporte;

        }
    }
}
