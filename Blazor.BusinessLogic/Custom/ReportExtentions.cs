using Blazor.BusinessLogic.Models;
using Blazor.Infrastructure;
using Blazor.Infrastructure.Entities;
using Blazor.Reports.AtencionesResultado;
using Blazor.Reports.AtencionNotaProcedimientos;
using Blazor.Reports.EntregaAdmisiones;
using Blazor.Reports.FacturaDetalle;
using Blazor.Reports.Facturas;
using Blazor.Reports.HistoriaClinicasNotasAclaratorias;
using Blazor.Reports.HistoriasClinicas;
using Blazor.Reports.Incapacidades;
using Blazor.Reports.IndicacionesMedicas;
using Blazor.Reports.LiquidacionHonorarios;
using Blazor.Reports.Notas;
using Blazor.Reports.OrdenesMedicamentos;
using Blazor.Reports.OrdenesServicios;
using Blazor.Reports.RadicacionCuentas;
using DevExpress.XtraReports.UI;
using Dominus.Backend.Application;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Blazor.BusinessLogic
{
    public static class ReportExtentions
    {
        #region sobrecargas

        public static XtraReport Report<T>(this Dominus.Backend.DataBase.BusinessLogic logic, string userName = null, Dictionary<string, object> parametros = null)
        {
            return Report<T>(logic, null, userName, parametros);
        }

        public static XtraReport Report<T>(this Dominus.Backend.DataBase.BusinessLogic logic, long id, string userName = null, Dictionary<string, object> parametros = null)
        {
            return Report<T>(logic, new long[] { id }, userName, parametros);
        }

        #endregion

        public static XtraReport Report<T>(this Dominus.Backend.DataBase.BusinessLogic logic, long[] ids, string userName = null, Dictionary<string, object> parametros = null)
        {
            #region parametrizacion

            BlazorUnitWork unitOfWork = new BlazorUnitWork(logic.settings);
            ReporteModel reportModel = new ReporteModel();
            reportModel.BD = logic.settings;
            reportModel.Empresa = unitOfWork.Repository<Empresas>().Table.Include(x => x.LogoArchivos).First();
            if (ids != null && ids.Count() > 0)
            {
                reportModel.Ids = ids;
            }
            var p_UsuarioGenero = string.Empty;
            if (string.IsNullOrWhiteSpace(userName) || userName.Equals(DApp.Util.UserSystem))
            {
                p_UsuarioGenero = DApp.Util.UserSystem;
            }
            else
            {
                User user = unitOfWork.Repository<User>().FindById(x => string.Equals(x.UserName, userName), false);
                p_UsuarioGenero = $"{user.UserName} | {user.Names} {user.LastNames}";
            }
            if (parametros != null && parametros.Count > 0)
            {
                reportModel.ParametrosAdicionales = parametros;
            }

            reportModel.ParametrosAdicionales.Add("P_UsuarioGenero", p_UsuarioGenero);

            #endregion

            var type = typeof(T).Name;
            switch (type)
            {
                case nameof(AtencionesResultadoReporte):
                    return new AtencionesResultadoReporte(reportModel);
                case nameof(AtencionNotaProcedimientosReporte):
                    return new AtencionNotaProcedimientosReporte(reportModel);
                case nameof(EntregaAdmisionesReporte):
                    return new EntregaAdmisionesReporte(reportModel);
                case nameof(FacturaDetalleReporte):
                    return new FacturaDetalleReporte(reportModel);
                case nameof(CarteraGeneralReporte):
                    return new CarteraGeneralReporte(reportModel);
                case nameof(CarteraReporte):
                    return new CarteraReporte(reportModel);
                case nameof(FacturasParticularReporte):
                    return new FacturasParticularReporte(reportModel);
                case nameof(FacturasReporte):
                    return new FacturasReporte(reportModel);
                case nameof(TotalCarteraReporte):
                    return new TotalCarteraReporte(reportModel);
                case nameof(DocumentosAPacientesReporte):
                    return new DocumentosAPacientesReporte(reportModel);
                case nameof(HistoriasClinicasReporte):
                    return new HistoriasClinicasReporte(reportModel);
                case nameof(HistoriaClinicasNotasAclaratoriasReporte):
                    return new HistoriaClinicasNotasAclaratoriasReporte(reportModel);
                case nameof(IncapacidadesReporte):
                    return new IncapacidadesReporte(reportModel);
                case nameof(IndicacionesMedicasReporte):
                    return new IndicacionesMedicasReporte(reportModel);
                case nameof(LiquidacionHonorariosReporte):
                    return new LiquidacionHonorariosReporte(reportModel);
                case nameof(NotasReporte):
                    return new NotasReporte(reportModel);
                case nameof(OrdenesMedicamentosReporte):
                    return new OrdenesMedicamentosReporte(reportModel);
                case nameof(OrdenesServiciosReporte):
                    return new OrdenesServiciosReporte(reportModel);
                case nameof(RadicacionCuentasReporte):
                    return new RadicacionCuentasReporte(reportModel);
                default:
                    throw new System.Exception("Ningun reporte fue parametrizado. Revisar ReportExtentions");
            }
        }
    }
}