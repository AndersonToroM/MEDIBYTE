using System;
using Blazor.WebApp.Models;
using DevExpress.XtraReports.UI;

namespace WebApp.Reportes.OrdenesServicios
{
    public partial class OrdenesServiciosReporte
    {
        private InformacionReporte InformacionReporte { get; set; }
        private bool IsFromRoot { get; set; }

        public OrdenesServiciosReporte()
        {
            InitializeComponent();
        }

        public void SetSubReporte(InformacionReporte informacionReporte, bool isFromRoot = false)
        {
            this.InformacionReporte = informacionReporte;
            this.IsFromRoot = isFromRoot;
        }

        public OrdenesServiciosReporte(InformacionReporte _informacionReporte, bool isFromRoot = false)
        {
            this.InformacionReporte = _informacionReporte;
            InitializeComponent();
        }

        protected override void OnReportInitialize()
        {
            if (InformacionReporte != null)
            {
                this.P_Ids.Value = InformacionReporte.Ids;
                if (IsFromRoot)
                {
                    this.P_Ids.Value = null;
                    this.P_HC_ID.Value = InformacionReporte.Ids[0];
                }
                this.logoEmpresa.ImageSource = InformacionReporte.LogoEmpresa;
                this.P_UsuarioGenero.Value = InformacionReporte.ParametrosAdicionales["P_UsuarioGenero"];
            }
            this.P_Ids.Visible = false;
            base.OnReportInitialize();
        }

        protected override void OnDataSourceDemanded(EventArgs e)
        {
            this.FuenteDatos.ConnectionParameters = InformacionReporte.DataConnectionParametersBase;
        }
    }
}
