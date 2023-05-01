namespace Blazor.Reports.HistoriasClinicas
{
    public partial class HistoriasClinicasIncapacidadesSubReporte
    {
        public HistoriasClinicasIncapacidadesSubReporte()
        {
            InitializeComponent();
        }
        public void SetConnectionParameters(DevExpress.DataAccess.ConnectionParameters.DataConnectionParametersBase connectionParameters)
        {
            this.FuenteDatos.ConnectionParameters = connectionParameters;
        }
    }
}
