using System;
using Blazor.WebApp.Models;

namespace WebApp.Reportes.Notas
{
    public partial class NotasReporte
    {
        private InformacionReporte InformacionReporte { get; set; }
        public NotasReporte(InformacionReporte _informacionReporte)
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