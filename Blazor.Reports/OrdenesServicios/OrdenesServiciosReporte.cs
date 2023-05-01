using Blazor.BusinessLogic.Models;

namespace Blazor.Reports.OrdenesServicios
{
    public partial class OrdenesServiciosReporte
    {
        private ReporteModel ReporteModel { get; set; }
        private bool IsFromRoot { get; set; }

        public OrdenesServiciosReporte()
        {
            InitializeComponent();
        }

        public void SetSubReporte(ReporteModel reporteModel, bool isFromRoot = false)
        {
            this.ReporteModel = reporteModel;
            this.IsFromRoot = isFromRoot;
        }

        public OrdenesServiciosReporte(ReporteModel reporteModel, bool isFromRoot = false)
        {
            this.ReporteModel = reporteModel;
            this.IsFromRoot = isFromRoot;
            InitializeComponent();
        }

        protected override void OnReportInitialize()
        {
            if (ReporteModel != null)
            {
                this.P_Ids.Value = ReporteModel.Ids;
                if (IsFromRoot)
                {
                    this.P_Ids.Value = null;
                    this.P_HC_ID.Value = ReporteModel.Ids[0];
                }
                this.logoEmpresa.ImageSource = ReporteModel.LogoEmpresa;
                this.P_UsuarioGenero.Value = ReporteModel.ParametrosAdicionales["P_UsuarioGenero"];
            }
            this.P_Ids.Visible = false;
            base.OnReportInitialize();
        }

        protected override void OnDataSourceDemanded(EventArgs e)
        {
            this.FuenteDatos.ConnectionParameters = ReporteModel.DataConnectionParametersBase;
        }
    }
}
