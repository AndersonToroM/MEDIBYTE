using Blazor.BusinessLogic;
using Blazor.BusinessLogic.Models;
using Blazor.Infrastructure.Entities;
using Blazor.Reports.FacturaDetalle;
using Blazor.Reports.Facturas;
using Blazor.WebApp.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
using Dominus.Frontend.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WidgetGallery;

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
            return PartialView("Edit", EditModel(model));
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

                var entidad = Manager().GetBusinessLogic<Entidades>().FindById(x => x.Id == entidadId, false);
                var parametros = new Dictionary<string, object>
                {
                    {"p_Nit", entidad.NumeroIdentificacion }
                };
                var report = Manager().Report<CarteraReporte>(User.Identity.Name, parametros);
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
                var report = Manager().Report<CarteraGeneralReporte>(User.Identity.Name);
                return PartialView("_ViewerReport", report);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.GetFullErrorMessage());
            }
        }

        [HttpGet]
        public IActionResult DescargarInformeCartera(long entidadId)
        {
            try
            {
                if (entidadId <= 0)
                    throw new Exception("El parametro entidadId no fue enviado correctamente al servidor.");

                var entidad = Manager().GetBusinessLogic<Entidades>().FindById(x => x.Id == entidadId, false);
                var parametros = new Dictionary<string, object>
                {
                    {"p_Nit", entidad.NumeroIdentificacion }
                };
                var report = Manager().Report<CarteraReporte>(User.Identity.Name, parametros);
                return PartialView("_ViewerReport", report);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.GetFullErrorMessage());
            }
        }

        [HttpGet]
        public IActionResult ExcelInformeCarteraEntidad(long entidadId, DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                if (entidadId <= 0 || fechaDesde.Year < 2000 || fechaHasta > DateTime.Now)
                    throw new Exception("Los parametros Fecha Desde, Fecha Hasta y Entidad no fueron enviados correctamente al servidor.");

                fechaDesde = new DateTime(fechaDesde.Year, fechaDesde.Month, fechaDesde.Day, 0, 0, 0);
                fechaHasta = new DateTime(fechaHasta.Year, fechaHasta.Month, fechaHasta.Day, 23, 59, 59);
                byte[] book = Manager().FacturasBusinessLogic().ExcelInformeCartera(entidadId, fechaDesde, fechaHasta);
                return File(book, "application/octet-stream", $"Informe_Facturacion_Por_Entidad_{fechaDesde.ToString("ddMMyyyy")}_{fechaHasta.ToString("ddMMyyyy")}.xlsx");
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.GetFullErrorMessage());
            }
        }

        [HttpGet]
        public IActionResult ExportarInformeGeneralCartera(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                if (fechaDesde.Year < 2000 || fechaHasta > DateTime.Now)
                    throw new Exception("Los parametros Fecha Desde, Fecha Hasta y Entidad no fueron enviados correctamente al servidor.");

                fechaDesde = new DateTime(fechaDesde.Year, fechaDesde.Month, fechaDesde.Day, 0, 0, 0);
                fechaHasta = new DateTime(fechaHasta.Year, fechaHasta.Month, fechaHasta.Day, 23, 59, 59);
                byte[] book = Manager().FacturasBusinessLogic().ExcelInformeGeneralCartera(fechaDesde, fechaHasta);
                return File(book, "application/octet-stream", $"Informe_General_Facturacion_{fechaDesde.ToString("ddMMyyyy")}_{fechaHasta.ToString("ddMMyyyy")}.xlsx");
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.GetFullErrorMessage());
            }
        }

        [HttpGet]
        public IActionResult ExportarSiigo(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                if (fechaDesde.Year < 2000 || fechaHasta > DateTime.Now)
                    throw new Exception("Los parametros Fecha Desde y Fecha Hasta no fueron enviados correctamente al servidor.");

                fechaDesde = new DateTime(fechaDesde.Year, fechaDesde.Month, fechaDesde.Day, 0, 0, 0);
                fechaHasta = new DateTime(fechaHasta.Year, fechaHasta.Month, fechaHasta.Day, 23, 59, 59);
                byte[] book = Manager().FacturasBusinessLogic().ExcelExportarSiigo(fechaDesde, fechaHasta);
                return File(book, "application/octet-stream", $"Exportar_SIIGO_{fechaDesde.ToString("ddMMyyyy")}_{fechaHasta.ToString("ddMMyyyy")}.xlsx");
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
                var report = Manager().Report<FacturaDetalleReporte>(Id, User.Identity.Name);
                return PartialView("_ViewerReport", report);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.GetFullErrorMessage());
            }
        }


        [HttpGet]
        public IActionResult ImprimirFacturaPorId(int Id)
        {
            try
            {
                Facturas factura = Manager().GetBusinessLogic<Facturas>().FindById(x => x.Id == Id, false);
                var parametrosGenerales = Manager().GetBusinessLogic<ParametrosGenerales>().Tabla().FirstOrDefault();

                if (parametrosGenerales == null || string.IsNullOrWhiteSpace(parametrosGenerales.LinkVerificacionDIAN))
                {
                    throw new Exception("El link de validación DIAN no se encuentra parametrizado en el sistema.");
                }

                var parametrosReporte = new Dictionary<string, object>
                {
                    {"p_LinkValidacionDIAN", parametrosGenerales.LinkVerificacionDIAN }
                };
                if (factura.AdmisionesId != null)
                {
                    var report = Manager().Report<FacturasParticularReporte>(Id, User.Identity.Name, parametrosReporte);
                    return PartialView("_ViewerReport", report);
                }
                else
                {
                    var report = Manager().Report<FacturasReporte>(Id, User.Identity.Name, parametrosReporte);
                    return PartialView("_ViewerReport", report);
                }
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.GetFullErrorMessage());
            }

        }

        [HttpPost]
        public IActionResult ModificarDatosFactura(long id, string cambioOrdenCompra, string cambioReferenciaFactura, string cambioObservaciones)
        {
            try
            {
                FacturasModel facturasModel = new FacturasModel();
                var factura = Manager().GetBusinessLogic<Facturas>().FindById(x => x.Id == id, true);
                if (factura == null)
                {
                    throw new Exception("Factura no encontrada.");
                }
                factura.Fecha = DateTime.Now; //cambio la fecha de emisión a hoy;
                factura.OrdenCompra = cambioOrdenCompra;
                factura.ReferenciaFactura = cambioReferenciaFactura;
                factura.Observaciones = cambioObservaciones;
                facturasModel.Entity = Manager().GetBusinessLogic<Facturas>().Modify(factura);
                return Edit(id);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.GetFullErrorMessage());
            }

        }

        public IActionResult ListInterface()
        {
            return View("ListInterface");
        }

        [HttpPost]
        public IActionResult GenerateFileToCobol(List<long> ids)
        {
            try
            {
                string path = Manager().FacturasBusinessLogic().GenerateFileToCobol(ids);
                return Ok(path);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.GetFullErrorMessage());
            }
        }

        [HttpGet]
        public IActionResult DownloadZipFileToCobol(string path)
        {
            try
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                return File(fileBytes, "application/octet-stream", $"ArchivosSiesa85_{DateTime.Now.ToString("dd-MM-yyyy_HHmmss")}.zip");
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.GetFullErrorMessage());
            }
        }

        [HttpGet]
        public async Task<IActionResult> EnviarFacturaDIAN(long id)
        {
            try
            {
                IntegracionEnviarFEModel result = await Manager().FacturasBusinessLogic().EnviarFacturaDian(id, User.Identity.Name, Request.Host.Value);
                return Ok(result);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.GetFullErrorMessage());
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConsultarEstadoDocumentoDIAN(long id)
        {
            try
            {
                IntegracionEnviarFEModel result = await Manager().FacturasBusinessLogic().ConsultarEstadoDocumento(id, User.Identity.Name, Request.Host.Value);
                return Ok(result);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.GetFullErrorMessage());
            }
        }

        [HttpGet]
        public async Task<IActionResult> EnviarEmailFactura(long id)
        {
            try
            {
                await Manager().FacturasBusinessLogic().EnviarEmail(id, "Envio Factura Manual", User.Identity.Name, Request.Host.Value);
                return Ok();
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.GetFullErrorMessage());
            }
        }

        [HttpGet]
        public async Task<ActionResult> DescargarXMLDIAN(int id)
        {
            try
            {
                var resultado = await Manager().FacturasBusinessLogic().GetArchivoXmlDIAN(id, User.Identity.Name, Request.Host.Value);
                return File(resultado.ContentBytes, resultado.ContentType, resultado.FileName);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.GetFullErrorMessage());
            }
        }

        [HttpGet]
        public async Task<ActionResult> EnviarRips(int id)
        {
            try
            {
                var resultado = await Manager().FacturasBusinessLogic().EnviarRips(id, User.Identity.Name, Request.Host.Value);
                return Ok(resultado);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.GetFullErrorMessage());
            }
        }

        [HttpGet]
        public ActionResult DescargarJsonRips(int id)
        {
            try
            {
                var resultado = Manager().FacturasBusinessLogic().GetJsonRipsFile(id);
                return File(resultado.Archivo, resultado.ContentType, resultado.Nombre);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.GetFullErrorMessage());
            }
        }

    }
}


