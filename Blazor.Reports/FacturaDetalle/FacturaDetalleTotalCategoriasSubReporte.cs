namespace Blazor.Reports.FacturaDetalle
{
    public partial class FacturaDetalleTotalCategoriasSubReporte
    {
        public FacturaDetalleTotalCategoriasSubReporte()
        {
            InitializeComponent();
        }
        public void SetConnectionParameters(DevExpress.DataAccess.ConnectionParameters.DataConnectionParametersBase connectionParameters)
        {
            this.FuenteDatos.ConnectionParameters = connectionParameters;
        }
    }
}
