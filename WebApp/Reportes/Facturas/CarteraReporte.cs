using System;
using Blazor.WebApp.Models;
using DevExpress.XtraReports.UI;

namespace WebApp.Reportes.Facturas
{
    public partial class CarteraReporte
    {
        private InformacionReporte InformacionReporte { get; set; }
        public void SetInformacionReporte(InformacionReporte informacionReporte)
        {
            this.InformacionReporte = informacionReporte;
        }
        public CarteraReporte()
        {
            InitializeComponent();
        }
        protected override void BeforeReportPrint()
        {
            this.p_Nit.Value = InformacionReporte.ParametrosAdicionales["p_Nit"];
            this.p_UsuarioGenero.Value = InformacionReporte.ParametrosAdicionales["p_UsuarioGenero"];
            this.logoEmpresa.ImageSource = InformacionReporte.LogoEmpresa;
            base.BeforeReportPrint();
        }

        protected override void OnReportInitialize()
        {
            this.p_Nit.Visible = false;
            this.p_UsuarioGenero.Visible = false;
            base.OnReportInitialize();
        }

        protected override void OnDataSourceDemanded(EventArgs e)
        {
            this.FuenteDatos.ConnectionParameters = InformacionReporte.DataConnectionParametersBase;
        }
    }
}