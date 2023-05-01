using Blazor.BusinessLogic.Models;

namespace Blazor.Reports.RadicacionCuentas
{
    public partial class RadicacionCuentasReporte
    {
        private ReporteModel ReporteModel { get; set; }
        public RadicacionCuentasReporte(ReporteModel informacionReporte)
        {
            this.ReporteModel = informacionReporte;
            InitializeComponent();
        }

        protected override void OnReportInitialize()
        {
            this.P_Ids.Value = ReporteModel.Ids;
            this.logoEmpresa.ImageSource = ReporteModel.LogoEmpresa;
            this.P_Ids.Visible = false;
            base.OnReportInitialize();
        }

        protected override void OnDataSourceDemanded(EventArgs e)
        {
            this.FuenteDatos.ConnectionParameters = ReporteModel.DataConnectionParametersBase;
        }
    }
}
