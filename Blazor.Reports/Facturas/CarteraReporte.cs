using Blazor.BusinessLogic.Models;

namespace Blazor.Reports.Facturas
{
    public partial class CarteraReporte
    {
        private ReporteModel InformacionReporte { get; set; }
        public void SetInformacionReporte(ReporteModel informacionReporte)
        {
            this.InformacionReporte = informacionReporte;
        }
        public CarteraReporte(ReporteModel informacionReporte)
        {
            this.InformacionReporte = informacionReporte;
            InitializeComponent();
        }
        protected override void BeforeReportPrint()
        {
            this.p_Nit.Value = InformacionReporte.ParametrosAdicionales["p_Nit"];
            this.p_UsuarioGenero.Value = InformacionReporte.ParametrosAdicionales["P_UsuarioGenero"];
            this.logoEmpresa.ImageSource = InformacionReporte.LogoEmpresa;
            base.BeforeReportPrint();
        }

        protected override void OnReportInitialize()
        {
            this.p_Nit.Visible = false;
            this.p_UsuarioGenero.Visible = false;
            base.OnReportInitialize();
        }

        protected override void OnDataSourceDemanded(EventArgs e)
        {
            this.FuenteDatos.ConnectionParameters = InformacionReporte.DataConnectionParametersBase;
        }
    }
}
