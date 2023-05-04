namespace Blazor.Reports.HistoriasClinicas
{
    public partial class HistoriasClinicasIndicacionesMedicasSubReporte
    {
        public HistoriasClinicasIndicacionesMedicasSubReporte()
        {
            InitializeComponent();
        }

        public void SetConnectionParameters(DevExpress.DataAccess.ConnectionParameters.DataConnectionParametersBase connectionParameters)
        {
            this.FuenteDatos.ConnectionParameters = connectionParameters;
        }
    }
}
