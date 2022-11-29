using Blazor.BusinessLogic;
using Blazor.Infrastructure.Entities;
using Blazor.WebApp.Models;
using DevExtreme.AspNet.Data;
using WidgetGallery;
using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
using Dominus.Frontend.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using Dominus.Backend.Data;
using System.Linq;
using Dominus.Backend.Application;
using WebApp.Reportes.Facturas;
using DevExpress.XtraReports.UI;
using WebApp.Reportes.FacturaDetalle;
using DevExpress.XtraSpreadsheet.Forms;

namespace Blazor.WebApp.Controllers
{

    [Authorize] 
    public partial class FacturasController : BaseAppController
    {

        //private const string Prefix = "Facturas"; 

        public FacturasController(IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(config, httpContextAccessor)
        {
        }

        #region Functions Master

        [HttpPost]
        public LoadResult Get(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(Manager().GetBusinessLogic<Facturas>().Tabla(true), loadOptions);
        }

        public IActionResult List()
        {
            return View("List");
        }

       

        public IActionResult ListPartial()
        {
            return PartialView("List");
        }

        [HttpGet]
        public IActionResult New()
        {
            return PartialView("Edit", NewModel());
        }

        private FacturasModel NewModel() 
        { 
            FacturasModel model = new FacturasModel();
            model.Entity.IsNew = true;
            return model; 
        } 

        [HttpGet]
        public IActionResult Edit(long Id)
        {
            return PartialView("Edit", EditModel(Id));
        }

        private FacturasModel EditModel(long Id) 
        { 
            FacturasModel model = new FacturasModel();
            model.Entity = Manager().GetBusinessLogic<Facturas>().FindById(x => x.Id == Id, true);
            //if ( !string.IsNullOrWhiteSpace( model.Entity.IdDbusiness) && model.Entity.IdDbusiness != Guid.Empty.ToString())
            //{
            //    string cleanHostName = DApp.GetTenantService(Request.Host.Host, "ElectronicInvoice");
            //    cleanHostName = cleanHostName.Replace("https://","").Replace("https://","");
            //    cleanHostName = cleanHostName.Split(":")[0];

            //    CallReport report = new CallReport
            //    {
            //        ReportName = "InvoiceReport",
            //        ConnectionName = cleanHostName
            //    };

            //    report.Parameters.Add("pLogo", model.Entity.Empresas.NumeroIdentificacion);
            //    report.Parameters.Add("pId", model.Entity.IdDbusiness);

            //    model.UrlReport = GetUrlReport(report);
            //}
            model.Entity.IsNew = false;
            return model; 
        } 

        [HttpPost]
        public IActionResult Edit(FacturasModel model)
        {
            return PartialView("Edit",EditModel(model));
        }

        private FacturasModel EditModel(FacturasModel model) 
        { 
            ViewBag.Accion = "Save"; 
            var OnState = model.Entity.IsNew; 
            if (ModelState.IsValid) 
            { 
                try 
                { 
                    model.Entity.LastUpdate = DateTime.Now; 
                    model.Entity.UpdatedBy = User.Identity.Name; 
                    if (model.Entity.IsNew) 
                    { 
                        model.Entity.CreationDate = DateTime.Now; 
                        model.Entity.CreatedBy = User.Identity.Name; 
                        model.Entity = Manager().GetBusinessLogic<Facturas>().Add(model.Entity); 
                        model.Entity.IsNew = false;
                    } 
                    else 
                    { 
                        model.Entity = Manager().GetBusinessLogic<Facturas>().Modify(model.Entity); 
                    } 
                } 
                catch (Exception e) 
                { 
                    ModelState.AddModelError("Entity.Id", e.GetFullErrorMessage()); 
                } 
            }
            else
            {
                ModelState.AddModelError("Entity.Id", $"Error en vista, diferencia con base de datos. | " + ModelState.GetFullErrorMessage());
            }
            return model; 
        } 

        [HttpPost]
        public IActionResult Delete(FacturasModel model)
        {
            return PartialView("Edit", DeleteModel(model));
        }

        private FacturasModel DeleteModel(FacturasModel model)
        { 
            ViewBag.Accion = "Delete"; 
            FacturasModel newModel = NewModel(); 
            if (ModelState.IsValid) 
            { 
                try 
                { 
                    model.Entity = Manager().GetBusinessLogic<Facturas>().FindById(x => x.Id == model.Entity.Id, false); 
                    Manager().GetBusinessLogic<Facturas>().Remove(model.Entity); 
                    return newModel;
                } 
                catch (Exception e) 
                { 
                    ModelState.AddModelError("Entity.Id", e.GetFullErrorMessage()); 
                } 
            } 
            return model; 
        } 

        #endregion 

        #region Functions Detail 
        /*

        [HttpGet]
        public IActionResult NewDetail(long IdFather)
        {
            return PartialView("EditDetail", NewModelDetail(IdFather));
        }

        private FacturasModel NewModelDetail(long IdFather) 
        { 
            FacturasModel model = new FacturasModel(); 
            model.Entity.IdFather = IdFather; 
            model.Entity.IsNew = true; 
            return model; 
        } 

        [HttpGet]
        public IActionResult EditDetail(long Id)
        {
            return PartialView("EditDetail", EditModel(Id));
        }

        [HttpPost]
        public IActionResult EditDetail(FacturasModel model)
        {
            return PartialView("EditDetail",EditModel(model));
        }

        [HttpPost]
        public IActionResult DeleteDetail(FacturasModel model)
        {
            return PartialView("EditDetail", DeleteModelDetail(model));
        }

        private FacturasModel DeleteModelDetail(FacturasModel model)
        { 
            ViewBag.Accion = "Delete"; 
            FacturasModel newModel = NewModelDetail(model.Entity.IdFather); 
            if (ModelState.IsValid) 
            { 
                try 
                { 
                    model.Entity = Manager().GetBusinessLogic<Facturas>().FindById(x => x.Id == model.Entity.Id, false); 
                    Manager().GetBusinessLogic<Facturas>().Remove(model.Entity); 
                    return newModel;
                } 
                catch (Exception e) 
                { 
                    ModelState.AddModelError("Entity.Id", e.GetFullErrorMessage()); 
                } 
            } 
            return model; 
        } 

        #endregion 

        #region Funcions Detail Edit in Grid 

        [HttpPost] 
        public IActionResult AddInGrid(string values) 
        { 
             Facturas entity = new Facturas(); 
             JsonConvert.PopulateObject(values, entity); 
             FacturasModel model = new FacturasModel(); 
             model.Entity = entity; 
             model.Entity.IsNew = true; 
             this.EditModel(model); 
             if(ModelState.IsValid) 
                 return Ok(ModelState); 
             else 
                 return BadRequest(ModelState.GetFullErrorMessage()); 
        } 

        [HttpPost] 
        public IActionResult ModifyInGrid(int key, string values) 
        { 
             Facturas entity = Manager().GetBusinessLogic<Facturas>().FindById(x => x.Id == key, false); 
             JsonConvert.PopulateObject(values, entity); 
             FacturasModel model = new FacturasModel(); 
             model.Entity = entity; 
             model.Entity.IsNew = false; 
             this.EditModel(model); 
             if(ModelState.IsValid) 
                 return Ok(ModelState); 
             else 
                 return BadRequest(ModelState.GetFullErrorMessage()); 
        } 

        [HttpPost]
        public void DeleteInGrid(int key)
        { 
             Facturas entity = Manager().GetBusinessLogic<Facturas>().FindById(x => x.Id == key, false); 
             Manager().GetBusinessLogic<Facturas>().Remove(entity); 
        } 

        */
        #endregion 

        #region Datasource Combobox Foraneos 

        [HttpPost]
        public LoadResult GetEmpresasId(DataSourceLoadOptions loadOptions)
        { 
            return DataSourceLoader.Load(Manager().GetBusinessLogic<Empresas>().Tabla(true), loadOptions);
        }

        [HttpPost]
        public LoadResult GetFormasPagosId(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(Manager().GetBusinessLogic<FormasPagos>().Tabla(true), loadOptions);
        }

        [HttpPost]
        public LoadResult GetEntidadesId(DataSourceLoadOptions loadOptions)
        { 
            return DataSourceLoader.Load(Manager().GetBusinessLogic<Entidades>().Tabla(true), loadOptions);
        } 
        [HttpPost]
        public LoadResult GetEstadosid(DataSourceLoadOptions loadOptions)
        { 
            return DataSourceLoader.Load(Manager().GetBusinessLogic<Estados>().Tabla(true), loadOptions);
        } 
        [HttpPost]
        public LoadResult GetFilialesId(DataSourceLoadOptions loadOptions)
        { 
            return DataSourceLoader.Load(Manager().GetBusinessLogic<Filiales>().Tabla(true), loadOptions);
        } 
        [HttpPost]
        public LoadResult GetPacientesId(DataSourceLoadOptions loadOptions)
        { 
            return DataSourceLoader.Load(Manager().GetBusinessLogic<Pacientes>().Tabla(true), loadOptions);
        } 
        [HttpPost]
        public LoadResult GetSedesId(DataSourceLoadOptions loadOptions)
        { 
            return DataSourceLoader.Load(Manager().GetBusinessLogic<Sedes>().Tabla(true), loadOptions);
        }
        [HttpPost]
        public LoadResult GetDocumentosId(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(Manager().GetBusinessLogic<Documentos>().Tabla(true), loadOptions);
        }

        [HttpPost]
        public LoadResult GetTiposRegimenesId(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(Manager().GetBusinessLogic<TiposRegimenes>().Tabla(true), loadOptions);
        }

        [HttpPost]
        public LoadResult GetMediosPagoId(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(Manager().GetBusinessLogic<MediosPago>().Tabla(true), loadOptions);
        }

        #endregion

        [HttpGet]
        public IActionResult ImprimirReporteCartera(long entidadId)
        {
            try
            {
                if (entidadId <= 0)
                    throw new Exception("El parametro entidadId no fue enviado correctamente al servidor.");

                InformacionReporte informacionReporte = new InformacionReporte();
                informacionReporte.Empresa = Manager().GetBusinessLogic<Empresas>().FindById(x => x.Id == this.ActualEmpresaId(), true);
                informacionReporte.BD = DApp.GetTenantConnection(Request.Host.Value);
                var entidad = Manager().GetBusinessLogic<Entidades>().FindById(x => x.Id == entidadId,false);
                informacionReporte.ParametrosAdicionales.Add("p_Nit", entidad.NumeroIdentificacion);
                informacionReporte.ParametrosAdicionales.Add("p_UsuarioGenero", User.Identity.Name);

                CarteraReporte report = new CarteraReporte();
                report.SetInformacionReporte(informacionReporte);
                XtraReport xtraReport = report;
                return PartialView("_ViewerReport", report);

            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.GetFullErrorMessage());
            }
        }

