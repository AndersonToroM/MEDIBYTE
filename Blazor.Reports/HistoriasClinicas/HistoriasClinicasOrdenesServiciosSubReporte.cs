namespace Blazor.Reports.HistoriasClinicas
{
    public partial class HistoriasClinicasOrdenesServiciosSubReporte
    {
        public HistoriasClinicasOrdenesServiciosSubReporte()
        {
            InitializeComponent();
        }

        public void SetConnectionParameters(DevExpress.DataAccess.ConnectionParameters.DataConnectionParametersBase connectionParameters)
        {
            this.FuenteDatos.ConnectionParameters = connectionParameters;
        }
    }
}
