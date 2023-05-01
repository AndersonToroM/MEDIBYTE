namespace Blazor.Reports.HistoriasClinicas
{
    public partial class HistoriasClinicasDiagnosticosSubReporte
    {
        public HistoriasClinicasDiagnosticosSubReporte()
        {
            InitializeComponent();
        }
        public void SetConnectionParameters(DevExpress.DataAccess.ConnectionParameters.DataConnectionParametersBase connectionParameters)
        {
            this.FuenteDatos.ConnectionParameters = connectionParameters;
        }
    }
}
