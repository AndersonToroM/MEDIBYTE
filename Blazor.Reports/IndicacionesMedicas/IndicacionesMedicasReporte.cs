using Blazor.BusinessLogic.Models;

namespace Blazor.Reports.IndicacionesMedicas
{
    public partial class IndicacionesMedicasReporte
    {
        private ReporteModel InformacionReporte { get; set; }
        private bool IsFromRoot { get; set; }
        public void SetSubReporte(ReporteModel informacionReporte, bool isFromRoot = false)
        {
            this.InformacionReporte = informacionReporte;
            this.IsFromRoot = isFromRoot;
        }
        public IndicacionesMedicasReporte()
        {
            InitializeComponent();
        }

        public IndicacionesMedicasReporte(ReporteModel informacionReporte, bool isFromRoot = false)
        {
            this.InformacionReporte = informacionReporte;
            this.IsFromRoot = isFromRoot;
            InitializeComponent();
        } 

        protected override void OnReportInitialize()
        {
            if (InformacionReporte != null)
            {
                this.P_Ids.Value = InformacionReporte.Ids;
                if (IsFromRoot)
                {
                    this.P_Ids.Value = null;
                    this.P_HC_ID.Value = InformacionReporte.Ids[0];
                }
                this.logoEmpresa.ImageSource = InformacionReporte.LogoEmpresa;
                this.P_UsuarioGenero.Value = InformacionReporte.ParametrosAdicionales["P_UsuarioGenero"];
            }
            this.P_Ids.Visible = false;
            base.OnReportInitialize();
        }

        protected override void OnDataSourceDemanded(EventArgs e)
        {
            this.FuenteDatos.ConnectionParameters = InformacionReporte.DataConnectionParametersBase;
        }
    }
}
