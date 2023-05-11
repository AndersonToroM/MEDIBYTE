using Blazor.BusinessLogic.Models;
using Blazor.Reports.Incapacidades;
using Blazor.Reports.IndicacionesMedicas;
using Blazor.Reports.OrdenesMedicamentos;
using Blazor.Reports.OrdenesServicios;

namespace Blazor.Reports.HistoriasClinicas
{
    public partial class DocumentosAPacientesReporte
    {
        private ReporteModel InformacionReporte { get; set; }
        public DocumentosAPacientesReporte(ReporteModel _informacionReporte)
        {
            this.InformacionReporte = _informacionReporte;
            InitializeComponent();
        }

        protected override void OnReportInitialize()
        {
            this.P_Ids.Value = InformacionReporte.Ids[0];
            this.P_Ids.Visible = false;
            base.OnReportInitialize();
        }

        protected override void OnDataSourceDemanded(EventArgs e)
        {
            this.FuenteDatos.ConnectionParameters = InformacionReporte.DataConnectionParametersBase;

            var subReporteIncapacidades = new IncapacidadesReporte(InformacionReporte, true);
            subReporteIncapacidades.SetSubReporte(InformacionReporte,true);
            this.IncapacidadesSubReporte.ReportSource = subReporteIncapacidades;

            var subReporteIndicacionesMedicas = new IndicacionesMedicasReporte(InformacionReporte, true);
            subReporteIndicacionesMedicas.SetSubReporte(InformacionReporte,true);
            this.IndicacionesMedicasSubReporte.ReportSource = subReporteIndicacionesMedicas;

            var subReporteOrdenesMedicamentos = new OrdenesMedicamentosReporte(InformacionReporte, true);
            subReporteOrdenesMedicamentos.SetSubReporte(InformacionReporte, true);
            this.OrdenesMedicamentosSubReporte.ReportSource = subReporteOrdenesMedicamentos;

            var subReporteOrdenesServicios = new OrdenesServiciosReporte(InformacionReporte,true);
            subReporteOrdenesServicios.SetSubReporte(InformacionReporte, true);
            this.OrdenesServiciosSubReporte.ReportSource = subReporteOrdenesServicios;
        }
    }
}
