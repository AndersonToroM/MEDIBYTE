namespace Blazor.Reports.HistoriasClinicas
{
    public partial class HistoriasClinicasOrdenesMedicamentosSubReporte
    {
        public HistoriasClinicasOrdenesMedicamentosSubReporte()
        {
            InitializeComponent();
        }

        public void SetConnectionParameters(DevExpress.DataAccess.ConnectionParameters.DataConnectionParametersBase connectionParameters)
        {
            this.FuenteDatos.ConnectionParameters = connectionParameters;
        }
    }
}
