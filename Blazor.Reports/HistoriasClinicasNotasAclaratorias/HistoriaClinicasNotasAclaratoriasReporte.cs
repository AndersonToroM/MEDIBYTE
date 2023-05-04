using Blazor.BusinessLogic.Models;
using Blazor.Reports.HistoriasClinicas;

namespace Blazor.Reports.HistoriaClinicasNotasAclaratorias
{
    public partial class HistoriaClinicasNotasAclaratoriasReporte
    {
        private ReporteModel InformacionReporte { get; set; }
        public HistoriaClinicasNotasAclaratoriasReporte(ReporteModel _informacionReporte)
        {
            this.InformacionReporte = _informacionReporte;
            InitializeComponent();
        }

        protected override void OnReportInitialize()
        {
            this.P_Ids.Value = InformacionReporte.Ids;
            this.P_UsuarioGenero.Value = InformacionReporte.ParametrosAdicionales["P_UsuarioGenero"];
            this.logoEmpresa.ImageSource = InformacionReporte.LogoEmpresa;
            this.P_Ids.Visible = false;
            base.OnReportInitialize();
        }

        protected override void OnDataSourceDemanded(EventArgs e)
        {
            this.FuenteDatos.ConnectionParameters = InformacionReporte.DataConnectionParametersBase;

            var subReporteDiagnosticos = new HistoriasClinicasDiagnosticosSubReporte();
            subReporteDiagnosticos.SetConnectionParameters(this.FuenteDatos.ConnectionParameters);
            this.HistoriasClinicasDiagnosticosSubReporte.ReportSource = subReporteDiagnosticos;

        }
    }
}