        [HttpGet]
        public IActionResult ImprimirReporteCarteraGeneral()
        {
            try
            {
                InformacionReporte informacionReporte = new InformacionReporte();
                informacionReporte.Empresa = Manager().GetBusinessLogic<Empresas>().FindById(x => x.Id == this.ActualEmpresaId(), true);
                informacionReporte.BD = DApp.GetTenantConnection(Request.Host.Value);
                var user = Manager().GetBusinessLogic<User>().FindById(x => x.UserName == User.Identity.Name, false);
                informacionReporte.ParametrosAdicionales.Add("P_UsuarioGenero", $"{user.UserName} | {user.Names} {user.LastNames}");

                CarteraGeneralReporte report = new CarteraGeneralReporte();
                report.SetInformacionReporte(informacionReporte);
                XtraReport xtraReport = report;

                return PartialView("_ViewerReport", report);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.GetFullErrorMessage());
            }
        }

        [HttpGet]
        public IActionResult ImprimirInformeCartera(long entidadId)
        {
            try
            {
                if (entidadId <= 0)
                    throw new Exception("El parametro entidadId no fue enviado correctamente al servidor.");

                InformacionReporte informacionReporte = new InformacionReporte();
                informacionReporte.Empresa = Manager().GetBusinessLogic<Empresas>().FindById(x => x.Id == this.ActualEmpresaId(), true);
                informacionReporte.BD = DApp.GetTenantConnection(Request.Host.Value);
                var entidad = Manager().GetBusinessLogic<Entidades>().FindById(x => x.Id == entidadId, false);
                informacionReporte.ParametrosAdicionales.Add("p_Nit", entidad.NumeroIdentificacion);
                informacionReporte.ParametrosAdicionales.Add("p_UsuarioGenero", User.Identity.Name);

                CarteraReporte report = new CarteraReporte();
                report.SetInformacionReporte(informacionReporte);
                XtraReport xtraReport = report;
                return PartialView("_ViewerReport", report);

            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.GetFullErrorMessage());
            }
        }

        [HttpGet]
        public IActionResult XLSXInformeCartera(long entidadId, DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                if (entidadId <= 0 || fechaDesde.Year < 1900 || fechaHasta.Year < 1900)
                    throw new Exception("Los parametros Fecha Desde, Fecha Hasta y Entidad no fueron enviados correctamente al servidor.");

                fechaDesde = new DateTime(fechaDesde.Year, fechaDesde.Month, fechaDesde.Day, 0, 0, 0);
                fechaHasta = new DateTime(fechaHasta.Year, fechaHasta.Month, fechaHasta.Day, 23, 59, 59);
                byte[] book = Manager().FacturasBusinessLogic().ExcelInformeCartera(entidadId, fechaDesde, fechaHasta);
                return File(book, "application/octet-stream", $"Informe_Cartera_Por_Entidad_{fechaDesde.ToString("ddMMyyyy")}_{fechaHasta.ToString("ddMMyyyy")}.xlsx");
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.GetFullErrorMessage());
            }
        }

        [HttpGet]
        public IActionResult ImprimirInformeGeneralCartera(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                if (fechaDesde.Year < 1900 || fechaHasta.Year < 1900)
                    throw new Exception("Los parametros Fecha Desde, Fecha Hasta y Entidad no fueron enviados correctamente al servidor.");

                fechaDesde = new DateTime(fechaDesde.Year, fechaDesde.Month, fechaDesde.Day, 0, 0, 0);
                fechaHasta = new DateTime(fechaHasta.Year, fechaHasta.Month, fechaHasta.Day, 23, 59, 59);
                byte[] book = Manager().FacturasBusinessLogic().ExcelInformeGeneralCartera(fechaDesde, fechaHasta);
                return File(book, "application/octet-stream", $"Informe_General_Cartera_{fechaDesde.ToString("ddMMyyyy")}_{fechaHasta.ToString("ddMMyyyy")}.xlsx");
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.GetFullErrorMessage()); 
            }
        }

        [HttpGet]
        public IActionResult ImprimirAnexoFactura(int Id)
        {
            try
            {
                InformacionReporte informacionReporte = new InformacionReporte();
                informacionReporte.Empresa = Manager().GetBusinessLogic<Empresas>().FindById(x => x.Id == this.ActualEmpresaId(), true);
                informacionReporte.BD = DApp.GetTenantConnection(Request.Host.Value);
                informacionReporte.Ids = new long[] { Id };

                FacturaDetalleReporte report = new FacturaDetalleReporte(informacionReporte);
                XtraReport xtraReport = report;

                return PartialView("_ViewerReport", report);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.GetFullErrorMessage());
            }
        }
    }
}


