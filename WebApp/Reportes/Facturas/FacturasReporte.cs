using System;
using Blazor.WebApp.Models;

namespace WebApp.Reportes.Facturas
{
    public partial class FacturasReporte
    {
        private InformacionReporte InformacionReporte { get; set; }
        public FacturasReporte(InformacionReporte _informacionReporte)
        {
            this.InformacionReporte = _informacionReporte;
            InitializeComponent();
        }

        protected override void OnReportInitialize()
        {
            this.P_Ids.Value = InformacionReporte.Ids;
            this.logoEmpresa.ImageSource = InformacionReporte.LogoEmpresa;
            this.P_Ids.Visible = false;
            base.OnReportInitialize();
        }

        protected override void OnDataSourceDemanded(EventArgs e)
        {
            this.FuenteDatos.ConnectionParameters = InformacionReporte.DataConnectionParametersBase;
        }
    }
}