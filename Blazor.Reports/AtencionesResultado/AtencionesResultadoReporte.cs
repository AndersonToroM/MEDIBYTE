using Blazor.BusinessLogic.Models;

namespace Blazor.Reports.AtencionesResultado
{
    public partial class AtencionesResultadoReporte
    {
        private ReporteModel reportModel { get; set; }
        public AtencionesResultadoReporte(ReporteModel reportModel)
        {
            this.reportModel = reportModel;
            InitializeComponent();
        }

        protected override void OnReportInitialize()
        {
            this.P_Ids.Value = reportModel.Ids;
            this.logoEmpresa.ImageSource = reportModel.LogoEmpresa;
            this.P_Ids.Visible = false;
            base.OnReportInitialize();
        }

        protected override void OnDataSourceDemanded(EventArgs e)
        {
            this.FuenteDatos.ConnectionParameters = reportModel.DataConnectionParametersBase;
        }
    }
}
