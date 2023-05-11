namespace Blazor.Reports.EntregaAdmisiones
{
    public partial class EntregaAdmisionesTotalesSubReporte
    {
        public EntregaAdmisionesTotalesSubReporte()
        {
            InitializeComponent();
        }

        public void SetConnectionParameters(DevExpress.DataAccess.ConnectionParameters.DataConnectionParametersBase connectionParameters)
        {
            this.FuenteDatos.ConnectionParameters = connectionParameters;
        }
    }
}
