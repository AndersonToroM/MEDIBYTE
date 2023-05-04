namespace Blazor.Reports.HistoriasClinicas
{
    public partial class HistoriasClinicasRespuestasSubReporte
    {
        public HistoriasClinicasRespuestasSubReporte()
        {
            InitializeComponent();
        }

        public void SetConnectionParameters(DevExpress.DataAccess.ConnectionParameters.DataConnectionParametersBase connectionParameters)
        {
            this.FuenteDatos.ConnectionParameters = connectionParameters;
        }
    }
}
