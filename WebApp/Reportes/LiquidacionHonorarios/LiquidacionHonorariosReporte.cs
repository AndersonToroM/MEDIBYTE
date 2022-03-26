using Blazor.WebApp.Models;
using System;

namespace WebApp.Reportes.LiquidacionHonorarios
{
    public partial class LiquidacionHonorariosReporte
    {
        private InformacionReporte InformacionReporte { get; set; }
        public LiquidacionHonorariosReporte(InformacionReporte informacionReporte)
        {
            this.InformacionReporte = informacionReporte;
            InitializeComponent();
        }

        protected override void OnReportInitialize()
        {
            this.P_Ids.Value = InformacionReporte.Ids;
            this.logoEmpresa.ImageSource = InformacionReporte.LogoEmpresa;
            this.P_UsuarioGenero.Value = InformacionReporte.ParametrosAdicionales["P_UsuarioGenero"];
            this.P_Ids.Visible = false;
            this.P_UsuarioGenero.Visible = false;
            base.OnReportInitialize();
        }

        protected override void OnDataSourceDemanded(EventArgs e)
        {
            this.FuenteDatos.ConnectionParameters = InformacionReporte.DataConnectionParametersBase;
        }
    }
}
